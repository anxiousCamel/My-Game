using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source Settings")]
    [ReadOnly] public AudioSource audioSource;  // Fonte de áudio para reproduzir os sons
    private float defaultPitch;  // Pitch padrão do áudio

    /// <summary>
    /// Método chamado ao inicializar o script. 
    /// Obtém o AudioSource e salva o pitch padrão.
    /// </summary>
    private void Awake()
    {
        // Se o AudioSource não estiver atribuído, busca o componente na GameObject atual
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Salva o pitch padrão do AudioSource
        if (audioSource != null)
        {
            defaultPitch = audioSource.pitch;
        }
        else
        {
            Debug.LogWarning("AudioSource não encontrado!");  // Emite um aviso se não encontrar o AudioSource
        }
    }

    /// <summary>
    /// Reproduz o som com o pitch padrão.
    /// </summary>
    /// <param name="sound">O clip de áudio a ser reproduzido.</param>
    public void PlaySound(AudioClip sound)
    {
        PlaySoundWithPitch(sound, defaultPitch);
    }

    /// <summary>
    /// Reproduz o som com um pitch aleatório.
    /// </summary>
    /// <param name="sound">O clip de áudio a ser reproduzido.</param>
    public void PlaySoundRandomPitch(AudioClip sound)
    {
        float randomPitch = Random.Range(0.5f, 1f);  // Gera um pitch aleatório entre 0.5 e 1
        PlaySoundWithPitch(sound, randomPitch);
    }

    /// <summary>
    /// Método auxiliar para reproduzir som com um pitch específico.
    /// </summary>
    /// <param name="sound">O clip de áudio a ser reproduzido.</param>
    /// <param name="pitch">O valor do pitch a ser utilizado.</param>
    private void PlaySoundWithPitch(AudioClip sound, float pitch)
    {
        if (audioSource != null && audioSource.enabled && sound != null)
        {
            audioSource.pitch = pitch;
            audioSource.clip = sound;
            audioSource.Play();
        }
        else if (sound == null)
        {
            Debug.LogWarning("Nenhum AudioClip fornecido para reprodução!");
        }
    }

    /// <summary>
    /// Atualiza o estado do AudioSource a cada quadro.
    /// Se o áudio não estiver sendo reproduzido, reseta o clip para null.
    /// </summary>
    private void Update()
    {
        if (audioSource != null && audioSource.enabled && !audioSource.isPlaying)
        {
            audioSource.clip = null;
        }
    }
}
