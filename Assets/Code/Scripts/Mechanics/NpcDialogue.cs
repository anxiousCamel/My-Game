using System.Collections;
using TMPro;
using UnityEngine;

public class NpcDialogue : MonoBehaviour
{
    public NpcDialogueSO npcDialogueSO;
    public DialogueSystem dialogueSystem;

    void Start()
    {
        // substituir para uma logica
        DisplayDialogue(0);
    }

    private void DisplayDialogue(int dialogueIndexToShow)
    {
        // Verifica se o índice é válido antes de exibir o diálogo
        if (IsValidIndex(dialogueIndexToShow))
        {
            // Obtém o diálogo atual e exibe usando o DialogueSystem
            NpcDialogueSO.Dialogue currentDialogue = npcDialogueSO.Dialogues[dialogueIndexToShow];
            ShowDialog(currentDialogue.Message);
        }
    }

    private bool IsValidIndex(int index)
    {
        // Verifica se o índice está dentro dos limites aceitáveis
        if (npcDialogueSO != null && index >= 0 && index < npcDialogueSO.Dialogues.Count)
        {
            return true;
        }
        else
        {
            // Exibe uma mensagem de erro se o índice não for válido
            Debug.LogError("NpcDialogueSO não atribuído ou dialogueIndexToShow fora do range.");
            return false;
        }
    }

    private void ShowDialog(string text)
    {
        // Exibe o diálogo usando o DialogueSystem
        if (dialogueSystem != null)
        {
            dialogueSystem.ShowDialog(text);
        }
        else
        {
            // Exibe uma mensagem de aviso se o DialogueSystem não estiver atribuído
            Debug.LogWarning("DialogueSystem não atribuído. Não é possível exibir o diálogo.");
        }
    }
}