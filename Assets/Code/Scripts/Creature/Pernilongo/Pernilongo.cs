using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pernilongo : MonoBehaviour
{
    [Header("Componetes")]
    [ReadOnly] public ParticleSystem particle;
    [ReadOnly] public Rigidbody2D body;
    [ReadOnly] public BoxCollider2D col;
    [ReadOnly] public SpriteRenderer sprite;
    [ReadOnly] public EnemyGenerics generics;
    [ReadOnly] public AudioManager audioManager;
    [ReadOnly] public AudioSource audioSource;
    [ReadOnly] public Animator anim;
    public GameObject player;

    [Header("Attack")]
    [ReadOnly] public String predatorTag = "Player";
    [ReadOnly] public String solidTag = "Downloadable";
    public bool countAttack;
    public bool isAttack;
    public float lastAttack;
    public float cooldown;
    public bool collWithPlayer;
    public bool collWithGroud;

    [Header("Delimitadores")]
    [ReadOnly] public Vector2 target;
    [Range(0, 9)] public float tolerance;
    public BoxCollider2D colliderArea;


    [Header("Movement")]
    [ReadOnly] public Vector2 moveDirection;
    public float speed;
    public float attackSpeed;
    public float range;

    [Header("Animation")]
    public AnimationState currentState;
    public enum AnimationState
    {
        Idle,
        Walk,
        Attack,
        Dash
    }

    void Start()
    {
        generics = GetComponent<EnemyGenerics>();
        audioManager = GetComponent<AudioManager>();
        body = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        particle = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();

        SetRandomTargetPosition();

        if (player == null)
        {
            player = generics.GetPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Verifica se o objeto colidido tem a tag "Predator"
        if (col.CompareTag(predatorTag))
        {
            collWithPlayer = true;
        }

        if (col.CompareTag(solidTag))
        {
            collWithGroud = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        // Verifica se o objeto colidido tem a tag "Predator"
        if (col.CompareTag(predatorTag))
        {
            collWithPlayer = false;
        }

        if (col.CompareTag(solidTag))
        {
            collWithGroud = false;
        }
    }

    void Update()
    {
        Flip();
        if (generics.canMove)
        {
            // Cooldown de ataque
            if (countAttack)
            {
                lastAttack++;
                if (lastAttack >= cooldown)
                {
                    countAttack = false;
                    lastAttack = 0;
                }
            }


            // Verifica se o inimigo chegou ao destino
            if (Vector2.Distance(transform.position, target) <= tolerance)
            {
                // Define uma nova posição aleatória como destino
                SetRandomTargetPosition();
            }

            // Verifica se o inimigo chegou ao destino
            if (Vector2.Distance(transform.position, moveDirection) <= tolerance)
            {
                //CancelAttack();
            }

            if (isAttack)
            {
                Attack();

                if (collWithGroud || collWithPlayer || !CanAttack())
                {
                    CancelAttack();
                }
            }

            if (CanAttack())
            {
                ChangeAnimationState(AnimationState.Attack);
            }

            else
            {

                // Move o inimigo em direção ao destino
                moveDirection = (target - (Vector2)transform.position).normalized;
                body.velocity = moveDirection * speed;
                ChangeAnimationState(AnimationState.Walk);
            }
        }

    }

    void CancelAttack()
    {
        countAttack = true;
        isAttack = false;
    }

    void Attack()
    {
        //! colocar pra ele ir RETO AO INFINITO E ALEM até ou bater no jogador ou na parede
        body.velocity = moveDirection * attackSpeed;


        // Calcula o ângulo de rotação
        float angleZ = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

        // Verifica a direção do movimento
        float Direction = Mathf.Sign(body.velocity.x);

        // Ajusta a rotação para a esquerda
        if (Direction < 0)
        {
            angleZ += 180f;
        }

        // Aplica a rotação
        transform.rotation = Quaternion.Euler(0f, Direction < 0 ? 180f : 0f, angleZ);
    }

    void GetDirectionAttack()
    {
        moveDirection = (player.transform.position - transform.position).normalized;
    }

    void StartAnimDash()
    {
        isAttack = true;
        GetDirectionAttack();
        ChangeAnimationState(AnimationState.Dash);
    }


    bool CanAttack()
    {
        return generics.InRange(transform.position, player.transform.position, range) && lastAttack == 0;
    }

    void Flip()
    {
        if (body.velocity.x >= 0.1)
        {
            transform.localRotation = Quaternion.Euler(0, 0, transform.localRotation.z);
        }
        else if (body.velocity.x <= -0.1)
        {
            transform.localRotation = Quaternion.Euler(0, 180, transform.localRotation.z);
        }
    }

    void SetRandomTargetPosition()
    {
        target = generics.GetRandomPosition(colliderArea);
    }

    private void ChangeAnimationState(AnimationState newState)
    {
        //Se o novo estado for o mesmo que o estado atual, não faz nada
        if (currentState == newState)
        {
            return;
        }

        //Inicia a animação do novo estado e atualiza o estado atual
        anim.Play(newState.ToString());
        currentState = newState;
    }

}
