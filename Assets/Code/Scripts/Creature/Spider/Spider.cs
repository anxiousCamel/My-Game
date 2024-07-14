using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [Header("Componetes")]
    public GameObject player;
    [ReadOnly] public Animator anim;
    [ReadOnly] public Rigidbody2D body;
    [ReadOnly] public BoxCollider2D col;
    [ReadOnly] public SpriteRenderer sprite;
    [ReadOnly] public EnemyGenerics generics;
    [ReadOnly] public AudioManager audioManager;
    [ReadOnly] public AudioSource audioSource;
    [ReadOnly] public ParticleSystem particle;
    [ReadOnly] public LineRenderer line;

    [Header("DownWeb")]
    [ReadOnly] public bool inPlatformMode;
    [ReadOnly] public bool isDown;
    public float speedDown;
    [ReadOnly] public Vector3 initialPointLine;
    public Transform lastPointLine;
    [Range(90, 1000)] public float rotationSpeed;

    [Header("Check")]
    public LayerMask solid;
    [Range(0, 10)] public float rayDistance;
    [ReadOnly] public bool isGround;
    [ReadOnly] public bool isWall;

    [Header("Movement")]
    [ReadOnly] Vector2 moveDirection;
    [ReadOnly] Vector2 targetDirection;
    [ReadOnly] Vector2 randomPosition;
    [ReadOnly] Vector2 moveForce;
    public Collider2D colliderArea;
    public float FollowRange;
    public float speed;
    public float jump;
    [ReadOnly] public float jumpTemporary;
    public float tolerance;

    [Header("Attack")]
    public float attackRange;
    public float speedAttack;
    public float jumpAttack;
    [ReadOnly] public bool attack;

    [Header("Animation")]
    public AnimationState currentState;

    public enum AnimationState
    {
        Idle,
        Walk,
        Attack,
        Waiting,
        Land,
        Down
    }


    void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        audioManager = GetComponent<AudioManager>();
        audioSource = GetComponent<AudioSource>();
        generics = GetComponent<EnemyGenerics>();
        sprite = GetComponent<SpriteRenderer>();
        line = GetComponent<LineRenderer>();
        col = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (player == null)
        {
            player = generics.GetPlayer();
        }

        SetLine();
    }

    void SetLine()
    {
        initialPointLine = transform.position;

        // Definindo os pontos da linha
        line.SetPosition(0, initialPointLine);
        line.SetPosition(1, lastPointLine.position);
    }

    // Update is called once per frame
    void Update()
    {
        isGround = generics.BoxCastCheckDirection(col, solid, Vector2.down);

        Vector2 origin = (Vector2)transform.position - new Vector2(0f, col.bounds.extents.y);
        isWall = generics.RaycastCheckDirection(origin, rayDistance, solid, Vector2.right)
        || generics.RaycastCheckDirection(origin, rayDistance, solid, Vector2.left);

        if (generics.canMove)
        {

            jumpTemporary = isWall ? jump : body.velocity.y;

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, isWall ? 90 : 0);

            // Movimentação normal
            if (inPlatformMode)
            {
                Flip();

                // Change Animations
                if (!attack)
                {
                    if (body.velocity.x == 0)
                    {
                        ChangeAnimationState(AnimationState.Idle);
                    }

                    else
                    {
                        ChangeAnimationState(AnimationState.Walk);
                    }
                }

                // Follow Player
                if (generics.InRange(transform.position, player.transform.position, FollowRange) && !generics.InRange(transform.position, player.transform.position, attackRange))
                {
                    targetDirection = player.transform.position - transform.position;
                    moveDirection = new Vector2(Mathf.Sign(targetDirection.x) * speed, jumpTemporary);
                    body.velocity = moveDirection;
                }

                else if (generics.InRange(transform.position, player.transform.position, attackRange))
                {
                    if (isGround)
                    {
                        ChangeAnimationState(AnimationState.Attack);
                    }
                }

                else
                {
                    // Determina a direção do movimento com base na posição atual em relação ao alvo
                    float direction = randomPosition.x > transform.position.x ? 1 : -1;

                    moveForce = new Vector2(speed * direction, jumpTemporary);

                    // Aplica a força ao corpo rígido para realizar o movimento horizontal
                    body.velocity = moveForce;
                }

                // sortear nova posição
                if (Mathf.Abs(transform.position.x - randomPosition.x) < tolerance)
                {
                    randomPosition.x = generics.GetRandomPosition(colliderArea).x;
                }
            }

            else
            {

                // Trocar de posição
                if (!isDown && isGround)
                {
                    ChangeAnimationState(AnimationState.Land);
                    body.gravityScale = 1;
                }

                // Descer a teia
                if (isDown)
                {
                    // Alterar animação
                    ChangeAnimationState(AnimationState.Down);

                    if (!isGround)
                    {
                        // Mover para baixo
                        body.velocity = new Vector2(0, -speedDown);
                    }

                    else
                    {
                        isDown = false;
                    }

                    // DrawLine
                    line.enabled = true;
                    line.SetPosition(0, initialPointLine);
                    line.SetPosition(1, lastPointLine.position);

                    // Rotation
                    float rotationAngle = rotationSpeed * Time.deltaTime;
                    transform.Rotate(0, rotationAngle, 0);
                }

                else
                {
                    // reset rotation
                    transform.rotation = Quaternion.Euler(0, 0, 0);

                    // enable line
                    line.enabled = false;
                    body.velocity = Vector2.zero;
                }
            }
        }
    }

    public void Attack() // Gatilho por animação
    {

        // Determina a direção do movimento com base na posição atual em relação ao alvo
        float direction = player.transform.position.x > transform.position.x ? 1 : -1;

        // Aplica uma força de impulso vertical para realizar o pulo
        Vector2 AttackForce = new Vector2(speedAttack * direction, jumpAttack);

        body.AddForce(AttackForce, ForceMode2D.Impulse);

    }

    public void FinishAttack()
    {
        attack = false;
        body.velocity = new Vector2(0, 0);
    }

    public void StartAttack()
    {
        attack = true;
    }

    void Flip()
    {
        // Flip
        if (body.velocity.x >= 0.1)
        {
            transform.localRotation = Quaternion.Euler(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        }
        else if (body.velocity.x <= -0.1)
        {
            transform.localRotation = Quaternion.Euler(transform.eulerAngles.x, 180, transform.eulerAngles.z);
        }
    }


    public void FinishLand() // chamado por evento do animator
    {
        inPlatformMode = true;
        randomPosition.x = generics.GetRandomPosition(colliderArea).x;
    }

    public void ChangeDown()
    {
        if (inPlatformMode)
        {
            return;
        }

        isDown = true;
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
