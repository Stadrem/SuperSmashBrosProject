using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Cord : MonoBehaviour
{
    public float speed = 1;
    Material mat;
   
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        //mat = mesh.material;
    }

    // Update is called once per frame
    void Update()
    {
        //mat.mainTextureOffset += Vector2.right * speed * Time.deltaTime;
    }

    void EnemyBullterFire()
    {
        //print("11111");
    }

}
