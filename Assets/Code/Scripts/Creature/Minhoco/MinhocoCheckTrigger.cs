using System;
using UnityEngine;

public class MinhocoCheckTrigger : MonoBehaviour
{
    Minhoco minhoco;

    private void Start() 
    {
        minhoco = GetComponentInParent<Minhoco>();
    }

    [ReadOnly] public String tagPlayer = "Player";
    [ReadOnly] public String tagLaunched = "Launched";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tagLaunched) || collision.CompareTag(tagPlayer))
        {
            minhoco.IsAttack();
        }
    }
}
