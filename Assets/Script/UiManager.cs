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
    //�ν��Ͻ�ȭ
    public static UiManager instance;

    //ī�޶�
    CameraController cameraController;

    //ĳ���� ����
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

    //ĳ���� P1, P2 ������
    public Canvas p1;
    public Canvas p2;

    //Player�� Enemy. UiManager.instance.player�� �������� ��.
    public GameObject enemy;
    public GameObject player;

    //ī�޶� ��Ŀ�� ����
    GameObject focus;
    GameObject defeat;
    Vector3 point;

    //���� ����Ʈ
    public RawImage readyGo;
    public RawImage gameSet;

    //������ ó�� ����
    public GameObject lifePlayer1;
    public GameObject lifePlayer2;
    public GameObject lifeEnemy1;
    public GameObject lifeEnemy2;
    int playerLife = 2;
    int enemyLife = 2;

    //����Ʈ ���
    public GameObject effect;
    GameObject effectSpawn;

    //���� ���� ����
    public bool gameEnd = false;
    bool playerWin = false; 

    //ReStartUI�� ����մϴ�.
    public GameObject restartUI;

    //Text �ִϸ��̼�
    Animator animatorPlayerText;
    Animator animatorEnemyText;

    //�ǴϽ� ���� �ִϸ��̼�
    public Canvas finishUI;
    
    //��������
    public GameObject stage;

    //������ �ִϸ��̼�
    public Animator chGaugeColor;

    void Awake()
    {
        //�ν��Ͻ� ���� �� �ı�
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //�ִϸ����� �ҷ�����
        animatorPlayerText = playerHPUI.GetComponent<Animator>();
        animatorEnemyText = enemyHPUI.GetComponent<Animator>();

        //���ʹ̿� �÷��̾� �ҷ�����
        enemy = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        //���� �� ����Ʈ
        readyGo.gameObject.SetActive(true);

        //���� ī�޶� �ҷ�����
        cameraController = Camera.main.GetComponent<CameraController>();

        //�÷��̾� �̸� UI ����
        playerName.text = chName;
        enemyName.text = enName;

        //HP �ʱ�ȭ
        playerHPUI.text = "0";
        enemyHPUI.text = "0";

        //������ �ʱ�ȭ
        playerGauge.value = 0;
        enemyGauge.value = 0;
    }


    // Update is called once per frame
    void Update()
    {
        //P1 P2 ������ ����ٴϱ�
        p1.transform.position = player.transform.position;
        p2.transform.position = enemy.transform.position;
    }

    //������ ���� �Լ�
    public void LifeSub(GameObject collision)
    {
        //�÷��̾� ���� ó��
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
                //���ʹ� �¸�
                gameEnd = true;

                StartCoroutine(effectStart(player.transform, enemy.transform));
            }
        }

        //���ʹ� ���� ó��
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
                //������ �¸�
                gameEnd = true;

                playerWin = true;

                StartCoroutine(effectStart(enemy.transform, player.transform));
            }
        }
    }

    //�߶� ����Ʈ �߻�
    private IEnumerator effectStart(Transform defeat, Transform focus)
    {
        //�߶� ����Ʈ ����
        effectSpawn = Instantiate(effect);
        effectSpawn.transform.position = defeat.position;

        //����Ʈ ���� �� ���� �ص� ���ϴ´�� �ȳ��ͼ� ������ ����
        effectSpawn.transform.LookAt(focus.transform);
        effectSpawn.transform.rotation *= Quaternion.Euler(new Vector3(100, 0, 0));

        //�÷��̾� �¸� ó��
        if (gameEnd == true && playerWin == true)
        {
            gameSet.gameObject.SetActive(true);

            yield return new WaitForSeconds(3.0f);

            SceneManager.LoadScene("Victory");
        }
        //���ʹ� �¸� ó��
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

    //������ �ñر� ��� �� ī�޶� ��Ŀ��
    public void FocusCamera(Transform focus)
    {
        point = focus.transform.position;

        cameraController.cameraTwoFocus = false;
        Camera.main.transform.position = new Vector3(point.x + 4, point.y + 2, -4);
        Camera.main.transform.Rotate(7, -36, 0);
    }

    //������ ���ΰ�ħ �� �߰� ����
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

    //HP ���ΰ�ħ �� �߰� ����
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

    //HP �÷� ��ȭ
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

    //HP �ִϸ��̼� 
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

    //�ָ� �������� �� �߻��ϴ� ����Ʈ �Լ�
    public void FinishEffect(Transform ts)
    {
        finishUI.gameObject.SetActive(true);
        stage.gameObject.SetActive(false);

        Time.timeScale = 0.5f;

        finishUI.transform.position = new Vector3(ts.position.x, ts.position.y, 7);

        StartCoroutine(FinishStart());
    }

    //���ο� ��� �ڷ�ƾ
    IEnumerator FinishStart()
    {
        yield return new WaitForSeconds(0.4f);

        Time.timeScale = 1.0f;

        finishUI.gameObject.SetActive(false);

        stage.gameObject.SetActive(true);
    }
}
