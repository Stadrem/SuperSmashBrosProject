using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    public GameObject particleRun;
    public ParticleSystem sysRun;

    public GameObject particleHit;
    ParticleSystem sysHit;

    public GameObject particleBlock;
    ParticleSystem sysBlock;

    public GameObject particleUlti;
    ParticleSystem sysUlti;

    public GameObject particleRange;
    ParticleSystem sysRange;

    public GameObject particleKnockP;
    ParticleSystem sysKnockP;

    public GameObject particleKnockE;
    ParticleSystem sysKnockE;

    public GameObject particleLanding;
    ParticleSystem sysLanding;

    GameObject player;
    GameObject enemy;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        player = GameObject.Find("Player");
        enemy = GameObject.Find("Enemy");

        sysRun = particleRun.GetComponent<ParticleSystem>();
        sysHit = particleHit.GetComponent<ParticleSystem>();
        sysBlock = particleBlock.GetComponent<ParticleSystem>();
        sysUlti = particleUlti.GetComponent<ParticleSystem>();
        sysRange = particleRange.GetComponent<ParticleSystem>();
        sysKnockP = particleKnockP.GetComponent<ParticleSystem>();
        sysKnockE = particleKnockE.GetComponent<ParticleSystem>();
        sysLanding = particleLanding.GetComponent<ParticleSystem>();

        particleKnockP.transform.parent = player.transform;
        particleKnockP.transform.position = player.transform.position;
        particleKnockP.transform.position += new Vector3(0,0,1);
        particleKnockE.transform.parent = enemy.transform;
        particleKnockE.transform.position = enemy.transform.position;
        particleKnockE.transform.position += new Vector3(0, 0, 1);
    }

    public void RunEffectPlay()
    {
        sysRun.Play();
    }
    public void RunEffectStop()
    {
        sysRun.Stop();
    }

    public void KnockPStart()
    {
        sysKnockP.Play();
    }

    public void KnockEStart()
    {
        sysKnockE.Play();
    }

    public void HitEffect(Vector3 position)
    {
        particleHit.transform.position = new Vector3(position.x, position.y + 1.5f, -1);
        sysHit.Play();
    }

    public void BlockEffect()
    {
        sysBlock.Play();
    }

    public void UltiEffect()
    {
        sysUlti.Play();
    }

    public void RangeEffect()
    {
        sysRange.Play();
    }

    public void LandingEffect()
    {
        sysLanding.Play();
    }
}
