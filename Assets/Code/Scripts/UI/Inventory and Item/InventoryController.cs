using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Model;
using Inventory.UI;
using UnityEngine;
namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private UiInventoryPage inventoryUi;
        [SerializeField] private InventorySO inventoryData;
        [SerializeField] private PlayerData_Input input;

        public List<InventoryItem> initialItems = new List<InventoryItem>();

        private void Start()
        {
            PrepareUi();
            PrepareInventoryData();
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;

            foreach (InventoryItem item in initialItems)
            {
                if(item.isEmpty)
                {
                    continue;
                }

                else
                {
                    inventoryData.AddItem(item);
                }
            }
           
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUi.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUi.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
            }
        }

        private void PrepareUi()
        {
            inventoryUi.InitializeInventoryUi(inventoryData.Size);
            this.inventoryUi.OnDescriptionRequested += HandleDescriptionRequest;
            this.inventoryUi.OnSwapItems += HandleSwapItems;
            this.inventoryUi.OnStartDragging += HandleDragging;
            this.inventoryUi.OnItemActionRequested += HandleItemActionRequested;
        }

        private void HandleItemActionRequested(int itemIndex)
        {

        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if(inventoryItem.isEmpty)
            {
                return;
            }
            else
            {
                inventoryUi.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
            }
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            inventoryData.SwapItems(itemIndex_1, itemIndex_2);
           
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.isEmpty)
            {
                inventoryUi.ResetSelection();
                return;
            }


            ItemSO item = inventoryItem.item;
            inventoryUi.UpdateDescription(itemIndex, item.Name, item.Type, item.Description);

        }

        private void Update()
        {

            if (input.CheckInput.inputInventory)
            {
                if (inventoryUi.inventoryPagePrefab.activeSelf)
                {
                    inventoryUi.Hide();
                }
                else
                {
                    inventoryUi.Show();
                    foreach (var item in inventoryData.GetCurrentInventoryState())
                    {
                        inventoryUi.UpdateData(
                            item.Key,
                            item.Value.item.ItemImage,
                            item.Value.quantity
                        );
                    }
                }
            }
        }
    }
}
