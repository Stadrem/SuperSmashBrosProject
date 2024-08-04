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

        //�ñر� ��
        enemyMove.isUltimate = false;


       

    }

    // Update is called once per frame
    void Update()
    {


        UltimateCameraPosTest();

        //�������� �������� �ʻ�⸦ ���
        if (UiManager.instance.enGauge == 1)
        {
            //print("���� ������" + UiManager.instance.enGauge);
            //�ñر� ��������
            isUltimateActive = true;
            //ī�޶� ��ġ�� �ٲ��ִ� �Լ�ȣ��
            isCameraPosChnage = true;
            //isUltimateActiveKey = false;
            //print("�ñر� �ߵ�Ű" + isUltimateActive);
            
            //�������� 0����
            UiManager.instance.enGauge = 0;
            UiManager.instance.GaugeRefresh(0, 0);
        }


        //����0Ű�� �ñر� �۵�
        isUltimateActiveKey = Input.GetKeyDown(KeyCode.Keypad0);
        if (isUltimateActiveKey)
        {
            isUltimateActive = true;
           
        }


        //print(enemyCameraPos);
        //enemyUltimateCamera = enemyCameraPos + new Vector3(-3, 0, 0);

        
        //�ñرⰡ ���Ǹ�
        if (isUltimateActive)
        {
            //����� ������.
            playerMove.isEnBlocking = true;
            //�� �̵�����
            enemyMove.isEnemyMove = false;
            //�� ��������
            enemyMove.isBulletFire = false;
            //�� ��ź����
            enemyMineManager.isMineActive = false;


            //Ÿ�̸Ӹ� �۵���ŵ�ϴ�.
            isUltimateTimer = true;
            enemyMove.isUltimate = false;

            //ī�޶� ��ȯ ���ݽô�.
            CameraPositionChange();
            //���� ����Ʈ ���
            UltimateUseEffect();
            GameObject enemyJet = Instantiate(jetFacroty);
            enemyJet.transform.position = new Vector3(140, 0, 0);
            
            enemyAnimator.SetTrigger("Ultimate");
            AudioSource.PlayClipAtPoint(ultimateSoundFactory, Camera.main.transform.position);
            
            //�ñر� ������ ���ϴ�.
            isUltimateActive = false;
          
        }

       
        //�ñؽ� ���� �ð�����
        UltimateTimer();


        //�ñر� ����� ī�޶� �����̴� ����
        EnemyCameraMove();


        //3�ʰ� ������ ī�޸��� ����ġ�� �ϴ� ����
        EnemyCameraPosOrigin();


    }//������Ʈ ��


    //isUltimateTimer ���̸� �ð�����
    void UltimateTimer()
    {
        if (isUltimateTimer)
        {
            currTime += Time.deltaTime;
        }

        //1�ʰ� ������ ī�޶��̵��� ������
        if (currTime > 1)
        {
            isEnemyCameraMove = true;
            //print(currTime);
        }
        if(currTime > 3)
        {
            //���� ī�޸� �̵�����
            isEnemyCameraMove = false;
            //���� ī�޶� ����ġ
            isEnemyCameraPosOrigin = true;
            //Ÿ�̸� ����.
            isUltimateTimer = false;
            //�ð��� 0����
            currTime = 0;
            //�� �����̰� ����.
            enemyMove.isUltimate = false;
            
        }


    }

    void CameraPositionChange()
    {
        if (isCameraPosChnage)
        {

            //����ī�޶��� ��ġ�� ���� ī�޶� ��ġ�� ����
            mainCamera.transform.position = enemyCamera.transform.position;
            //����ī�޶� ī�޶��
            mainCamera = Camera.main;
            //����ī�޶� ����
            mainCamera.gameObject.SetActive(false);
            //�� ī�޶��� �±׸� ����ī�޶��
            enemyCamera.tag = "MainCamera";
            //�� ī�޸��� ������.
            enemyCamera.gameObject.SetActive(true);
            //�ñر� �ִϸ��̼� ������.
            enemyAnimator.SetTrigger("Ultimate");
            //�Լ�ȣ�� �ݱ�
            isCameraPosChnage = false;
                
        
        }

        
    }

    //isEnemyCameraMove ���̸� �۵�
    void EnemyCameraMove()
    {
 
        if (isEnemyCameraMove)
        {
            enemyMove.isEnemyMove = false;
            enemyMineManager.isMineManagerUpdate = false;
            Vector3 moveJetPos = jetCameraPos.transform.position - enemyCamera.transform.position;
            moveJetPos.Normalize();
            enemyCamera.transform.position += moveJetPos * 1.5f * Time.deltaTime;

            //������ǥ�� ����������� ������ǥ �������� ������.
            enemyCamera.transform.rotation = Quaternion.LookRotation(Vector3.right);
        }

    }

    void EnemyCameraPosOrigin()
    {
        if(isEnemyCameraPosOrigin)
        {
            //print(33333);
            //����ī�޶� ��ġ�� �ٽ�����
            
            enemyCamera.transform.localPosition = new Vector3(1.5f, 0.5f, 1);
            //�����̼� ���� 0.7�Դϴ�.


            enemyCamera.transform.localRotation = Quaternion.Euler(0, 180, 0);
            Quaternion newQuaternion = Quaternion.Euler(0, 50, 0);
            enemyCamera.transform.localRotation *= newQuaternion;
            print(enemyCamera.transform.localRotation);
            //print(enemyCamera.transform.rotation);       
           
            //����ī�޶� ��Ȱ��ȭ
            enemyCamera.gameObject.SetActive(false);
            //����ī�޶� Ȱ��ȭ
            mainCamera.gameObject.SetActive(true);

            

            isEnemyCameraPosOrigin = false;
            //����� ������.
            playerMove.isEnBlocking = false;
        }

        
      
    }

    void UltimateCameraPosTest()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            //print(1111);
            //����ī�޶��� ��ġ�� ����
            mainCamera.transform.position = enemyCamera.transform.position;
            //print("����ī�޶� ��ġ" + mainCamera.transform.position);
            //print("�� ī�޶� ��ġ" + enemyCamera.transform.position);
            mainCamera = Camera.main;
            //����ī�޶󸦺�Ȱ��ȭ
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

            //������ǥ�� ����������� ������ǥ �������� ������.
            enemyCamera.transform.rotation = Quaternion.LookRotation(Vector3.right);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            //
            enemyCamera.gameObject.SetActive(false);
            //����ī�޶�Ȱ��ȭ
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

            //������ǥ�� ����������� ������ǥ �������� ������.
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