using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    [ReadOnly] public PlayerData_Mechanics mechanics;

    [ReadOnly] public String predatorTag = "Predator";
    public ParticleSystem particle;

    void Start()
    {
        if(mechanics == null)
        {
            mechanics = GetComponentInParent<PlayerData_Mechanics>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o objeto colidido tem a tag "Predator"
        if (other.CompareTag(predatorTag))
        {
            // Inicia particula
            particle.Play();

            // Desativa o sprite e o collider
            CleanObjectAndCreateParticle();
        }
    }

    private void CleanObjectAndCreateParticle()
    {
        mechanics.CleanObject();
        particle.Play();
    }



}
