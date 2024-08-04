using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyGo : MonoBehaviour
{
    float currentTime = 0;
    float readyTime = 2.0f;
    public MarioPlayerMove mpm;
    public EnemyMove em;

    // Start is called before the first frame update
    void Start()
    {
        mpm.enabled = false;
        em.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > readyTime)
        {
            mpm.enabled = true;
            em.enabled = true;

            Destroy(gameObject, 2.0f);
        }
    }
}
