using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private UiInventoryPage inventoryUi;
    [SerializeField] private InventorySO inventoryData;
    [SerializeField] private PlayerData_Input input;

    private void Start()
    {
        PrepareUi();
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

    }

    private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
    {

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
