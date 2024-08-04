using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyDodge : MonoBehaviour
{
    public AudioClip dodgeSound;
    public ParticleSystem dodgeEffectFactory;
    public int beforeHP = 0;
    public int dodgeHP = 20;
    public bool isDodgeActive = false;

    GameObject enemyAnim;
    GameObject player;
    GameObject mineManager;
    MarioPlayerMove playerMove;
    Collider enemyCapColl;
    
    EnemyMove enemyMove;
   public EnemyMineManager enemyMineManager;

    Vector3 playerDir;
    float playerDIrX;
    float playerDist;
    float currTime = 0;


    int enemyHP;
    int currHP = 0;
    
    
    bool isCurrTimeActive = false;
    bool isDodgeMove = false;
    // Start is called before the first frame update
    void Start()
    {
        enemyCapColl = gameObject.GetComponent<CapsuleCollider>();
        enemyAnim = GameObject.Find("EnemyGun");
        player = GameObject.Find("Mario");
        playerMove = player.GetComponent<MarioPlayerMove>();
        
       mineManager = GameObject.Find("EnemyMineManager");
       enemyMineManager = mineManager.GetComponent<EnemyMineManager>();
       enemyMove = gameObject.GetComponent<EnemyMove>();
        
    }

    // Update is called once per frame
    void Update()
    {
       

        playerDir = player.transform.position - transform.position;
        playerDIrX = playerDir.x;
        playerDist = Vector3.Distance(player.transform.position, transform.position);

        enemyHP = UiManager.instance.enHP;
       
        
        currHP = enemyHP;
        if(currHP == 0)
        {
            beforeHP = currHP;
        }

        if (beforeHP + dodgeHP < currHP)
        {
            isDodgeActive = true;
            
        }

        if (isDodgeActive)
        {
            //����� ������.
            playerMove.isEnBlocking = true;
            //�� �̵�����
            enemyMove.isEnemyMove = false;
            //�� ��������
            enemyMove.isBulletFire = false;
            //�� ��ź����
            enemyMineManager.isMineManagerUpdate = false;
            //�ð�����
            isCurrTimeActive = true;
            //�ִϸ��̼� ������
            Animator animator;
            animator = enemyAnim.GetComponent<Animator>();
            animator.SetTrigger("Rolling");
            AudioSource.PlayClipAtPoint(dodgeSound, Camera.main.transform.position);
            
            //ĸ�� �ݸ����� ������.
            //enemyCapColl.enabled = false;
            
            //ü�´���
            beforeHP = currHP;
            isDodgeActive = false;
            //
            isDodgeMove = true;




        }

        //�ð�����
        if(isCurrTimeActive)
        {
            currTime += Time.deltaTime;

        }

        if(1 < currTime)
        {
            
            if(isDodgeMove)
            {
                isDodgeMove = false;
                //�Ÿ��� ������ �÷��̾� �ڷ� �̵�
                if (playerDist < 4)
                { 
                    transform.position += new Vector3(playerDIrX * 1.5f, 0, 0);
                    ParticleSystem dodgeEffect = Instantiate(dodgeEffectFactory);
                    dodgeEffect.transform.position = transform.position;
                    dodgeEffect.transform.localScale = new Vector3(3, 3, 3);
                    dodgeEffect.Play();
                    Destroy(dodgeEffect.gameObject, 2);
                   

                }
                
            }
           

        }

        //2�������� ����� ������.
        if (2 < currTime)
        {
           
            playerMove.isEnBlocking = false;
            isCurrTimeActive = false;
            enemyMove.isEnemyMove = true;
            enemyMove.isBulletFire = true;
            enemyMineManager.isMineManagerUpdate = true;
            currTime = 0;
           


        }



    }//������Ʈ ��

    void CurrentEnemyDamage(int value)
    {
      

    }

    void Timer()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Contains("Player"))
        {
            transform.position += player.transform.position - new Vector3(10, 0, 0);
        }
    }

}
