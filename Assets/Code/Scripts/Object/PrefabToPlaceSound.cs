using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabToPlaceSound : MonoBehaviour
{
    AudioManager audioManager;
    public AudioClip audioPlaceObject;

    void Start()
    {
        audioManager = GetComponent<AudioManager>();

        audioManager.PlaySoundRandomPitch(audioPlaceObject);
    }
}
