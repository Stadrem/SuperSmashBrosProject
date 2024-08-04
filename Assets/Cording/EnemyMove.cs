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

    
    //�Ѿ˰���
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
    //���̵�
    public bool isEnemyMove = true;
    //������
    public bool isBulletFire = false;
    //���� ��� �ִ���
    public bool isEnemyFire;
    //Ÿ�̸� ����
    public bool isEnemyTimer = false;
    //��������
    public bool isEnemyHurt = false;
    //������Ʈ ����
    public bool isEnemyMoveUpdate = false;

    
    //�Ѿ��� ����� ����.
    public bool isEnemyBulletInstantiate = false;

    //�������÷��̾�� ��ũ��Ʈ
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

    //���� ü��
    int enemyHP = 0;
    //���� ���� ü��
    int currEnemyHP = 0;
    //���� �̵��ӵ�
     
    //�÷��̾�� �Ÿ�
    float playerDist;
    //�÷��̾���� �Ƿ� 
    bool enemyBackMove = false;
    //�߾� ������Ʈ���� �Ÿ�
    float centerDist;
    float rightDist;
    float leftDist;
    //�ѽ�¾ִϸ��̼�
    bool isFireAnim = true;


    // Start is called before the first frame update
    void Start()
    {
       
        //���̾��Ű���� ������Ʈ ã��(�̸�)
        targetPlayer = GameObject.Find("Mario");
        //�ֳʹ� �ѱ�
        enemyFirePos = GameObject.Find("EnemyFirePos");
        //�������� ������Ʈ�� ��������.
        marioPlayerMove = targetPlayer.GetComponent<MarioPlayerMove>();
        //�߽���
        centerObject = GameObject.Find("CenterObject");
        //���� ����
        leftObject = GameObject.Find("LeftObject");
        //������ ����
        rightObject = GameObject.Find("RightObject");
        //
        enemyAnim = GameObject.Find("EnemyGun");
        enemydodge = GetComponent<EnemyDodge>();
        //����
        enemyShoutGun = GetComponent<EnemyShotGun>();

        //�Ѿ˻����ϰ� ������.
        //isEnemyBulletInstantiate = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (UiManager.instance.gameEnd) return;


        if(isEnemyMoveUpdate)
        {

        

           //������ �Ÿ�
           centerDist = Vector3.Distance(centerObject.transform.position,transform.position);

            //�ð�����
            currTime += Time.deltaTime;

            //enemyTimer�� true�϶� currTime ����
            EnemyTimer();

            //���ظ� ������ 1�ʵ��� �÷����� �̵��� ����
            MoveStop();

            //�÷��̾� ������ ���ϴ� �Լ� P<-E playerDir
            PlayerDirection();

            //�÷��̾��� �������� ȸ���ϴ� �Լ� , �÷��̾� x���� �����ͼ� +�϶��� -�϶� ȸ��
            EnemyRotation();
      
            BackMove();

            AttackTimer();

            EnemyBulletFire();


        }


    }//������Ʈ ��


    
    //�⺻���� : �Ѿ� �߻�
    void EnemyBulletFire()
    {
        if (enemydodge.isDodgeActive) return;
        if (isBulletFire)
        {

            //�̵�����
            isEnemyMove = false;
            //��������
            enemyShoutGun.isEnemyFireRay = false;
            if (isFireAnim)
            {
                //�ִϸ��̼� ���
                Animator animator;
                animator = enemyAnim.GetComponent<Animator>();
                animator.SetTrigger("Fire");
                isFireAnim = false;
                //Ÿ�̸Ӹ� ����
                isEnemyTimer = true;
            }
            
            
            if( bulletFireSpeed < currTime )
            {
                //print(1111);
                //�Ѿ˻���
                //isEnemyFireStop = true;
                if (isEnemyBulletInstantiate)
                {
                    GameObject enemyBullet = Instantiate(bulletFactory);
                    // Vector3 enemyForwardPos = new Vector3(-2,0,0);
                    //�ѱ���ġ
                    //enemyBullet.transform.position = transform.position + enemyForwardPos;
                    enemyBullet.transform.position = enemyFirePos.transform.position;
                    ParticleSystem particle = Instantiate(bulletEffectFactory);
                    particle.transform.position = enemyFirePos.transform.position + new Vector3(0, 0, 0);
                    particle.Play();
                    Destroy(particle.gameObject, 2);
                }
                

                //Ÿ�̸Ӹ� ����.
                isEnemyTimer = false;
                //�߻�����
                isBulletFire = false;
                //�ִ��� ������
                isFireAnim = true;
                //�ٽ��̵�
                isEnemyMove = true;
            }


            
            

        }
        

       

    }


    //�÷��̾� ������ ���ϴ� �Լ�
    void PlayerDirection()
    {
        //enemyFirePos = GameObject.Find("EnemyFirePos");
        //�� ��ġ�� ����.
        enemyPos = transform.position;
        //�÷��̾� ��ġ�� ����.
        playerPos = targetPlayer.transform.position;
        //�÷��̾� ������ ������. �÷��̾� ������ �̵�.
        playerDir = playerPos - enemyPos;
        //����ȭ
        playerDir.Normalize();
        //�÷��̾���� �Ÿ� ���
        //print("�÷������ �Ÿ� :" + playerDir);
    }
    //�÷��̾��� �������� ȸ���ϴ� �Լ�, 
    void EnemyRotation()
    {
        //PlayerDirection();
        //�÷��̾� ������ x���� �������� �Լ�
        float playerDistX = playerDir.x;
        //���� - ������ +
        //print("�÷����� X�� :" + playerDistX);

        if (0 < playerDistX)
        {
            //���� �����̼Ǻ��� 
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if ( 0 > playerDistX)
        {   //���� �����̼Ǻ���
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {

        }
    }

    void BackMove()
    {

        #region �����ϴ� ����

        //�÷���� ���� �Ÿ� ������.
        playerDist = Vector3.Distance(playerPos, enemyPos);
        playerDir.Normalize();

        //��Ƽ����Ʈ�� ���̸�
        if (isUltimate) return;

        //�����ϼ� ������
        if (isEnemyMove)
        {
            //print(centerDist);
            //print("�÷��̾�Ÿ�" +playerDist);
            //���Ϳ� �Ÿ��� 15���� ������
            if (centerDist < 15)
            {

                ///�̵������ϰ� �÷��̾�� �Ÿ��� 5���� �۴ٸ�
                //if (isEnemyMove)// && playerDist < 5)
                // if (isEnemyMove && playerDist < 3)
                //�Ÿ��� 3���� �۾����� ����
                
                if (playerDist < backMoveDist)
                {
                    //print("BackWalk" + playerDist);
                    //�÷��̾� �ݴ� �������� �̵�
                    transform.position += -playerDir * enemyMoveSpeed * Time.deltaTime;
                    Animator animator;
                    animator = enemyAnim.GetComponent<Animator>();
                    animator.SetTrigger("BackWalk");

                    // rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);

                    //�÷��̰� �̵��� �Ÿ��� �����ϰ� �ʹ�.
                    //�̵��� �Ÿ��� �����ϰ� �Ÿ��� ���� 10���� Ŀ���� ��� ��������.
                    //�̵��� �Ÿ��� ������ ����
                    

                }

                //5�Ÿ����� �־�����
                else if (5 < playerDist)
                {   
                    //7���� �־�����
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

                

                
                //�Ÿ��� 4���� ������ ����
                /*else if ( playerDist < 5 )  
                {
                    print("idle");
                    isEnemyMove = false;
                    Animator animator;
                    animator = enemyAnim.GetComponent<Animator>();
                    animator.SetTrigger("Idle");
                }*/
                /*//�Ÿ��� 5���� ũ�� ����
                if( 5 < playerDist )
                {
                    //print("foward");
                    //�÷��̾ ����
                    transform.position += playerDir * enemyMoveSpeed * Time.deltaTime;
                    Animator animator;
                    animator = enemyAnim.GetComponent<Animator>();
                    animator.SetTrigger("Walk");
                }*/
            }
            //���ͺ��� �Ÿ��� 15���� Ŭ��
            else
            {
               // print("else run");
                //�÷��̾ ����
                transform.position += playerDir * (enemyMoveSpeed * enemySpeedBoost) * Time.deltaTime;
                Animator animator;
                animator = enemyAnim.GetComponent<Animator>();
                animator.SetTrigger("Run");
            }

        }//�����ϼ� ������ ��
        //���� ���̸� ������. ����� ����. 
        //playerDist= playerDir.magnitude   ;

    }

    #endregion
    void AttackTimer()
    {
        //2�ʺ��� ũ�� ����
        if (3 < currTime)
        {
          
            //print(1111);
            isEnemyMove = false;
            isBulletFire = true;
            currTime = 0;
            
            
        }//���� ��

    }//�Լ���

    void EnemyTimer()
    {
        //Ÿ�̸Ӹ� ����.
        if (isEnemyTimer)
        {
            currTime += Time.deltaTime;
        }
        
    }

    void MoveStop()
    {
        //������Ʈ�� ��ü���� ��� ��������.
        enemyHP = marioPlayerMove.enHP;
        //Debug.Log(enemyHP);

        //���� ü���� ���� ü������ ���ٸ�
        if (currEnemyHP < enemyHP)
        {
            //print(1111);
            //����ü���� ����
            currEnemyHP = enemyHP;
            
            //����
            isEnemyMove = false;
            //��������
            isBulletFire = false;
            //Ÿ�̸Ӵ���
            isEnemyTimer = true;
            isEnemyHurt = true;
            if (isEnemyHurt)
            {
                Animator animator;
                animator = enemyAnim.GetComponent<Animator>();
                animator.SetTrigger("Hurt");
 
            }
            

            //0.5�� ������
            if (currTime > 0.5)
            {
                
                isEnemyTimer = false;
                currTime = 0;
                isEnemyMove = true;
            }
        }
    }


}//Ŭ���� ������
