using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



namespace Inventory.Model
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject
    {
        [SerializeField] private List<InventoryItem> InventoryItems;
        [field: SerializeField] public int Size { get; set; }

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;

        public void Initialize()
        {
            InventoryItems = new List<InventoryItem>();
            for (int i = 0; i < Size; i++)
            {
                InventoryItems.Add(InventoryItem.GetEmptyItem());
            }
        }

        public int AddItem(ItemSO item, int quantity)
        {
            if (item.IsStackable == false)
            {
                for (int i = 0; i < InventoryItems.Count; i++)
                {
                    while (quantity > 0 && IsInventoryFull() == false)
                    {
                        quantity -= AddItemToFristFreeSlot(item, 1);
                    }
                    InformAboutChange();
                    return quantity;
                }
            }

            quantity = addStackableItem(item, quantity);
            InformAboutChange();
            return quantity;
        }

        private int AddItemToFristFreeSlot(ItemSO item, int quantity)
        {
            InventoryItem newItem = new InventoryItem
            {
                item = item,
                quantity = quantity,
            };

            for (int i = 0; i < InventoryItems.Count; i++)
            {

                if (InventoryItems[i].isEmpty)
                {
                    InventoryItems[i] = newItem;
                    return quantity;
                };
            }

            return 0;
        }

        private bool IsInventoryFull()
        {
            return InventoryItems.Where(item => item.isEmpty).Any() == false;
        }

        private int addStackableItem(ItemSO item, int quantity)
        {
           for (int i = 0; i < InventoryItems.Count; i++)
           {
                if(InventoryItems[i].isEmpty)
                {
                    continue;
                }

                if(InventoryItems[i].item.ID == item.ID)
                {
                    int amountPossibleToTake = 
                    InventoryItems[i].item.MaxStackSize - InventoryItems[i].quantity;

                    if(quantity > amountPossibleToTake)
                    {
                        InventoryItems[i] = InventoryItems[i].
                        ChangeQuantity(InventoryItems[i].item.MaxStackSize);

                        quantity -= amountPossibleToTake;
                    }
                    else
                    {
                        InventoryItems[i] = InventoryItems[i].
                        ChangeQuantity(InventoryItems[i].quantity + quantity);
                        InformAboutChange();
                        return 0;
                    }
                }
           }

           while(quantity > 0 && IsInventoryFull() == false)
           {
                int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
                quantity -= newQuantity;
                AddItemToFristFreeSlot(item, newQuantity);
           }

           return quantity;
        }

        public Dictionary<int, InventoryItem> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();

            for (int i = 0; i < InventoryItems.Count; i++)
            {
                if (InventoryItems[i].isEmpty)
                {
                    continue;
                }

                returnValue[i] = InventoryItems[i];
            }

            return returnValue;
        }

        public InventoryItem GetItemAt(int itemIndex)
        {
            return InventoryItems[itemIndex];
        }

        public void AddItem(InventoryItem item)
        {
            AddItem(item.item, item.quantity);
        }

        public void SwapItems(int itemIndex_1, int itemIndex_2)
        {
            InventoryItem item1 = InventoryItems[itemIndex_1];
            InventoryItems[itemIndex_1] = InventoryItems[itemIndex_2];
            InventoryItems[itemIndex_2] = item1;
            InformAboutChange();
        }

        private void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
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
}