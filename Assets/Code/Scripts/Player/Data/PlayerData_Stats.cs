using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData_Stats : MonoBehaviour
{
    [ReadOnly] public float life;  // Vida atual do jogador
    public float maxLife;
    [ReadOnly] public float stamina;  // Stamina atual do jogador
    public float maxStamina;
    public SpriteRenderer playerSprite;  // Referência ao SpriteRenderer do jogador
    [HideInInspector] public Coroutine blinkCoroutine;  // Referência à corrotina de piscar

    // Adiciona uma quantidade de saúde ao jogador
    public void AddHealth(float health)
    {
        life += health;
    }

    // Remove uma quantidade de saúde do jogador
    public void RemoveHealth(float health)
    {
        life -= health;
    }

    // Adiciona uma quantidade de stamina ao jogador
    public void AddStamina(float energy)
    {
        stamina += energy;
    }

    // Remove uma quantidade de stamina do jogador
    public void RemoveStamina(float energy)
    {
        stamina -= energy;
    }
}
