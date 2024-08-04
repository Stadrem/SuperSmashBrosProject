using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMove : MonoBehaviour
{

    
    //총알공장
    public GameObject bulletFactory;
    public GameObject enemyFirePos;
    public ParticleSystem bulletEffectFactory;
    //public int bulletMoveSpeed = 0;
    public float enemyRotY;
    public float backMoveDist = 1;
    public float enemyMoveSpeed = 1;
    public float enemySpeedBoost = 1;
    public float bulletFireSpeed = 0.5f;

    public bool isUltimate = false;
    //적이동
    public bool isEnemyMove = true;
    //적공격
    public bool isBulletFire = false;
    //총을 쏠수 있는지
    public bool isEnemyFire;
    //타이머 누적
    public bool isEnemyTimer = false;
    //경직상태
    public bool isEnemyHurt = false;
    //업데이트 제한
    public bool isEnemyMoveUpdate = false;

    
    //총알을 못쏘게 하자.
    public bool isEnemyBulletInstantiate = false;

    //마리오플레이어무브 스크립트
    MarioPlayerMove marioPlayerMove;
    EnemyDodge enemydodge;
    EnemyShotGun enemyShoutGun;

    GameObject targetPlayer;
    GameObject centerObject;
    GameObject leftObject;
    GameObject rightObject;
    GameObject enemyAnim;

    Vector3 enemyPos;
    Vector3 playerPos;
    Vector3 playerDir;
    
  
    float currTime = 0;

    //적의 체력
    int enemyHP = 0;
    //적의 누적 체력
    int currEnemyHP = 0;
    //적의 이동속도
     
    //플레이어와 거리
    float playerDist;
    //플레이어와의 실력 
    bool enemyBackMove = false;
    //중앙 오브젝트와의 거리
    float centerDist;
    float rightDist;
    float leftDist;
    //총쏘는애니메이션
    bool isFireAnim = true;


    // Start is called before the first frame update
    void Start()
    {
       
        //하이어라키에서 오브젝트 찾기(이름)
        targetPlayer = GameObject.Find("Mario");
        //애너미 총구
        enemyFirePos = GameObject.Find("EnemyFirePos");
        //마리오의 컴포넌트를 가져오자.
        marioPlayerMove = targetPlayer.GetComponent<MarioPlayerMove>();
        //중심점
        centerObject = GameObject.Find("CenterObject");
        //왼쪽 지점
        leftObject = GameObject.Find("LeftObject");
        //오른쪽 지점
        rightObject = GameObject.Find("RightObject");
        //
        enemyAnim = GameObject.Find("EnemyGun");
        enemydodge = GetComponent<EnemyDodge>();
        //샷건
        enemyShoutGun = GetComponent<EnemyShotGun>();

        //총알생성하게 해주자.
        //isEnemyBulletInstantiate = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (UiManager.instance.gameEnd) return;


        if(isEnemyMoveUpdate)
        {

        

           //두점의 거리
           centerDist = Vector3.Distance(centerObject.transform.position,transform.position);

            //시간누적
            currTime += Time.deltaTime;

            //enemyTimer가 true일때 currTime 시작
            EnemyTimer();

            //피해를 받으면 1초동안 플레어의 이동이 정지
            MoveStop();

            //플레이어 방향을 구하는 함수 P<-E playerDir
            PlayerDirection();

            //플레이어의 방향으로 회전하는 함수 , 플레이어 x값을 가져와서 +일때와 -일때 회전
            EnemyRotation();
      
            BackMove();

            AttackTimer();

            EnemyBulletFire();


        }


    }//업데이트 끝


    
    //기본공격 : 총알 발사
    void EnemyBulletFire()
    {
        if (enemydodge.isDodgeActive) return;
        if (isBulletFire)
        {

            //이동정지
            isEnemyMove = false;
            //샷건정지
            enemyShoutGun.isEnemyFireRay = false;
            if (isFireAnim)
            {
                //애니메이션 출력
                Animator animator;
                animator = enemyAnim.GetComponent<Animator>();
                animator.SetTrigger("Fire");
                isFireAnim = false;
                //타이머를 켜자
                isEnemyTimer = true;
            }
            
            
            if( bulletFireSpeed < currTime )
            {
                //print(1111);
                //총알생성
                //isEnemyFireStop = true;
                if (isEnemyBulletInstantiate)
                {
                    GameObject enemyBullet = Instantiate(bulletFactory);
                    // Vector3 enemyForwardPos = new Vector3(-2,0,0);
                    //총구위치
                    //enemyBullet.transform.position = transform.position + enemyForwardPos;
                    enemyBullet.transform.position = enemyFirePos.transform.position;
                    ParticleSystem particle = Instantiate(bulletEffectFactory);
                    particle.transform.position = enemyFirePos.transform.position + new Vector3(0, 0, 0);
                    particle.Play();
                    Destroy(particle.gameObject, 2);
                }
                

                //타이머를 끄자.
                isEnemyTimer = false;
                //발사정지
                isBulletFire = false;
                //애님을 켜주자
                isFireAnim = true;
                //다시이동
                isEnemyMove = true;
            }


            
            

        }
        

       

    }


    //플레이어 방향을 구하는 함수
    void PlayerDirection()
    {
        //enemyFirePos = GameObject.Find("EnemyFirePos");
        //적 위치를 담자.
        enemyPos = transform.position;
        //플레이어 위치를 담자.
        playerPos = targetPlayer.transform.position;
        //플레이어 방향을 구하자. 플레이어 쪽으로 이동.
        playerDir = playerPos - enemyPos;
        //정규화
        playerDir.Normalize();
        //플레이어와의 거리 출력
        //print("플레어와의 거리 :" + playerDir);
    }
    //플레이어의 방향으로 회전하는 함수, 
    void EnemyRotation()
    {
        //PlayerDirection();
        //플레이어 방향의 x값을 가져오는 함수
        float playerDistX = playerDir.x;
        //왼쪽 - 오른쪽 +
        //print("플레어의 X값 :" + playerDistX);

        if (0 < playerDistX)
        {
            //적의 로테이션변경 
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if ( 0 > playerDistX)
        {   //적의 로테이션변경
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {

        }
    }

    void BackMove()
    {

        #region 후퇴하는 로직

        //플레어와 나의 거리 구하자.
        playerDist = Vector3.Distance(playerPos, enemyPos);
        playerDir.Normalize();

        //얼티메이트가 참이면
        if (isUltimate) return;

        //움직일수 있을때
        if (isEnemyMove)
        {
            //print(centerDist);
            //print("플레이어거리" +playerDist);
            //센터와 거리가 15보다 작을때
            if (centerDist < 15)
            {

                ///이동가능하고 플레이어와 거리가 5보다 작다면
                //if (isEnemyMove)// && playerDist < 5)
                // if (isEnemyMove && playerDist < 3)
                //거리가 3보다 작아지면 후퇴
                
                if (playerDist < backMoveDist)
                {
                    //print("BackWalk" + playerDist);
                    //플레이어 반대 방향으로 이동
                    transform.position += -playerDir * enemyMoveSpeed * Time.deltaTime;
                    Animator animator;
                    animator = enemyAnim.GetComponent<Animator>();
                    animator.SetTrigger("BackWalk");

                    // rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);

                    //플레이가 이동한 거리를 저장하고 싶다.
                    //이동한 거리를 누적하고 거리의 값이 10보다 커지면 잠시 정지하자.
                    //이동한 거릴를 누적할 변수
                    

                }

                //5거리보다 멀어지면
                else if (5 < playerDist)
                {   
                    //7보다 멀어지면
                    if (7 < playerDist)
                    {
                        transform.position += playerDir * enemyMoveSpeed * Time.deltaTime;
                        Animator animator;
                        animator = enemyAnim.GetComponent<Animator>();
                        animator.SetTrigger("Run");
                        //animator.SetTrigger("Walk");
                    }
                    else
                    {
                        //print("run" + playerDist);
                        transform.position += playerDir * enemyMoveSpeed * Time.deltaTime;
                        Animator animator;
                        animator = enemyAnim.GetComponent<Animator>();
                        //animator.SetTrigger("run");
                        animator.SetTrigger("Walk");
                    }
                    
                }

                

                
                //거리가 4보다 작으면 정지
                /*else if ( playerDist < 5 )  
                {
                    print("idle");
                    isEnemyMove = false;
                    Animator animator;
                    animator = enemyAnim.GetComponent<Animator>();
                    animator.SetTrigger("Idle");
                }*/
                /*//거리가 5보다 크면 전진
                if( 5 < playerDist )
                {
                    //print("foward");
                    //플레이어가 전진
                    transform.position += playerDir * enemyMoveSpeed * Time.deltaTime;
                    Animator animator;
                    animator = enemyAnim.GetComponent<Animator>();
                    animator.SetTrigger("Walk");
                }*/
            }
            //센터보다 거리가 15보다 클때
            else
            {
               // print("else run");
                //플레이어가 전진
                transform.position += playerDir * (enemyMoveSpeed * enemySpeedBoost) * Time.deltaTime;
                Animator animator;
                animator = enemyAnim.GetComponent<Animator>();
                animator.SetTrigger("Run");
            }

        }//움직일수 있을때 끝
        //벡터 길이를 구하자. 양수로 나옴. 
        //playerDist= playerDir.magnitude   ;

    }

    #endregion
    void AttackTimer()
    {
        //2초보다 크면 정지
        if (3 < currTime)
        {
          
            //print(1111);
            isEnemyMove = false;
            isBulletFire = true;
            currTime = 0;
            
            
        }//정지 끝

    }//함수끝

    void EnemyTimer()
    {
        //타이머를 켜자.
        if (isEnemyTimer)
        {
            currTime += Time.deltaTime;
        }
        
    }

    void MoveStop()
    {
        //컴포넌트의 적체력을 계속 가져오자.
        enemyHP = marioPlayerMove.enHP;
        //Debug.Log(enemyHP);

        //적의 체력이 누적 체려보다 많다면
        if (currEnemyHP < enemyHP)
        {
            //print(1111);
            //누적체력을 갱신
            currEnemyHP = enemyHP;
            
            //정지
            isEnemyMove = false;
            //공격정지
            isBulletFire = false;
            //타이머누적
            isEnemyTimer = true;
            isEnemyHurt = true;
            if (isEnemyHurt)
            {
                Animator animator;
                animator = enemyAnim.GetComponent<Animator>();
                animator.SetTrigger("Hurt");
 
            }
            

            //0.5초 지나면
            if (currTime > 0.5)
            {
                
                isEnemyTimer = false;
                currTime = 0;
                isEnemyMove = true;
            }
        }
    }


}//클래스 마지막
