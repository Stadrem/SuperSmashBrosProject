using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class move : MonoBehaviour
{
    // Start is called before the first frame update

    //플로어 트랜스폼 0, -2, 0

    // 디버그 로그 룰
    //멤버는 여기서 설정
    //방향, 속도를 지정
    public Vector3 currentPosition;
    public Vector3 dirX;
    public Vector3 dirY;
    public Vector3 dirZ;
    public int moveSpeed = 30;
    public float jumpForce = 10f; // 점프 힘의 세기

    int playerHP = 0;
   
    void Start()
    {
        print("hello ");
        //P= P0 + vt

        //유니티에서는 Transform 컴포넌트를 사용하여 오브젝트의 위치를 설정할 수 있습니다.
        //이는 언리얼 엔진의 SetActorLocation과 비슷한 기능을 합니다.
        // 객체의 현재 위치를 가져오기 (getactorLocation)
        // 스타트를 해야 현재 위치를 알수 있음.
        currentPosition = transform.position;
        Debug.Log("현재 위치: " + currentPosition);
        print(currentPosition);

        // blue
        // dirX = Vector3.forward;
        dirX = new Vector3(0.01f, 0, 0);
        // red
        dirY = Vector3.right;
        // green
        dirZ = Vector3.up;

        // 객체의 위치를 (x, y, z)로 설정
        //Vector3 newPosition = new Vector3(x, y, z);
        //transform.position = newPosition;
        //Vector3 newPosition = new Vector3(1, 1, 1);

        //플레이어의 처음 위치를 구하자.
        //방향, 속도 값을 구한다.
        //플레이어를 움직인다.



}

    // Update is called once per frame
    void Update()
    {
        // // 현재 위치에서 타겟 위치로 부드럽게 이동
        //현재위치 = 벡터 러프 (현재위치 + 방향, 속도, 타임)
        //dirX = Vector3.forward;
        //dirY = Vector3.right;
        //transform.position += Vector3.Lerp(currentPosition, dirX, speed * Time.deltaTime );
        //transform.position = Vector3.Lerp(currentPosition, dirX, speed * Time.deltaTime);

        //오브젝트르 움직이자.
        // 방향, 속도, 시간
        //Vector3 dir = new Vector3(0, 1, 0);
        //transform.Translate(dir * 5 * Time.deltaTime);

        //입력 키값을 계속 감지
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 입력에 따라 캐릭터를 이동시킴
        Vector3 movement = new Vector3(horizontalInput, 0f, 0f) * 10f * Time.deltaTime;
        //transform.Translate(movement);
  

        //리지드 바디 velocity를 이용해서 속도를 제한, 타임은 내부적으로 처리.
        //
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(horizontalInput, 0f, 0f) * 10f;

        // 예를 들어, Space 키를 누르면 점프하도록 설정할 수 있음
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        //리지드 바디 
        //캐릭터 컨트롤러 


    }

    void Jump()
    {
        // 점프 로직 구현
        // 예를 들어, Rigidbody를 사용하여 중력에 따른 점프 처리
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            //rb.velocity = new Vector3(0f, 10f, 0f) * 10f;
            rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        }
    }
    void OnCollisionEnter(Collision other)
    {
        /* playerHP += 10;
         Debug.Log("playerHP: " + playerHP);*/



       

        if (other.gameObject.GetComponent("BulletMove"))
        {
            playerHP += 10;
            Debug.Log("playerHP: " + playerHP);
        }
        else
        {
            return;
        }


    }


}
