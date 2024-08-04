using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;




public class EnemyMoveTest : MonoBehaviour
{
    public Camera mainCameraFactory;
    public GameObject player;

    bool isBackMove = false;
    bool isOriginalPos = false;
    bool isOtherPos = false;

    float nowPosX = 0;
    float beforePosX = 0;
    float currPosX = 0;
    float calPosX = 0;

    Vector3 EnemyPos;
    Vector3 nowPos;
    Vector3 beforePos;
    Vector3 playerPos;

    Vector3 mianCameaPos;


    float currPos = 0;
    float calPos = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");


    }

   

    // Update is called once per frame
    void Update()
    {
        EnemyPos = transform.position;


        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            //ī�޶��� ��ġ�� �ٲٷ��� ������ ���� �������̴� �������� �ٲ���.
            mainCameraFactory.transform.position = EnemyPos;


            //Camera.main.transform.position = EnemyPos;
            //print(cameraPos);

          /*  mianCameaPos = mainCameraFactory.transform.position;
            mianCameaPos = EnemyPos;*/

          ;
            
        } 

        if(calPosX < 10)
        {
            //transform.position += Vector3.right * 1 * Time.deltaTime;
            //transform.position += Vector3.left * 1 * Time.deltaTime;

            //��ġ���� �޴� ����
            PositionCal();
            //��ġ�� �޴� ������ �����ϴ� ����
            PositionONOFF();
        }
        


    }

    void PositionCal()
    {
        if (isOriginalPos)
        {
        
            //������ġ�� ����.
            nowPosX = transform.position.x;
            nowPos = transform.position;
            //print("���� ��ġ" + nowPosX);
        
        }
        else
        {
            //���� ��ġ�� �𸣴� ���� ������ġ�� ����
            beforePosX = nowPosX;
            beforePos = nowPos;
            //print("���� ��ġ" + beforePosX);
        }

        //x���� ��ȭ���� �����ϴ� �Լ�
        CalPosX();

        currPos = Vector3.Distance(nowPos,beforePos);
        calPos += currPos;
        //print("�̵���ġ ���밪 " + calPos);


    }

    void PositionONOFF()
    {
        if(isOriginalPos)
        {
            isOriginalPos = false;
            isOtherPos = true;
           //print("������ġ�� ����");
        }
        else
        {
            isOriginalPos = true;
            isOtherPos = false;
            //print("������ġ�� ����");
        }
    }

    void CalPosX()
    {
        //��ġ����, ������ġ - ������ġ;
        currPosX = nowPosX - beforePosX;
        calPosX += currPosX;
        //print("���� �̵��Ÿ�" + calPosX);
    }

    void PlayerMove()
    {
        playerPos = player.transform.position;
        playerPos = transform.position;
        print("�÷��̾��� ��ġ" + playerPos);
    }

}
