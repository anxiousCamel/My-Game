using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiInventoryPage : MonoBehaviour
{
    [SerializeField] private UiInventoryItem itemPrefab;
    [SerializeField] private RectTransform contentPanelInventory;
    [SerializeField] private RectTransform contentPanelBar;

    List<UiInventoryItem> listOfUiItens = new List<UiInventoryItem>();
    public GameObject inventoryPagePrefab;


    public void InitializeInventoryUi(int inventorySize, int barSize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            UiInventoryItem uiItem =  Instantiate(itemPrefab, Vector3.zero, Quaternion.identity, contentPanelInventory);
            uiItem.transform.SetParent(contentPanelInventory);
            listOfUiItens.Add(uiItem);

            uiItem.OnItemClick += HandleItemSelection;
            uiItem.OnItemBegginDrag += HandleBeginDrag;
            uiItem.OnItemDroppedOn += HandleSwag;
            uiItem.OnItemEndedDrag += HandleEndDrag;
            uiItem.OnRightClick += HandleRightClick;
        }

        for (int i = 0; i < barSize; i++)
        {
            UiInventoryItem uiItem =  Instantiate(itemPrefab, Vector3.zero, Quaternion.identity, contentPanelBar);
            uiItem.transform.SetParent(contentPanelBar);
            listOfUiItens.Add(uiItem);
        }
    }

    private void HandleRightClick(UiInventoryItem obj)
    {
        throw new NotImplementedException();
    }

    private void HandleEndDrag(UiInventoryItem obj)
    {
        throw new NotImplementedException();
    }

    private void HandleSwag(UiInventoryItem obj)
    {
        throw new NotImplementedException();
    }

    private void HandleBeginDrag(UiInventoryItem obj)
    {
        throw new NotImplementedException();
    }

    private void HandleItemSelection(UiInventoryItem obj)
    {
        Debug.Log(obj.name);
    }

    public void Show()
    {
        inventoryPagePrefab.SetActive(true);
    }

    public void Hide()
    {
        inventoryPagePrefab.SetActive(false);
    }
}
