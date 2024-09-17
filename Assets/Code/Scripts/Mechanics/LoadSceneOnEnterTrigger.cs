using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnEnterTrigger : MonoBehaviour
{
    [ReadOnly] public BoxCollider2D col;
    public Scenes sceneToLoad;
    public LayerMask playerLayer; // Substituindo por LayerMask
    [ReadOnly] public bool playerInside = false;

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    // Verifica se o objeto que entrou pertence à camada do Player
    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPlayerLayer(other.gameObject))
        {
            playerInside = true;
            LoadScene();
        }
    }

    // Se o player já estiver dentro do trigger, mantém o playerInside como true
    void OnTriggerStay2D(Collider2D other)
    {
        if (IsPlayerLayer(other.gameObject))
        {
            playerInside = true;
        }
    }

    // Se o player sair da área do trigger
    void OnTriggerExit2D(Collider2D other)
    {
        if (IsPlayerLayer(other.gameObject))
        {
            playerInside = false;
        }
    }

    // Função para verificar se o objeto está na layer do Player
    bool IsPlayerLayer(GameObject obj)
    {
        // Verifica se a layer do objeto está dentro do LayerMask
        return ((1 << obj.layer) & playerLayer) != 0;
    }

    // Carrega a cena correspondente
    void LoadScene()
    {
        if (playerInside)
        {
            SceneManager.LoadScene((int)sceneToLoad);
            playerInside = false;
        }
    }
}
