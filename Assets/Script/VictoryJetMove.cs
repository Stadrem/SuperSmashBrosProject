using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryJetMove : MonoBehaviour
{
    Transform trOrigin;

    // Start is called before the first frame update
    void Start()
    {
        trOrigin = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * 100 * Time.deltaTime;

        if(transform.position.x > 150)
        {
            transform.position = new Vector3(-60, trOrigin.position.y, trOrigin.position.z);
        }
    }
}
