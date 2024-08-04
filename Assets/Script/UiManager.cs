using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    //인스턴스화
    public static UiManager instance;

    //카메라
    CameraController cameraController;

    //캐릭터 상태
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerHPUI;
    public Slider playerGauge;
    public int chHP = 0;
    public float chGauge = 0;
    public string chName = "Mario";

    public TextMeshProUGUI enemyName;
    public TextMeshProUGUI enemyHPUI;
    public Slider enemyGauge;
    public int enHP = 0;
    public float enGauge = 0;
    public string enName = "Gunner";

    //캐릭터 P1, P2 아이콘
    public Canvas p1;
    public Canvas p2;

    //Player와 Enemy. UiManager.instance.player로 가져가면 됨.
    public GameObject enemy;
    public GameObject player;

    //카메라 포커싱 전용
    GameObject focus;
    GameObject defeat;
    Vector3 point;

    //비디오 이펙트
    public RawImage readyGo;
    public RawImage gameSet;

    //라이프 처리 관련
    public GameObject lifePlayer1;
    public GameObject lifePlayer2;
    public GameObject lifeEnemy1;
    public GameObject lifeEnemy2;
    int playerLife = 2;
    int enemyLife = 2;

    //이펙트 재생
    public GameObject effect;
    GameObject effectSpawn;

    //게임 종료 로직
    public bool gameEnd = false;
    bool playerWin = false; 

    //ReStartUI를 출력합니다.
    public GameObject restartUI;

    //Text 애니메이션
    Animator animatorPlayerText;
    Animator animatorEnemyText;

    //피니시 비디오 애니메이션
    public Canvas finishUI;
    
    //스테이지
    public GameObject stage;

    //게이지 애니메이션
    public Animator chGaugeColor;

    void Awake()
    {
        //인스턴스 선언 및 파괴
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //애니메이터 불러오기
        animatorPlayerText = playerHPUI.GetComponent<Animator>();
        animatorEnemyText = enemyHPUI.GetComponent<Animator>();

        //에너미와 플레이어 불러오기
        enemy = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        //레디 고 이펙트
        readyGo.gameObject.SetActive(true);

        //메인 카메라 불러오기
        cameraController = Camera.main.GetComponent<CameraController>();

        //플레이어 이름 UI 갱신
        playerName.text = chName;
        enemyName.text = enName;

        //HP 초기화
        playerHPUI.text = "0";
        enemyHPUI.text = "0";

        //게이지 초기화
        playerGauge.value = 0;
        enemyGauge.value = 0;
    }


    // Update is called once per frame
    void Update()
    {
        //P1 P2 아이콘 따라다니기
        p1.transform.position = player.transform.position;
        p2.transform.position = enemy.transform.position;
    }

    //라이프 차감 함수
    public void LifeSub(GameObject collision)
    {
        //플레이어 차감 처리
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerLife != 0)
            {
                StartCoroutine(effectStart(player.transform, enemy.transform));

                playerLife--;

                chHP = 0;
                HpRefresh(0, 0);

                collision.transform.position = new Vector3(0, 9, 0);
                if (playerLife == 0)
                {
                    lifePlayer1.gameObject.SetActive(false);
                }
                else if (playerLife == 1)
                {
                    lifePlayer2.gameObject.SetActive(false);
                }
            }
            else if (playerLife == 0)
            {
                //에너미 승리
                gameEnd = true;

                StartCoroutine(effectStart(player.transform, enemy.transform));
            }
        }

        //에너미 차감 처리
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (enemyLife != 0)
            {
                StartCoroutine(effectStart(enemy.transform, player.transform));

                enemyLife--;

                enHP = 0;
                HpRefresh(0, 0);

                collision.transform.position = new Vector3(0, 9, 0);
                if (enemyLife == 0)
                {
                    lifeEnemy1.gameObject.SetActive(false);
                }
                else if (enemyLife == 1)
                {
                    lifeEnemy2.gameObject.SetActive(false);
                }
            }
            else if (enemyLife == 0)
            {
                //마리오 승리
                gameEnd = true;

                playerWin = true;

                StartCoroutine(effectStart(enemy.transform, player.transform));
            }
        }
    }

    //추락 이펙트 발생
    private IEnumerator effectStart(Transform defeat, Transform focus)
    {
        //추락 이펙트 생성
        effectSpawn = Instantiate(effect);
        effectSpawn.transform.position = defeat.position;

        //이펙트 방향 뭔 짓을 해도 원하는대로 안나와서 강제로 보정
        effectSpawn.transform.LookAt(focus.transform);
        effectSpawn.transform.rotation *= Quaternion.Euler(new Vector3(100, 0, 0));

        //플레이어 승리 처리
        if (gameEnd == true && playerWin == true)
        {
            gameSet.gameObject.SetActive(true);

            yield return new WaitForSeconds(3.0f);

            SceneManager.LoadScene("Victory");
        }
        //에너미 승리 처리
        else if (gameEnd == true)
        {
            FocusCamera(focus);

            gameSet.gameObject.SetActive(true);

            yield return new WaitForSeconds(2.5f);

            Time.timeScale = 0;

            restartUI.SetActive(true);

            yield break;
        }
    }

    //마리오 궁극기 사용 시 카메라 포커싱
    public void FocusCamera(Transform focus)
    {
        point = focus.transform.position;

        cameraController.cameraTwoFocus = false;
        Camera.main.transform.position = new Vector3(point.x + 4, point.y + 2, -4);
        Camera.main.transform.Rotate(7, -36, 0);
    }

    //게이지 새로고침 및 추가 로직
    public void GaugeRefresh(float player, float enemy)
    {
        chGauge += player;
        enGauge += enemy;

        if(chGauge > 1.0f)
        {
            chGauge = 1.0f;
        }
        if (enGauge > 1.0f)
        {
            enGauge = 1.0f;
        }

        if(chGauge == 1)
        {
            chGaugeColor.SetBool("Anim", true);
        }
        else if(chGauge == 0)
        {
            chGaugeColor.SetBool("Anim", false);
        }

        playerGauge.value = chGauge;
        enemyGauge.value = enGauge;
    }

    //HP 새로고침 및 추가 로직
    public void HpRefresh(int player, int enemy)
    {
        chHP += player;
        enHP += enemy;

        if (chHP > 300)
        {
            chHP = 300;
        }
        if (enHP > 300)
        {
            enHP = 300;
        }

        TextAnimHit();

        HpColorCheck();

        playerHPUI.text = Convert.ToString(chHP);
        enemyHPUI.text = Convert.ToString(enHP);
    }

    //HP 컬러 변화
    void HpColorCheck()
    {
        if (chHP >= 200)
        {
            playerHPUI.color = Color.red;
        }
        else if (chHP >= 100)
        {
            playerHPUI.color = new Color(1f, 0.5f, 0, 1f);
        }
        else if (chHP >= 50)
        {
            playerHPUI.color = Color.yellow;
        }
        else
        {
            playerHPUI.color = Color.white;
        }

        if (enHP >= 200)
        {
            enemyHPUI.color = Color.red;
        }
        else if (enHP >= 100)
        {
            enemyHPUI.color = new Color(1f, 0.5f, 0, 1f);
        }
        else if (enHP >= 50)
        {
            enemyHPUI.color = Color.yellow;
        }
        else
        {
            enemyHPUI.color = Color.white;
        }
    }

    //HP 애니메이션 
    void TextAnimHit()
    {
        if (chHP != Convert.ToInt32(playerHPUI.text))
        {
            animatorPlayerText.SetTrigger("TextHit");
        }

        if (enHP != Convert.ToInt32(enemyHPUI.text))
        {
            animatorEnemyText.SetTrigger("TextHit");
        }
    }

    //멀리 날려보낼 시 발생하는 이펙트 함수
    public void FinishEffect(Transform ts)
    {
        finishUI.gameObject.SetActive(true);
        stage.gameObject.SetActive(false);

        Time.timeScale = 0.5f;

        finishUI.transform.position = new Vector3(ts.position.x, ts.position.y, 7);

        StartCoroutine(FinishStart());
    }

    //슬로우 모션 코루틴
    IEnumerator FinishStart()
    {
        yield return new WaitForSeconds(0.4f);

        Time.timeScale = 1.0f;

        finishUI.gameObject.SetActive(false);

        stage.gameObject.SetActive(true);
    }
}
