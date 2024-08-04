using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class BulletMove : MonoBehaviour

{
    public float moveSpeed = 5;

    GameObject targetPlayer;
    GameObject enemy;
    Vector3 bulletDir;
    

    // Start is called before the first frame update
    void Start()
    {
        //  Vector3 dir = new Vector3(1, 0, 0); ���ͺ��� �����
        targetPlayer = GameObject.Find("Mario");
        enemy = GameObject.Find("Enemy");
        
     

        #region  �Ѿ� ���� ����

        //�ֳʹ��� yȸ����
        float enemyRotY = enemy.transform.rotation.y;
        //print("���� rotation Y : " + enemyRotY); //�����̸� 1 //�������� 0

        if (enemyRotY == 1)
        {
            //�Ѿ� ����
            bulletDir = -transform.right;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {   //�Ѿ� ����
            bulletDir = transform.right;
            //transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        #endregion

        //5�ʰ� ������ �ı�
        Destroy(gameObject, 5);



    }

    // Update is called once per frame
    void Update()
    {

            transform.position += bulletDir * moveSpeed * Time.deltaTime;
            //print("�Ѿ��� ������ " + bulletDir);
      

        #region  �÷��̾ ����
        /*Quaternion enemyRot =  Enemy.transform.rotation;
        float enemyRotY = enemyRot.y;
        float bulletRoty = transform.rotation.y;
       

        if (enemyRotY == 0)
        {

            //moveForece = 5;
            moveForece = -5;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            //moveForece = 5;
            moveForece = -5;
            //transform.rotation = Quaternion.Euler(0, 0, 0);


        }*/
        #endregion

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Contains("Player"))
        {
            //��ġ, ������Ʈ ��ũ��Ʈ�� ����ȯ, ������ �Լ� ����.
            targetPlayer.GetComponent<MarioPlayerMove>().Damaged(5, 1.0f);
            Destroy(gameObject);
        }
      

      

    }



}
