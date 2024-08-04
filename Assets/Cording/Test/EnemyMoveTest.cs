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
            //카메라의 위치를 바꾸려면 변수의 값은 참조값이니 실제값을 바꾸자.
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

            //위치값을 받는 로직
            PositionCal();
            //위치값 받는 변수를 변경하는 로직
            PositionONOFF();
        }
        


    }

    void PositionCal()
    {
        if (isOriginalPos)
        {
        
            //현재위치를 받자.
            nowPosX = transform.position.x;
            nowPos = transform.position;
            //print("현재 위치" + nowPosX);
        
        }
        else
        {
            //현재 위치를 모르니 이전 현재위치로 갱신
            beforePosX = nowPosX;
            beforePos = nowPos;
            //print("이전 위치" + beforePosX);
        }

        //x값의 변화량을 저장하는 함수
        CalPosX();

        currPos = Vector3.Distance(nowPos,beforePos);
        calPos += currPos;
        //print("이동위치 절대값 " + calPos);


    }

    void PositionONOFF()
    {
        if(isOriginalPos)
        {
            isOriginalPos = false;
            isOtherPos = true;
           //print("이전위치를 받자");
        }
        else
        {
            isOriginalPos = true;
            isOtherPos = false;
            //print("현재위치를 받자");
        }
    }

    void CalPosX()
    {
        //위치차이, 이전위치 - 현재위치;
        currPosX = nowPosX - beforePosX;
        calPosX += currPosX;
        //print("누적 이동거리" + calPosX);
    }

    void PlayerMove()
    {
        playerPos = player.transform.position;
        playerPos = transform.position;
        print("플레이어의 위치" + playerPos);
    }

}
