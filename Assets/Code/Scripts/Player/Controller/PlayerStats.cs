using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float life;

    public void AddHealth(float health)
    {
        life += health;
    }

    public void RemoveHealth(float health)
    {
        life -= health;
    }
}
