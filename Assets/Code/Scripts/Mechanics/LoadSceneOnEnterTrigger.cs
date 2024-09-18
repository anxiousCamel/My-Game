using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnEnterTrigger : MonoBehaviour
{
    public Scenes sceneToLoad;
    public LayerMask playerLayer;
    private BoxCollider2D col;

    void Start()
    {
        col = GetComponent<BoxCollider2D>();

        // Pré-carregar a cena no início, se ainda não tiver sido pré-carregada
        if (!ScenePreloadManager.Instance.IsScenePreloaded(sceneToLoad))
        {
            ScenePreloadManager.Instance.PreloadScene(sceneToLoad);
        }

        // Obtém o nome ou o índice da cena atual
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Converte o nome da cena para o tipo Scenes
        Scenes currentScene;
        if (Enum.TryParse(currentSceneName, out currentScene))
        {
            // Usa a instância singleton para chamar o método
            bool isActivated = ScenePreloadManager.Instance.IsSceneActivated(currentScene);
            Debug.Log("Cena atual ativada: " + isActivated);
        }
        else
        {
            Debug.LogWarning("A cena atual não corresponde a nenhum valor definido na enumeração Scenes.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPlayerLayer(other.gameObject))
        {
            Debug.Log("Jogador entrou no trigger. Ativando cena: " + sceneToLoad);
            if (!ScenePreloadManager.Instance.IsSceneActivated(sceneToLoad))
            {
                ScenePreloadManager.Instance.ActivateScene(sceneToLoad);
            }
        }
    }

    bool IsPlayerLayer(GameObject obj)
    {
        return ((1 << obj.layer) & playerLayer) != 0;
    }
}
