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
        
        //��� �� ��ġ�� ��������.
        EnemyPos = Enemy.transform.position;
        //��� �� ��ġ�� x���� ��������.
        EnemyPos_X = EnemyPos.x;
        print(EnemyPos_X);

        
        //�÷��̾�� �� ������ �Ÿ��� �־�����
        //ī�޶� �ޱ��� �ڷ�����.




        //�÷��̾� ��ġ�� ��������
        PlayerPos = Player.transform.position;
    }
}
