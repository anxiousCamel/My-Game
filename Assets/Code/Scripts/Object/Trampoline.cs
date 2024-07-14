using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [ReadOnly] public GameObject playerObject;
    [ReadOnly] public BoxCollider2D col;
    [ReadOnly] public Animator anim;
    [ReadOnly] public bool playerDetect;
    [ReadOnly] public AnimationState currentState;
    public float impulseForce;
    public LayerMask player;
    public enum AnimationState
    {
        Static,
        Impulse,
    }

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(playerDetect)
        {
            ChangeAnimationState(AnimationState.Impulse);
            Impulse();
            ResetTrampoline();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o objeto que colidiu está na camada do jogador
        if (((1 << other.gameObject.layer) & player) != 0)
        {
            playerDetect = true;
            playerObject = other.gameObject;
        }
    }

    void Impulse()
    {
        if (playerObject != null)
        {
            playerObject.GetComponent<PlayerData_Movement>().Jump.trampolineJump = true;
            Rigidbody2D rb = playerObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f); // Zera a velocidade vertical atual
                rb.AddForce(Vector2.up * impulseForce, ForceMode2D.Impulse); // Aplica a força de impulso
            }
        }
    }

    void ResetTrampoline()
    {
        playerDetect = false;
        playerObject = null;
    }

    public void ChangeAnimationState(AnimationState newState)
    {
        //Inicia a animação do novo estado e atualiza o estado atual
        anim.Play(newState.ToString());
        currentState = newState;
    }
}

