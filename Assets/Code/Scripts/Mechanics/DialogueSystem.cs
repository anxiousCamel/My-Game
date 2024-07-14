using TMPro;
using UnityEngine;
using System.Collections;

public class DialogueSystem : MonoBehaviour
{
    public TextMeshPro dialogText;

    private float fadeInStartTime;
    private float fadeInDuration;

    // Função para exibir um diálogo genérico com efeito fadeIn
    public void ShowDialog(string text, float fadeInTime = 1f)
    {
        // Configura o início do fadeIn e a duração
        fadeInStartTime = Time.time;
        fadeInDuration = fadeInTime;

        // Configura o texto inicial
        Color startColor = dialogText.color;
        dialogText.text = text;
        dialogText.color = new Color(startColor.r, startColor.g, startColor.b, 0f);

        // Ativa o método Update para realizar o fadeIn
        enabled = true;
    }

    private void Update()
    {
        // Calcula o progresso do fadeIn
        float elapsedTime = Time.time - fadeInStartTime;
        float alpha = Mathf.Clamp01(elapsedTime / fadeInDuration);

        // Atualiza a cor do texto
        Color textColor = dialogText.color;
        dialogText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);

        // Desativa o método Update quando o fadeIn estiver concluído
        if (alpha >= 1f)
        {
            enabled = false;
        }
    }
}