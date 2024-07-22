using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    private static int idCounter = 0;
    [ReadOnly] public int id;
    public GameObject itemPrefab;
    public string itemName;
    [PreviewSprite] public Sprite icon;

    
    [Space(10)]
    [TextArea]
    public string description = "Ele esqueceu de colocar";


    [Space(20)]
    public int sellValue;
    public bool stackable;



    [Space(20)]
    [Header("Drop")]
    [Range(1, 6)]
    public int MinQuantityDrop;

    [Range(1, 6)]
    public int MaxQuantityDrop;

    [Range(0.001f, 1)]
    public float Probability;

    private void OnEnable()
    {
        if (id == 0) // Only set the ID if it is not already set
        {
            id = GenerateUniqueID();
        }
    }

    public int GenerateUniqueID()
    {
        return ++idCounter;
    }

    public void Initialize(int uniqueId)
    {
        id = uniqueId;
    }

}