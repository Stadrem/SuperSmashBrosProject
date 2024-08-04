using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource audioSource;
    public AudioClip hitAudio;
    public AudioClip attackAudio;
    public AudioClip jumpAudio;
    public AudioClip floorAudio;
    public AudioClip ultiAudio;
    public AudioClip startUltiAudio;
    public AudioClip damagedAudio;
    public AudioClip dashAudio;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void HitAudio()
    {
        audioSource.PlayOneShot(hitAudio);
        audioSource.volume = 0.5f;
    }

    public void AttackAudio()
    {
        audioSource.PlayOneShot(attackAudio);
        audioSource.volume = 0.5f;
    }

    public void JumpAudio()
    {
        audioSource.PlayOneShot(jumpAudio);
        audioSource.volume = 0.2f;
    }

    public void FloorAudio()
    {
        audioSource.PlayOneShot(floorAudio);
        audioSource.volume = 1.0f;
    }

    public void UltiAudio()
    {
        audioSource.PlayOneShot(ultiAudio);
        audioSource.volume = 1.0f;
    }

    public void StartUltiAudio()
    {
        audioSource.PlayOneShot(startUltiAudio);
        audioSource.volume = 0.5f;
    }

    public void DamagedAudio()
    {
        audioSource.PlayOneShot(damagedAudio);
        audioSource.volume = 0.8f;
    }

    public void DashAudio()
    {
        audioSource.PlayOneShot(dashAudio);
        audioSource.volume = 0.8f;
    }
}
