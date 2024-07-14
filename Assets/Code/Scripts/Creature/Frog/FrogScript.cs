using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogScript : MonoBehaviour
{
    [Header("Components")]
    private Animator anim;
    private Rigidbody2D body;
    private LineRenderer line;
    private CapsuleCollider2D col;
    public EnemyGenerics generics;
    private EdgeCollider2D edge;
    private SpriteRenderer sprite;

    [Header("PlayerInteraction")]
    public bool inRange;
    public GameObject player;
    public float range;


    [Header("Animation")]
    public AnimationState currentState;
    public enum AnimationState
    {
        Idle,
        Walk,
        Attack
    }


    [Header("Attack")]
    public float preyRange;
    [ReadOnly] public int indexPrey;
    public bool countAttack;
    public bool isAttack;
    public float lastAttack;
    public Transform firePoint;
    public List<GameObject> prey = new();
    public float cooldown;


    [Header("Check")]
    [Range(0, 10)] public float rayDistance;
    public bool isGround;
    public bool isWall;
    public LayerMask solid;


    [Header("Delimitadores")]
    public BoxCollider2D colliderArea;


    [Header("Movimento")]
    public Vector2 targetMove;
    public float timeRest;
    public float tolerance;
    public float jumpVelocity;
    public float extrajumpVelocity;
    public float movementSpeed;
    public float timeStopped;


    private void Awake()
    {
        targetMove.x = generics.GetRandomPosition(colliderArea).x;
        edge = GetComponentInChildren<EdgeCollider2D>();
        line = GetComponentInChildren<LineRenderer>();
        col = GetComponent<CapsuleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        ValidatePrey();

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

    }

    bool CanAttack()
    {
        if (isGround && !generics.canMove && lastAttack == 0 && timeStopped >= timeRest / 2)
        {
            indexPrey = RandomPrey();
            return inRange || indexPrey != -1;
        }

        return false;
    }

    void Update()
    {
        inRange = generics.InRange(transform.position, player.transform.position, range);
        isGround = generics.CapsuleCastCheckDirection(col, solid, Vector2.down);
        
        Vector2 origin = (Vector2)transform.position - new Vector2(0f, col.bounds.extents.y);
        isWall = generics.RaycastCheckDirection(origin, rayDistance, solid, Vector2.right)
        || generics.RaycastCheckDirection(origin, rayDistance, solid, Vector2.left);

        // Flip
        if (generics.canMove)
        {
            if (body.velocity.x >= 0.1)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (body.velocity.x <= -0.1)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }

        if (CanAttack())
        {
            isAttack = true;
            countAttack = true;
        }


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

        // Verifica se o personagem está próximo o suficiente do alvo horizontalmente
        bool isNearTarget = Mathf.Abs(transform.position.x - targetMove.x) < tolerance;

        // se estiver já no alvo sortear nova posição enquanto descansa
        if (isNearTarget)
        {
            targetMove.x = generics.GetRandomPosition(colliderArea).x;
            if (isGround)
            {
                generics.canMove = false;
            }
        }

        if (!generics.canMove)
        {
            timeStopped++;
        }

        // mover para o target depois de descansar
        if (timeStopped >= timeRest)
        {
            timeStopped = 0;
            generics.canMove = true;
        }

        // gatilho de animção andar
        if (generics.canMove && !isAttack)
        {
            ChangeAnimationState(AnimationState.Walk);
        }
        // gatilho de animção parado
        else if (!isAttack && isGround)
        {
            ChangeAnimationState(AnimationState.Idle);
        }
        // gatilho de animção atacar
        else
        {
            if (prey.Count > 0 || inRange)
            {
                ChangeAnimationState(AnimationState.Attack);
            }
        }
    }
    public void Attack() // acionado por evento do animator
    {
        Vector3[] positions = new Vector3[2];
        generics.canMove = false;
        transform.localRotation = Quaternion.Euler(0, 0, 0);

        if (inRange)  // obter player
        {

            positions[0] = firePoint.position;
            positions[1] = player.transform.position;

        }

        else if (prey.Count > 0) // obter presa e posição
        {
            {
                if (prey[indexPrey] != null)
                {
                    Vector3 preyPosition = prey[indexPrey].transform.position;

                    // Definir os pontos inicial e final da linha
                    positions[0] = firePoint.position;
                    positions[1] = preyPosition;

                }
            }
        }

        // Atualizar os pontos do Line Renderer
        line.positionCount = 2;
        line.SetPositions(positions);

        // Atualizar o Edge Collider
        UpdateEdgeCollider(positions);
    }

    int RandomPrey()
    {
        List<int> availableIndexes = new List<int>();

        for (int i = 0; i < prey.Count; i++)
        {
            if (prey[i].GetComponent<SpriteRenderer>().enabled)
            {
                // Verifica se a presa está dentro do intervalo de distância permitido
                float distance = Vector2.Distance(transform.position, prey[i].transform.position);
                if (distance <= range)
                {
                    availableIndexes.Add(i);
                }
            }
        }

        if (availableIndexes.Count == 0)
        {
            // Retorna -1 se nenhum objeto com SpriteRenderer ativado estiver dentro do intervalo de distância permitido
            return -1;
        }

        int randomIndex = Random.Range(0, availableIndexes.Count);
        return availableIndexes[randomIndex];
    }



    // Atualiza o Edge Collider com base nos pontos do Line Renderer
    private void UpdateEdgeCollider(Vector3[] positions)
    {
        // Converte os pontos do Line Renderer em pontos do Edge Collider
        Vector2[] edgePoints = new Vector2[positions.Length];
        for (int i = 0; i < positions.Length; i++)
        {
            // Subtrai a posição do firePoint para obter posições relativas ao objeto
            edgePoints[i] = new Vector2(positions[i].x - firePoint.position.x, positions[i].y - firePoint.position.y);
        }

        // Define os pontos do Edge Collider
        edge.points = edgePoints;
    }

    public void FinishAttack() // acionado por evento do animator
    {
        // Define o número de pontos como zero para limpar a linha
        line.positionCount = 0;

        // Limpa o Edge Collider definindo todos os pontos como (0, 0)
        int pointCount = edge.pointCount;
        Vector2[] emptyPoints = new Vector2[pointCount];
        for (int i = 0; i < pointCount; i++)
        {
            emptyPoints[i] = Vector2.zero;
        }
        edge.points = emptyPoints;
    }

    private void ValidatePrey()
    {
        for (int i = prey.Count - 1; i >= 0; i--)
        {
            if (prey[i] == null)
            {
                prey.RemoveAt(i);
            }
        }
    }

    public void finishAnimAttack()
    {
        indexPrey = -1;

        generics.canMove = true;

        // terminar ataque
        isAttack = false;
    }

    public void Movement() // acionado por evento do animator
    {
        // Determina a direção do movimento com base na posição atual em relação ao alvo
        float direction = targetMove.x > transform.position.x ? 1 : -1;

        // Cria um vetor de movimento com base na velocidade de movimento e direção horizontal
        Vector2 moveForce = new Vector2(movementSpeed * direction, body.velocity.y);

        // Aplica a força ao corpo rígido para realizar o movimento horizontal
        body.velocity = moveForce;

        if (isGround) // Verifica se o personagem está no chão
        {
            float temporaryJumpVelocity = isWall ? extrajumpVelocity : jumpVelocity;

            // Aplica uma força de impulso vertical para realizar o pulo
            Vector2 jumpForce = new Vector2(0f, temporaryJumpVelocity);
            body.AddForce(jumpForce, ForceMode2D.Impulse);
        }
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
    private void OnDrawGizmosSelected()
    {
        // Desenha uma esfera representando o range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * GetComponent<Renderer>().bounds.extents.y, range);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * GetComponent<Renderer>().bounds.extents.y, preyRange);
    }
/*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o objeto colidido tem a tag "Predator"
        if (collision.gameObject.CompareTag(generics.LaunchedTag))
        {
            StartCoroutine(Damage());
        }
    }

    IEnumerator Damage()
    {
        generics.SetSpriteMaterial(sprite, blink);
        yield return new WaitForSeconds(0.5f);
        generics.SetSpriteMaterial(sprite, normal);
        yield return new WaitForSeconds(0.1f);
        generics.DestroyObject(gameObject);
        yield return null;
    }
*/
}
