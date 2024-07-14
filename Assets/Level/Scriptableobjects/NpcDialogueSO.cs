using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNpcDialogue", menuName = "Dialogue/NewNpcDialogue")]
public class NpcDialogueSO : ScriptableObject
{
    [SerializeField] private List<Dialogue> dialogues = new();

    public List<Dialogue> Dialogues => dialogues;

    [Serializable] public class Dialogue
    {
        [SerializeField] private string speaker;

        [TextArea(3, 10)] [SerializeField] private string message;

        public string Speaker => speaker;
        public string Message => message;
    }
}
