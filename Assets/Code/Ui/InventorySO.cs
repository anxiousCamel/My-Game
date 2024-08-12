using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class InventorySO : ScriptableObject
{
    [SerializeField] private List<InventoryItem> InventoryItems;
    [field: SerializeField] public int Size { get; set; }

    public void Initialize()
    {
        InventoryItems = new List<InventoryItem>();
        for (int i = 0; i < Size; i++)
        {
            InventoryItems.Add(InventoryItem.GetEmptyItem());
        }
    }

    public void AddItem(ItemSO item, int quantity)
    {
        for (int i = 0; i < InventoryItems.Count; i++)
        {
            if (InventoryItems[i].isEmpty)
            {
                InventoryItems[i] = new InventoryItem
                {
                    item = item,
                    quantity = quantity
                };
            }
        }
    }

    public Dictionary<int, InventoryItem> GetCurrentInventoryState()
    {
        Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();
        
        for (int i =  0; i < InventoryItems.Count; i++)
        {
            if(InventoryItems[i].isEmpty)
            {
                continue;
            }

            returnValue[i] = InventoryItems[i];
        }

        return returnValue;
    }

    public InventoryItem GetItemAt(int itemIndex)
    {
        return  InventoryItems[itemIndex];
    }
}

[Serializable]
public struct InventoryItem
{
    public ItemSO item;
    public int quantity;
    public bool isEmpty => item == null;

    public InventoryItem ChangeQuantity(int newQuantity)
    {
        return new InventoryItem
        {
            item = this.item,
            quantity = newQuantity
        };
    }

    public static InventoryItem GetEmptyItem() => new InventoryItem
    {
        item = null,
        quantity = 0
    };
}