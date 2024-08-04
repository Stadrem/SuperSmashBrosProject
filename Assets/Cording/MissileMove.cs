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
        //ù���� �μ��� foward�� �ְ� 2��° �μ��� up������.
        transform.rotation = Quaternion.LookRotation(Vector3.forward, playerDir); //playerDir, -playerDir,Vector3.right
       

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Contains("Player"))
        {
            //��ƼŬȿ��
            GameObject exploEffect = Instantiate(exploEffectFactory);
            exploEffect.transform.position = transform.position;
            ParticleSystem exploParticle =  exploEffect.GetComponent<ParticleSystem>();
            exploParticle.Play();
            Destroy(exploEffect, 2);
            
            //����
            AudioSource.PlayClipAtPoint(missileExploSound, Camera.main.transform.position);

            //������ó��, ����
            player.GetComponent<MarioPlayerMove>().Damaged(25, 5);
            Destroy(gameObject);
            
        }
    }

}
