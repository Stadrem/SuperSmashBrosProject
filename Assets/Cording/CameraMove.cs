using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEditor;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    GameObject Player;
    GameObject Enemy;
    Vector3 PlayerPos;
    Vector3 EnemyPos;
    float PlayerPos_X;
    float EnemyPos_X;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        Enemy = GameObject.Find("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        
        //계속 적 위치를 가져오자.
        EnemyPos = Enemy.transform.position;
        //계속 적 위치의 x값을 가져오자.
        EnemyPos_X = EnemyPos.x;
        print(EnemyPos_X);

        
        //플레이어와 적 사이의 거리가 멀어지면
        //카메라 앵글을 뒤로하자.




        //플레이어 위치를 가져오자
        PlayerPos = Player.transform.position;
    }
}
