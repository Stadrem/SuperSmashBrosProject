using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyShotGun : MonoBehaviour
{
    // 레이캐스트 거리
    
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
            
            //1.2초후에 데미지
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
        //이동정지
        enemyMove.isEnemyMove = false;
        //발사정지
        enemyMove.isBulletFire = false;
        //지뢰소환
        enemyMineManagerComp.isMineActive = false;

        if (isEnemyFireRay)
        {
            // 캐릭터의 전방 방향으로 레이캐스트를 발사
            Ray enemyRay = new Ray(transform.position, playerPOS);
            RaycastHit hit;
           
            // 레이캐스트가 충돌한 경우
            if (Physics.Raycast(enemyRay, out hit, raycastDistance))
            {
                ViewRay();

                if (isEnemyShoutGunDamageTime)
                {
                    //데미지를 주자.
                    isEnemyDamageShotGun = true;
                    isEnemyShoutGunDamageTime = false;
                }



                //샷건에니메이션
                EnemyShotGunAnim();
                //샷건이펙트
                EnemyShotGunEffect();
                //샷건소리내기
                AudioSource.PlayClipAtPoint(shotGunSound, Camera.main.transform.position);


                /* Debug.Log("레이캐스트가 충돌한 오브젝트: " + hit.collider.name);
                 Debug.Log("충돌 위치: " + hit.point);
                 Debug.Log("충돌 표면의 법선 벡터: " + hit.normal);*/
            }
            else
            {
                ViewRay();
                //print(1111);
                EnemyShotGunAnim();
                //샷건이펙트
                EnemyShotGunEffect();
                //샷건소리내기
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
        // 레이 시각화 (디버그용)
        Debug.DrawRay(transform.position + enemyRayPos, playerPOS, Color.red);

    }

}
