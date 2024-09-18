using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePreloadManager : MonoBehaviour
{
    private Dictionary<Scenes, AsyncOperation> preloadedScenes = new Dictionary<Scenes, AsyncOperation>();
    private Dictionary<Scenes, bool> scenesActivated = new Dictionary<Scenes, bool>();

    public static ScenePreloadManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize the scene that is already loaded at startup
        InitializeCurrentScene();
    }

    private void InitializeCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        Scenes sceneEnum;
        if (Enum.TryParse(currentScene.name, out sceneEnum))
        {
            // Ensure the current scene is marked as activated
            if (!scenesActivated.ContainsKey(sceneEnum))
            {
                scenesActivated[sceneEnum] = true;
            }
        }
    }

    public void PreloadScene(Scenes scene)
    {
        // Verifica se a cena já está carregada ou pré-carregada
        if (SceneManager.GetSceneByBuildIndex((int)scene).isLoaded || preloadedScenes.ContainsKey(scene))
        {
            Debug.Log("Cena já está carregada ou pré-carregada: " + scene);
            return;
        }

        Debug.Log("Pré-carregando cena: " + scene);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;
        preloadedScenes.Add(scene, asyncOperation);
        scenesActivated.Add(scene, false);
    }

    public void ActivateScene(Scenes scene)
    {
        if (preloadedScenes.ContainsKey(scene))
        {
            if (!scenesActivated[scene])
            {
                AsyncOperation asyncOperation = preloadedScenes[scene];
                if (asyncOperation.progress >= 0.9f)
                {
                    Debug.Log("Ativando cena: " + scene);
                    asyncOperation.allowSceneActivation = true;
                    scenesActivated[scene] = true;
                }
                else
                {
                    Debug.LogWarning("A cena não está pronta para ativação: " + scene + " (Progresso: " + asyncOperation.progress + ")");
                }
            }
            else
            {
                Debug.LogWarning("A cena já está ativada: " + scene);
            }
        }
        else
        {
            Debug.LogWarning("A cena não foi pré-carregada: " + scene);
        }
    }

    public bool IsScenePreloaded(Scenes scene)
    {
        return preloadedScenes.ContainsKey(scene);
    }

    public bool IsSceneActivated(Scenes scene)
    {
        return scenesActivated.ContainsKey(scene) && scenesActivated[scene];
    }

    // Expose dictionaries for the custom editor
    public Dictionary<Scenes, AsyncOperation> GetPreloadedScenes()
    {
        return preloadedScenes;
    }

    public Dictionary<Scenes, bool> GetActivatedScenes()
    {
        return scenesActivated;
    }
}
