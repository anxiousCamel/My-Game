using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    public abstract class ItemSO : ScriptableObject
    {
        public int ID => GetInstanceID();

        [Header("Inventory Settings")]
        [SerializeField] private Sprite itemImage;
        [SerializeField] private int maxStackSize = 1;
        [SerializeField] private string itemName;
        [SerializeField] private string type;
        [SerializeField][TextArea] private string description;

        [Space(20)]

        [Header("Drop Settings")]
        [SerializeField] private GameObject prefabDrop;
        [SerializeField] private GameObject prefabToPlace;
        [SerializeField] private int minQuantityDrop = 1;
        [SerializeField] private int maxQuantityDrop = 1;
        [SerializeField] private float probabilityDrop = 100;

        [Space(20)]

        [Header("Item Properties")]
        [SerializeField] private bool isStackable = true;
        [SerializeField] private bool isConsumable;
        [SerializeField] private bool isSalable = true;
        [SerializeField] private bool isCraftMaterial = true;
        [SerializeField] private bool isPlaceable;
        


        #region Propriedades
        public bool IsStackable => isStackable;
        public bool IsConsumable => isConsumable;
        public bool IsSalable => isSalable;
        public bool IsCraftMaterial => isCraftMaterial;
        public bool IsPlaceable => isPlaceable;

        public Sprite ItemImage => itemImage;
        public int MaxStackSize => maxStackSize;
        public string Name => itemName;
        public string Type => type;
        public string Description => description;

        public GameObject PrefabDrop => prefabDrop;
        public GameObject PrefabToPlace => prefabToPlace;
        public int MinQuantityDrop => minQuantityDrop;
        public int MaxQuantityDrop => maxQuantityDrop;
        public float ProbabilityDrop => probabilityDrop;
        #endregion
    }
}
