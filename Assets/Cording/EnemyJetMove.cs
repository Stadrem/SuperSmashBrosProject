using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJetMove : MonoBehaviour
{
    public GameObject missileFactory;
    public AudioClip jetSound;

    float currTime = 0;
    float currTime1 = 0;
    Vector3 dir;
    Vector3 jetPos;

    bool isMissileFire = true;
    bool isMissileFire1 = true;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(140, -1, -12);
        dir = -Vector3.right;
        dir.Normalize();
        //AudioSource.PlayClipAtPoint(jetSound, transform.position);
        AudioSource jetSound1 =  GetComponent<AudioSource>();
        jetSound1.Play();
    }

    // Update is called once per frame
    void Update()
    {

        jetPos = transform.position;
        transform.position += dir * 15 * Time.deltaTime;
        currTime += Time.deltaTime;
       

        //if(transform.position.x <= 60)
        if (2 <= currTime )
        {
           
            if (isMissileFire)
            {
                MissileFire();
                isMissileFire = false;
            }
            
        }
        if (4 <= currTime)
        {
            if (isMissileFire1)
            {
                MissileFire();
                isMissileFire1 = false;
            }
            
            
        }
        if (8 <= currTime)
        {
            
            currTime = 0;
            transform.position = new Vector3(140, -1, -12);
            gameObject.GetComponent<AudioSource>().enabled = false;
            Destroy(gameObject);
            /*isMissileFire = true;
            isMissileFire1 = true;*/
        }
        

    }//업데이트 끝
    void MissileFire()
    {
        GameObject missile = Instantiate(missileFactory);
        missile.transform.position = new Vector3(40, 5, 0);
        
    }
    
}
