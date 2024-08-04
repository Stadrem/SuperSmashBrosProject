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
        //Floor 태그가 붙은 오브젝트가 닿으면 점프 초기화.
        if (collision.gameObject.CompareTag("Floor"))
        {
            marioPlayerMoveSC.FloorCheck();
        }
    }
}
