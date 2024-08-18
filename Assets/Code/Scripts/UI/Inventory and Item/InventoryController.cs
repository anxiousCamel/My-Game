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

            for (int i = 0; i < initialItems.Count; i++)
            {
                InventoryItem item = initialItems[i];

                if (item.isEmpty)
                {
                    // Adiciona um espaço vazio para manter a posição
                    inventoryData.SetItemAt(i, InventoryItem.GetEmptyItem());
                }
                else
                {
                    // Adiciona o item na posição correspondente, sem empilhar
                    inventoryData.SetItemAt(i, item);
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

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            InventoryItem item1 = inventoryData.GetItemAt(itemIndex_1);
            InventoryItem item2 = inventoryData.GetItemAt(itemIndex_2);

            // Verifica se os itens são do mesmo tipo e são empilháveis
            if (item1.item.ID == item2.item.ID && item1.item.IsStackable)
            {
                // Tenta combinar os itens
                inventoryData.CombineItems(itemIndex_1, itemIndex_2, item1, item2);
            }
            else
            {
                // Se não puderem ser combinados, troca os itens normalmente
                inventoryData.SwapItems(itemIndex_1, itemIndex_2);
            }
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.isEmpty)
            {
                return;
            }
            else
            {
                inventoryUi.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
            }
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
