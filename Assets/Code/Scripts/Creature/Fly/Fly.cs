using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Fly : MonoBehaviour
{
    [Header("Camadas")]
    [ReadOnly] public String predatorTag = "Predator";

    [Header("Renderer")]
    public float fadeInDuration;
    [ReadOnly] public Color corInicial;

    [Header("Life")]
    public bool enabledObject;
    public int respawm;

    [Header("Audio")]
    [ReadOnly] AudioClip defaultClip;

    [Header("Componetes")]
    [ReadOnly] public ParticleSystem particle;
    [ReadOnly] public Rigidbody2D body;
    [ReadOnly] public BoxCollider2D col;
    [ReadOnly] public SpriteRenderer sprite;
    [ReadOnly] public EnemyGenerics generics;
    [ReadOnly] public AudioManager audioManager;
    [ReadOnly] public AudioSource audioSource;
    public GameObject player;

    [Header("Movement")]
    [ReadOnly] Vector2 moveDirection;
    public float speed;
    public float range;

    [Header("Delimitadores")]
    private Vector2 target;
    [Range(0, 2)] public float tolerance;
    public BoxCollider2D colliderArea;


    void Start()
    {
        enabled = true;
        generics = GetComponent<EnemyGenerics>();
        audioManager = GetComponent<AudioManager>();
        body = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        particle = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = generics.RandomPitch();
        defaultClip = audioSource.clip;

        corInicial = sprite.color = generics.RandomColor();
        SetRandomTargetPosition();

        if (player == null)
        {
            player = generics.GetPlayer();
        }

    }

    void Update()
    {
        if (enabledObject)
        {
            // Verifica se o inimigo chegou ao destino
            if (Vector2.Distance(transform.position, target) <= tolerance)
            {
                // Define uma nova posição aleatória como destino
                SetRandomTargetPosition();
            }

            if (generics.InRange(transform.position, player.transform.position, range))
            {
                // seguir jogador
                moveDirection = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
                body.velocity = moveDirection * speed;
            }
            else
            {
                // Move o inimigo em direção ao destino
                moveDirection = (target - (Vector2)transform.position).normalized;
                body.velocity = moveDirection * speed;
            }
        }

        else
        {
            // parar movimento
            body.velocity = Vector2.zero;
        }
    }

    void SetRandomTargetPosition()
    {
        target = generics.GetRandomPosition(colliderArea);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o objeto colidido tem a tag "Predator"
        if (other.CompareTag(predatorTag))
        {
            // Inicia particula
            particle.Play();

            // Desativa o sprite e o collider
            Disable();

            // Chama o método para reativar o sprite e o collider após o tempo especificado
            Invoke(nameof(Activate), respawm);
        }
    }

    private void Disable() // Desliga Objeto
    {
        enabledObject = false;
        sprite.enabled = false;
        col.enabled = false;
        audioSource.enabled = false;
    }

    private void Activate()  // Liga objetos Objeto
    {
        audioSource.enabled = true;
        audioManager.PlaySound(defaultClip);
        enabledObject = true;
        sprite.enabled = true;
        col.enabled = true;
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            // Calcula o alpha atual com base no tempo
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);

            // Atualiza a cor do SpriteRenderer
            sprite.color = new Color(corInicial.r, corInicial.g, corInicial.b, alpha);

            // Incrementa o tempo decorrido
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Garante que o alpha seja exatamente 1 no final
        sprite.color = new Color(corInicial.r, corInicial.g, corInicial.b, 1f);
    }
}
