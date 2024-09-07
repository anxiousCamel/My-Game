using System.Collections;
using UnityEngine;

public class ObjectLaunched : MonoBehaviour
{
    [Range(0,5)]public float durationParticleTime = 1.0f;
    public ParticleSystem particle;
    public SpriteRenderer sprite;
    public BoxCollider2D boxCollider;
    public CircleCollider2D circleCollider;
    public AudioClip destructionAudio;
    public LayerMask Collider;
    [ReadOnly] public GameObject player;
    [ReadOnly] public PlayerData_Collider playerCollider;
    [ReadOnly] public PlayerData_Mechanics playerMechanics;
    [ReadOnly] public AudioManager audioManager;

    [Header("ShakeCamera")]
    [ReadOnly] public CameraShake cameraShake;
    public float shakeDuration;
    public float amplitude;

    // Método de inicialização
    private void Awake()
    {
        // Encontra o jogador na cena e obtém suas referências de collider e mechanics
        player = GameObject.FindGameObjectWithTag("Player");
        playerCollider = player.GetComponent<PlayerData_Collider>();
        playerMechanics = player.GetComponent<PlayerData_Mechanics>();
        audioManager = GetComponent<AudioManager>();

        GetCamera();
    }

    private void GetCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            cameraShake = mainCamera.GetComponentInChildren<CameraShake>();
        }

        // Certifique-se de que a referência foi encontrada
        if (cameraShake == null)
        {
            Debug.LogError("CameraShake script not found in camera's children.");
        }
    }

    // Método de atualização
    private void Update()
    {
        // Verifica colisões em todas as direções
        CheckCollision(Vector2.up);
        CheckCollision(Vector2.down);
        CheckCollision(Vector2.left);
        CheckCollision(Vector2.right);
    }

    // Método para verificar colisões em uma direção específica
    private void CheckCollision(Vector2 direction)
    {
        // Define o ponto de origem do raio no centro do collider do objeto
        Vector2 origin = GetComponent<BoxCollider2D>().bounds.center;
        Vector2 size = GetComponent<BoxCollider2D>().bounds.size;

        // Define o tamanho do raio na direção especificada
        float distance = 0.1f;

        // Dispara um raio na direção especificada a partir do ponto de origem
        RaycastHit2D hit = Physics2D.BoxCast(origin, size, 0, direction, distance, Collider);

        // Verifica se houve colisão
        if (hit.collider != null)
        {
            CreateParticlesAndDestroy();
        }
    }

    // Método para destruir o objeto lançado
    public void CreateParticlesAndDestroy()
    {
        StartCoroutine(Destroy());

    }

    public IEnumerator Destroy()
    {
        particle.textureSheetAnimation.SetSprite(0,sprite.sprite);
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        body.velocity = Vector2.zero;
        body.gravityScale = 0;

        cameraShake.ShakeCamera(amplitude,shakeDuration);

        circleCollider.enabled = true;
        yield return new WaitForSeconds(0.01f);
        circleCollider.enabled = false;

        audioManager.PlaySound(destructionAudio);
        particle.Play();
        sprite.enabled = false;
        boxCollider.enabled = false;

        yield return new WaitForSeconds(durationParticleTime);

        Destroy(gameObject);
    }

}
