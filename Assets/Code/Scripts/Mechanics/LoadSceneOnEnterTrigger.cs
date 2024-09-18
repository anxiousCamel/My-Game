using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
