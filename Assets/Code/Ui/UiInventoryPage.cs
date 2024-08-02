using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiInventoryPage : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private UiInventoryItem itemPrefab;
    [SerializeField] private RectTransform contentPanelInventory;
    [SerializeField] private RectTransform contentPanelBar;

    List<UiInventoryItem> listOfUiItens = new List<UiInventoryItem>();
    public GameObject inventoryPagePrefab;


    [Header("Description")]
    [SerializeField] UiInventoryDescription itemDescription;

    [Header("Mouse Follower")]
    [SerializeField] MouseFollower mouseFollower;

    private void Awake() 
    {
        Hide();
        mouseFollower.Toggle(false);
        itemDescription.ResetDescription();
    }

    private int currentlyDraggedItemIndex = -1;

    public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;

    public event Action<int, int> OnSwapItems;

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

            uiItem.OnItemClick += HandleItemSelection;
            uiItem.OnItemBegginDrag += HandleBeginDrag;
            uiItem.OnItemDroppedOn += HandleSwag;
            uiItem.OnItemEndedDrag += HandleEndDrag;
            uiItem.OnRightClick += HandleRightClick;
        }
    }

    public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
    {
        if(listOfUiItens.Count >= itemIndex)
        {
            listOfUiItens[itemIndex].SetData(itemImage, itemQuantity);
        }
    }

    private void HandleRightClick(UiInventoryItem inventoryItemUI)
    {
        throw new NotImplementedException();
    }

    private void HandleEndDrag(UiInventoryItem inventoryItemUI)
    {
        ResetDraggtedItem();
    }

    private void HandleSwag(UiInventoryItem inventoryItemUI)
    {
        int index = listOfUiItens.IndexOf(inventoryItemUI);
        if (index == -1)
        {
            return;
        }

       OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);

    }

    private void ResetDraggtedItem()
    {
        mouseFollower.Toggle(false);
        currentlyDraggedItemIndex = -1;
    }

    private void HandleBeginDrag(UiInventoryItem inventoryItemUI)
    {
        int index =  listOfUiItens.IndexOf(inventoryItemUI);
        if  (index == -1)
        {
            return;
        }

        currentlyDraggedItemIndex = index;
        HandleItemSelection(inventoryItemUI);
        OnStartDragging?.Invoke(index);
    }

    public void CreateDraggedItem(Sprite sprite, int quantity)
    {
        mouseFollower.Toggle(true);
        mouseFollower.SetData(sprite, quantity);
    }

    private void HandleItemSelection(UiInventoryItem inventoryItemUI)
    {
       int index =  listOfUiItens.IndexOf(inventoryItemUI);
       if(index == -1)
       {
        return ;
       }

       OnDescriptionRequested?.Invoke(index);
    }

    // lista os itens no inventario
    public void Show()
    {
        inventoryPagePrefab.SetActive(true);
        ResetSelection();
    }

    private void ResetSelection()
    {
        itemDescription.ResetDescription();
        DeselectAllItems();
    }

    private void DeselectAllItems()
    {
        foreach(UiInventoryItem item in listOfUiItens)
        {
            item.Deselect();
        }
    }

    public void Hide()
    {
        inventoryPagePrefab.SetActive(false);
        ResetDraggtedItem();
    }
}