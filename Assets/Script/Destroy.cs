using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Destroy : MonoBehaviour
{
    float currentTime = 0;
    float upTime = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > upTime)
        {
            transform.position += transform.up * 1;
            currentTime = 0;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (UiManager.instance.gameEnd == false)
        {
            UiManager.instance.LifeSub(collision.gameObject);
        }
    }
}
