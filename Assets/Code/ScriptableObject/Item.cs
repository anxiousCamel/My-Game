using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("Drop")]
    [Range(1, 6)]
    public int MinQuantityDrop;

    [Range(1, 6)]
    public int MaxQuantityDrop;

    [Range(0, 100)]
    public float Probability;

    [Header("Item")]
    public string itemName;
    public Sprite icon;
    public GameObject itemPrefab;
    public int sellValue;
    public bool stackable;
}