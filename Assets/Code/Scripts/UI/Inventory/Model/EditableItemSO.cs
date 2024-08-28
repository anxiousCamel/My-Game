using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Model;
using UnityEngine;
namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EditableItemSO : ItemSO, IDestroyItem, IDropItem, IItemAction
    {
        [Space(20)] [SerializeField] private List<ModifierData> modifiersData =  new List<ModifierData>();
        public string ActionName => "";

        public AudioClip actionSFX {get; private set;}

        public bool PerformAction(GameObject character)
        {
            foreach (ModifierData data in modifiersData)
            {
                data.statsModifier.AffectCharacter(character, data.value);
            }
            return true;
        }
    }
    public interface IDestroyItem
    {

    }
    public interface IDropItem
    {

    }
    public interface IItemAction
    {
        public string ActionName { get; }
        public AudioClip actionSFX { get; }
        bool PerformAction(GameObject character);
    }
    [Serializable] public class ModifierData
    {
        public CharacterStatsModifierSO statsModifier;
        public float value;
    }
}
