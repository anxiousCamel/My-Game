using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTrail : MonoBehaviour
{
    public GameObject ghostPrefab;  // Um prefab que será o fantasma
    public float ghostDelay = 0.1f; // Intervalo entre a criação dos fantasmas
    public float ghostLifetime = 0.5f; // Quanto tempo o fantasma dura

    private float timeSinceLastGhost = 0;
    private SpriteRenderer mainSpriteRenderer; // Referência ao SpriteRenderer do objeto principal

    private void Start()
    {
        // Obtenha o SpriteRenderer do objeto principal
        mainSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Verifica se o SpriteRenderer do objeto principal está ativo
        if (mainSpriteRenderer != null && mainSpriteRenderer.enabled)
        {
            // Cria o "fantasma" a cada 'ghostDelay' segundos
            if (timeSinceLastGhost >= ghostDelay)
            {
                CreateGhost();
                timeSinceLastGhost = 0;
            }
            else
            {
                timeSinceLastGhost += Time.deltaTime;
            }
        }
    }

    void CreateGhost()
    {
        // Instancia um clone do objeto original
        GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        
        // Ajusta a cor do clone para ser mais transparente
        SpriteRenderer ghostRenderer = ghost.GetComponent<SpriteRenderer>();
        ghostRenderer.sprite = GetComponent<SpriteRenderer>().sprite; // Copia o sprite atual

        StartCoroutine(FadeOut(ghostRenderer));
        
        // Destrói o "fantasma" após 'ghostLifetime' segundos
        Destroy(ghost, ghostLifetime);
    }

    IEnumerator FadeOut(SpriteRenderer spriteRenderer)
    {
        // Controla o fade-out (desvanecimento) do fantasma
        Color color = spriteRenderer.color;
        float fadeTime = ghostLifetime;

        while (fadeTime > 0)
        {
            // Verifica se o objeto ou o SpriteRenderer ainda existe
            if (spriteRenderer == null || !spriteRenderer.gameObject.activeInHierarchy)
            {
                yield break; // Para a coroutine se o objeto foi destruído ou desativado
            }

            fadeTime -= Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, fadeTime / ghostLifetime);
            spriteRenderer.color = color;  // Ajusta o alfa (transparência)
            yield return null;
        }
    }
}
