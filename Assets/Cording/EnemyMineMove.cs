using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor;
//using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Audio;

public class EnemyMineMove : MonoBehaviour
{
    public ParticleSystem bombExploFactory;
    public float bombKnockBack = 10;
    public float bombMoveSpeed = 1;

    //public AudioResource exploSound;
    public AudioClip exploSound;

    GameObject targetPlayer;
    GameObject enemy;
    GameObject bombObject;
    Vector3 playerPos;
    Vector3 playerDir;
    int rand;
    float currTime = 0;
   
    
    bool isEnemyMineMove = false;
    bool isBombTimer = false;
    bool isBombAnim = false;

    Collider playerColl;
    Quaternion mineRot;
    Material material02;

    // Start is called before the first frame update
    void Start()
    {
        //�÷��̾� ã��
        targetPlayer = GameObject.Find("Mario");
        //�÷��̾� ��ġ�� ������.
        playerPos = targetPlayer.transform.position;
        //�÷��̾� ������ ������.
        float playerDirX = playerPos.x - transform.position.x;
        playerDir = new Vector3(playerDirX, 0,0);

        bombObject = GameObject.Find("Bomb");

        playerDir.Normalize();
        //���� 0~1
        rand = Random.Range(0, 1);
        playerColl = targetPlayer.GetComponent<Collider>();

        //����ã��
        enemy = GameObject.Find("Enemy");


        //currTime += Time.deltaTime;

        //������ ��Ƽ���� ã��.
        GameObject bomb = GameObject.Find("BombMat");
        SkinnedMeshRenderer  skMesh = bomb.GetComponent<SkinnedMeshRenderer>();
        Material[] materials = skMesh.materials;
        material02 = materials[0];
        //material02.color = Color.red;
        //�ؽ�ó�� �ٲ��ش�.
        //material02.SetTexture();
        


        /*for (int i = 0; i < materials.Length; i++)
        {
            print(materials[i].name);
        }*/





    }

   
    // Update is called once per frame
    void Update()
    {

        if (isBombTimer)
        {
            BomeTimer();
        }

        if (transform.position.y > 0.5)
        {
            transform.position += new Vector3(0, -1, 0);
        }


        if (isEnemyMineMove)
        {
            
            transform.position += playerDir * bombMoveSpeed * Time.deltaTime;
            //����������� �Ӹ��� ������.
            //transform.rotation = Quaternion.LookRotation(playerDir, -Vector3.right);
            //transform.rotation = Quaternion.LookRotation(playerDir, Vector3.up);//);
            //transform.rotation = Quaternion.LookRotation(playerDir);
            Vector3 targetrDir= Vector3.Cross(Vector3.up, playerDir);
            transform.rotation = Quaternion.LookRotation(targetrDir, Vector3.up);
        }

       

        BombAnim();






    }//������Ʈ
    bool isBombColor = true;
    void BomeTimer()
    {
        currTime += Time.deltaTime;
       

        if (1 < currTime)
        {
            material02.color = Color.red;
            isBombColor = false;
        }

    }
    
   

    void OnTriggerEnter(Collider other)
    {
        //print("�浹�� ��� : " + other); //ũ�� x�� 10
        
        if (other.gameObject.name.Contains("Player"))
        {
            isBombTimer = true;
            isEnemyMineMove = true;
            isBombAnim = true;
            //isBombAnim = false;

        }

       



    }
    
     void OnTriggerStay(Collider other)
    {
        
       
        if  (other.gameObject.name.Contains("Player"))
        {

       

           //2�������� �浹���̸� ������+����
            if (2 < currTime)
            {
                //��ġ, ������Ʈ ��ũ��Ʈ�� ����ȯ, ������ �Լ� ����.
                targetPlayer.GetComponent<MarioPlayerMove>().Damaged(20, bombKnockBack);

                BombExploEffect();
                isBombTimer = false;
                //isBombAnim = false;



                 currTime = 0;
                Destroy(gameObject);
            }
        }
        else
        {
            //3�������� ����
            if(3 < currTime)
            {
                //print(3333);
                BombExploEffect();
                AudioSource expleAudioSource = GetComponent<AudioSource>();
                if(expleAudioSource.resource.name.Contains("Kick"))
                {
                    expleAudioSource.Play();
                }
                expleAudioSource.enabled = false;
                isBombTimer = false;
                //isBombAnim = false;
                currTime = 0;
                Destroy(gameObject);
            }
        }
    }

    void BombExploEffect()
    {
        ParticleSystem bombExploEffect = Instantiate(bombExploFactory);
        bombExploEffect.transform.position = transform.position;
        //ParticleSystem bombExploEffect = bombExplo.GetComponent<ParticleSystem>();
        bombExploEffect.Play();
 
        AudioSource.PlayClipAtPoint(exploSound, Camera.main.transform.position);       
        Destroy(bombExploEffect.gameObject, 2);
    }

    void BombAnim()
    {
        if (gameObject == null) return;
        
        if (isBombAnim)
        {
            Animator animator = GetComponent<Animator>();
            animator.SetTrigger("Walk");
            isBombAnim = false;
        }
    }
        
} //�ڵ峡

