using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Sounds")]
    [ReadOnly] public AudioSource audioSource;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlaySound(AudioClip sound)
    {
        if (audioSource != null && audioSource.enabled)
        {
            audioSource.clip = sound;
            audioSource.Play();
        }
    }

    void Update()
    {
        if (audioSource != null && audioSource.enabled)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = null;
            }
        }
    }
}
