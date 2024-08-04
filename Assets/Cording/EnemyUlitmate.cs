using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyUlitmate : MonoBehaviour
{
    public GameObject jetFacroty;
    public GameObject enemyMineFactory;
    public GameObject jetCameraPos;
   

    public AudioClip ultimateSoundFactory;
    public Camera mainCamera;
    public Camera enemyCamera;

    public GameObject ultimateEffectPos;
    public ParticleSystem ultimateEffect;

    GameObject player;
    GameObject enemy;
    GameObject enemyAnim;

    MarioPlayerMove playerMove;
    EnemyMove enemyMove;
    EnemyMineManager enemyMineManager;
    Animator enemyAnimator;

    Camera enemyCameraComp;
    Vector3 enemyCameraPos;
    Quaternion enemyCarmeraRot;

    Vector3 enemyUltimateCamera;

    float currTime = 0;
    bool isUltimateActive = false;
    bool isUltimateActiveKey = false;
    bool isCameraPosChnage = false;
    bool isUltimateTimer = false;
    bool isEnemyCameraMove = false;
    bool isEnemyCameraPosOrigin = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Mario");
        playerMove = player.GetComponent<MarioPlayerMove>();

        enemy = GameObject.Find("Enemy");
        enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMineManager = enemyMineFactory.GetComponent<EnemyMineManager>();

        enemyAnim = GameObject.Find("EnemyGun");
        enemyAnimator = enemyAnim.GetComponent<Animator>();

        jetCameraPos.transform.position = new Vector3(25, 2, -5);

        //궁극기 끔
        enemyMove.isUltimate = false;


       

    }

    // Update is called once per frame
    void Update()
    {


        UltimateCameraPosTest();

        //에너지가 가득차면 필살기를 사용
        if (UiManager.instance.enGauge == 1)
        {
            //print("적의 에너지" + UiManager.instance.enGauge);
            //궁극기 로직시작
            isUltimateActive = true;
            //카메라 위치를 바꿔주는 함수호출
            isCameraPosChnage = true;
            //isUltimateActiveKey = false;
            //print("궁극기 발동키" + isUltimateActive);
            
            //에너지를 0으로
            UiManager.instance.enGauge = 0;
            UiManager.instance.GaugeRefresh(0, 0);
        }


        //숫자0키로 궁극기 작동
        isUltimateActiveKey = Input.GetKeyDown(KeyCode.Keypad0);
        if (isUltimateActiveKey)
        {
            isUltimateActive = true;
           
        }


        //print(enemyCameraPos);
        //enemyUltimateCamera = enemyCameraPos + new Vector3(-3, 0, 0);

        
        //궁극기가 사용되면
        if (isUltimateActive)
        {
            //블락을 켜주자.
            playerMove.isEnBlocking = true;
            //적 이동정지
            enemyMove.isEnemyMove = false;
            //적 공격정지
            enemyMove.isBulletFire = false;
            //적 폭탄정지
            enemyMineManager.isMineActive = false;


            //타이머를 작동시킵니다.
            isUltimateTimer = true;
            enemyMove.isUltimate = false;

            //카메라를 전환 해줍시다.
            CameraPositionChange();
            //사용시 이펙트 출력
            UltimateUseEffect();
            GameObject enemyJet = Instantiate(jetFacroty);
            enemyJet.transform.position = new Vector3(140, 0, 0);
            
            enemyAnimator.SetTrigger("Ultimate");
            AudioSource.PlayClipAtPoint(ultimateSoundFactory, Camera.main.transform.position);
            
            //궁극기 로직을 끕니다.
            isUltimateActive = false;
          
        }

       
        //궁극시 사용시 시간누적
        UltimateTimer();


        //궁극기 사용후 카메라를 움직이는 로직
        EnemyCameraMove();


        //3초가 지나면 카메마를 원위치로 하는 로직
        EnemyCameraPosOrigin();


    }//업데이트 끝


    //isUltimateTimer 참이면 시간누적
    void UltimateTimer()
    {
        if (isUltimateTimer)
        {
            currTime += Time.deltaTime;
        }

        //1초가 지나면 카메라이동을 참으로
        if (currTime > 1)
        {
            isEnemyCameraMove = true;
            //print(currTime);
        }
        if(currTime > 3)
        {
            //서브 카메리 이동정지
            isEnemyCameraMove = false;
            //메인 카메라 원위치
            isEnemyCameraPosOrigin = true;
            //타이머 끄자.
            isUltimateTimer = false;
            //시간을 0으로
            currTime = 0;
            //적 움직이게 하자.
            enemyMove.isUltimate = false;
            
        }


    }

    void CameraPositionChange()
    {
        if (isCameraPosChnage)
        {

            //메인카메라의 위치에 적의 카메라 위치로 변경
            mainCamera.transform.position = enemyCamera.transform.position;
            //메인카메라를 카메라로
            mainCamera = Camera.main;
            //메인카메라 끄기
            mainCamera.gameObject.SetActive(false);
            //적 카메라의 태그를 메인카메라로
            enemyCamera.tag = "MainCamera";
            //적 카메마를 켜주자.
            enemyCamera.gameObject.SetActive(true);
            //궁극기 애니메이션 켜주자.
            enemyAnimator.SetTrigger("Ultimate");
            //함수호출 닫기
            isCameraPosChnage = false;
                
        
        }

        
    }

    //isEnemyCameraMove 참이면 작동
    void EnemyCameraMove()
    {
 
        if (isEnemyCameraMove)
        {
            enemyMove.isEnemyMove = false;
            enemyMineManager.isMineManagerUpdate = false;
            Vector3 moveJetPos = jetCameraPos.transform.position - enemyCamera.transform.position;
            moveJetPos.Normalize();
            enemyCamera.transform.position += moveJetPos * 1.5f * Time.deltaTime;

            //로컬좌표의 포워드방향을 월드좌표 기준으로 만들자.
            enemyCamera.transform.rotation = Quaternion.LookRotation(Vector3.right);
        }

    }

    void EnemyCameraPosOrigin()
    {
        if(isEnemyCameraPosOrigin)
        {
            //print(33333);
            //서브카메라 위치를 다시조정
            
            enemyCamera.transform.localPosition = new Vector3(1.5f, 0.5f, 1);
            //로테이션 값은 0.7입니다.


            enemyCamera.transform.localRotation = Quaternion.Euler(0, 180, 0);
            Quaternion newQuaternion = Quaternion.Euler(0, 50, 0);
            enemyCamera.transform.localRotation *= newQuaternion;
            print(enemyCamera.transform.localRotation);
            //print(enemyCamera.transform.rotation);       
           
            //서브카메라 비활성화
            enemyCamera.gameObject.SetActive(false);
            //메인카메라 활성화
            mainCamera.gameObject.SetActive(true);

            

            isEnemyCameraPosOrigin = false;
            //블락을 켜주자.
            playerMove.isEnBlocking = false;
        }

        
      
    }

    void UltimateCameraPosTest()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            //print(1111);
            //메인카메라의 위치를 변경
            mainCamera.transform.position = enemyCamera.transform.position;
            //print("메인카메라 위치" + mainCamera.transform.position);
            //print("적 카메라 위치" + enemyCamera.transform.position);
            mainCamera = Camera.main;
            //메인카메라를비활성화
            mainCamera.gameObject.SetActive(false);
            enemyCamera.tag = "MainCamera";
            enemyCamera.gameObject.SetActive(true);
            enemyAnimator.SetTrigger("Ultimate");

            if(1 < currTime)
            {
                isEnemyCameraMove = true;
            }
            //isEnemyCameraMove = true;

        }

        if (isEnemyCameraMove)
        {
            enemyMove.isEnemyMove = false;
            enemyMineManager.isMineManagerUpdate = false;
            Vector3 moveJetPos = jetCameraPos.transform.position - enemyCamera.transform.position;
            moveJetPos.Normalize();
            enemyCamera.transform.position += moveJetPos * 1.5f * Time.deltaTime;

            //로컬좌표의 포워드방향을 월드좌표 기준으로 만들자.
            enemyCamera.transform.rotation = Quaternion.LookRotation(Vector3.right);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            //
            enemyCamera.gameObject.SetActive(false);
            //메인카메라를활성화
            mainCamera.gameObject.SetActive(true);

        }


    }

    void CameraMoveJetSpawn()
    {
        if (isEnemyCameraMove)
        {
            enemyMove.isEnemyMove = false;
            enemyMineManager.isMineManagerUpdate = false;
            Vector3 moveJetPos = jetCameraPos.transform.position - enemyCamera.transform.position;
            moveJetPos.Normalize();
            enemyCamera.transform.position += moveJetPos * 1.5f * Time.deltaTime;

            //로컬좌표의 포워드방향을 월드좌표 기준으로 만들자.
            enemyCamera.transform.rotation = Quaternion.LookRotation(Vector3.right);
        }
    }

    void UltimateUseEffect()
    {

        ParticleSystem ultimateEff = Instantiate(ultimateEffect);
        ultimateEff.transform.position = ultimateEffectPos.transform.position;
        ultimateEff.transform.rotation = Quaternion.Euler(0,90,0);
        Destroy(ultimateEff.gameObject, 2);

    }




}