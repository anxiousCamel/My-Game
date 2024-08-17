using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(menuName = "Inventory/Item")]
    public class ItemSO : ScriptableObject
    {
        [SerializeField] private bool isStackable;
        
        public int ID => GetInstanceID();

        
        [Header("Inventory Settings")]
        [SerializeField] private Sprite itemImage;
        [SerializeField] private int maxStackSize = 1;
        [SerializeField] private string itemName;
        [SerializeField] private string type;
        [SerializeField] [TextArea] private string description;

        [Space(20)]
        
        [Header("Drop Settings")]
        [SerializeField] private GameObject prefab;
        [SerializeField] private int minQuantityDrop = 1;
        [SerializeField] private int maxQuantityDrop;
        [SerializeField] private float probabilityDrop = 100;

        // Propriedades
        public bool IsStackable => isStackable;
        public Sprite ItemImage => itemImage;
        public int MaxStackSize => maxStackSize;
        public string Name => itemName;
        public string Type => type;
        public string Description => description;
        
        public GameObject Prefab => prefab;
        public int MinQuantityDrop => minQuantityDrop;
        public int MaxQuantityDrop => maxQuantityDrop;
        public float ProbabilityDrop => probabilityDrop;
    }
}
