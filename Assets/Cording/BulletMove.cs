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
        //  Vector3 dir = new Vector3(1, 0, 0); 벡터변수 만들기
        targetPlayer = GameObject.Find("Mario");
        enemy = GameObject.Find("Enemy");
        
     

        #region  총알 방향 결정

        //애너미의 y회전값
        float enemyRotY = enemy.transform.rotation.y;
        //print("적의 rotation Y : " + enemyRotY); //왼쪽이면 1 //오른쪽은 0

        if (enemyRotY == 1)
        {
            //총알 방향
            bulletDir = -transform.right;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {   //총알 방향
            bulletDir = transform.right;
            //transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        #endregion

        //5초가 지나면 파괴
        Destroy(gameObject, 5);



    }

    // Update is called once per frame
    void Update()
    {

            transform.position += bulletDir * moveSpeed * Time.deltaTime;
            //print("총알의 방향은 " + bulletDir);
      

        #region  플레이어를 추적
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
            //서치, 컴포넌트 스크립트로 형변환, 데미지 함수 실행.
            targetPlayer.GetComponent<MarioPlayerMove>().Damaged(5, 1.0f);
            Destroy(gameObject);
        }
      

      

    }



}
