using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCollision : MonoBehaviour
{
    public MarioPlayerMove marioPlayerMoveSC;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        //Floor �±װ� ���� ������Ʈ�� ������ ���� �ʱ�ȭ.
        if (collision.gameObject.CompareTag("Floor"))
        {
            marioPlayerMoveSC.FloorCheck();
        }
    }
}
