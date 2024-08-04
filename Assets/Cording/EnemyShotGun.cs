using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyShotGun : MonoBehaviour
{
    // ����ĳ��Ʈ �Ÿ�
    
    public ParticleSystem shotGunEffectFactory;
    public AudioClip shotGunSound;
    public float raycastDistance = 3;
    public float shotGunAttackTime = 3;

    public bool isEnemyDamageShotGun = false;
    public bool isEnemyFireRay = false;
    public bool isEnemyShoutGunDamageTime = true;

    float currTime = 0;
    float currShotGunTime = 0;
    
    GameObject player;
    GameObject enemy;
    GameObject enemyMesh;
    GameObject enemyFirePos;
    GameObject enemyMineManager;

    Vector3 playerPOS;
    Vector3 enemyRayPos;

    EnemyMove enemyMove;
    EnemyMineManager enemyMineManagerComp;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Mario");
        enemy = GameObject.Find("Enemy");
        enemyFirePos  = GameObject.Find("EnemyFirePos");
        enemyMesh = GameObject.Find("EnemyGun");
        enemyMineManager = GameObject.Find("EnemyMineManager");

        enemyMove = GetComponent<EnemyMove>();
        enemyMineManagerComp = enemyMineManager.GetComponent<EnemyMineManager>();
        isEnemyDamageShotGun = false;
        
        enemyRayPos = new Vector3(0, 0, 0);



    }

    // Update is called once per frame
    void Update()
    {
        playerPOS = player.transform.position - enemy.transform.position;

        currTime += Time.deltaTime;
        //print(currTime);

        if (shotGunAttackTime < currTime)
        {
            //print(shotGunAttackTime);
            isEnemyFireRay = true;
            currTime = 0;
        }


        if(isEnemyFireRay)
        {
            //print(11111);
            EnemyFireRay();
            isEnemyFireRay = false;
        }


        if (isEnemyDamageShotGun)
        {
            currShotGunTime += Time.deltaTime;
            
            //1.2���Ŀ� ������
            if (1.2 < currShotGunTime)
            {
                //print("currshotguntime");
                
                EnemyDamageShotGun();
                isEnemyDamageShotGun = false;
                isEnemyShoutGunDamageTime = true;


            }

        }

    }

    void EnemyFireRay()
    {
        //�̵�����
        enemyMove.isEnemyMove = false;
        //�߻�����
        enemyMove.isBulletFire = false;
        //���ڼ�ȯ
        enemyMineManagerComp.isMineActive = false;

        if (isEnemyFireRay)
        {
            // ĳ������ ���� �������� ����ĳ��Ʈ�� �߻�
            Ray enemyRay = new Ray(transform.position, playerPOS);
            RaycastHit hit;
           
            // ����ĳ��Ʈ�� �浹�� ���
            if (Physics.Raycast(enemyRay, out hit, raycastDistance))
            {
                ViewRay();

                if (isEnemyShoutGunDamageTime)
                {
                    //�������� ����.
                    isEnemyDamageShotGun = true;
                    isEnemyShoutGunDamageTime = false;
                }



                //���ǿ��ϸ��̼�
                EnemyShotGunAnim();
                //��������Ʈ
                EnemyShotGunEffect();
                //���ǼҸ�����
                AudioSource.PlayClipAtPoint(shotGunSound, Camera.main.transform.position);


                /* Debug.Log("����ĳ��Ʈ�� �浹�� ������Ʈ: " + hit.collider.name);
                 Debug.Log("�浹 ��ġ: " + hit.point);
                 Debug.Log("�浹 ǥ���� ���� ����: " + hit.normal);*/
            }
            else
            {
                ViewRay();
                //print(1111);
                EnemyShotGunAnim();
                //��������Ʈ
                EnemyShotGunEffect();
                //���ǼҸ�����
                AudioSource.PlayClipAtPoint(shotGunSound, Camera.main.transform.position);

            }

          

        }
    }

    void EnemyDamageShotGun()
    {
        //print("ShotGunDamage");
        player.GetComponent<MarioPlayerMove>().Damaged(10, 2.0f);
    }

    void EnemyShotGunEffect()
    {
        ParticleSystem shotGunEffect = Instantiate(shotGunEffectFactory);
        shotGunEffect.transform.position = enemyFirePos.transform.position; //+ new Vector3(0,3,0);
        shotGunEffect.Play();
        Destroy(shotGunEffect.gameObject, 2);
    }

    void EnemyShotGunAnim()
    {
        
        Animator animator = enemyMesh.GetComponent<Animator>();
        animator.SetTrigger("ShotGun");
       
    }

    void ViewRay()
    {  
        // ���� �ð�ȭ (����׿�)
        Debug.DrawRay(transform.position + enemyRayPos, playerPOS, Color.red);

    }

}
