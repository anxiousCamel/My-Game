using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("Item")]
    public string itemName;
    public Sprite icon;
    public GameObject itemPrefab;

    [TextArea]
    public string itemDescription= "Ele esqueceu de colocar";


    [Space(10)]
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

}