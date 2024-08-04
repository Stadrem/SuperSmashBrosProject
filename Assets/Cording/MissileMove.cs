using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MissileMove : MonoBehaviour
{
    public GameObject exploEffectFactory;
    public AudioClip missileExploSound;
    GameObject player;
    

    Vector3 playerPos;
    Vector3 missilePos;
    Vector3 playerDir;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Mario");
        
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
        missilePos = transform.position;
        playerDir = playerPos - missilePos;
        playerDir.Normalize();

        transform.position += playerDir * 13 * Time.deltaTime;
        //첫번재 인수에 foward를 넣고 2번째 인수에 up을넣자.
        transform.rotation = Quaternion.LookRotation(Vector3.forward, playerDir); //playerDir, -playerDir,Vector3.right
       

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Contains("Player"))
        {
            //파티클효과
            GameObject exploEffect = Instantiate(exploEffectFactory);
            exploEffect.transform.position = transform.position;
            ParticleSystem exploParticle =  exploEffect.GetComponent<ParticleSystem>();
            exploParticle.Play();
            Destroy(exploEffect, 2);
            
            //사운드
            AudioSource.PlayClipAtPoint(missileExploSound, Camera.main.transform.position);

            //데미지처리, 삭제
            player.GetComponent<MarioPlayerMove>().Damaged(25, 5);
            Destroy(gameObject);
            
        }
    }

}
