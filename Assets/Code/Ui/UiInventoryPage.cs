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

    [Header("Temporary Item")]
    public Sprite image;
    public int quantity;
    public string title, type, description;



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
        mouseFollower.Toggle(false);
    }

    private void HandleSwag(UiInventoryItem obj)
    {
        throw new NotImplementedException();
    }

    private void HandleBeginDrag(UiInventoryItem obj)
    {
        mouseFollower.Toggle(true);
        mouseFollower.SetData(image,quantity);
    }

    private void HandleItemSelection(UiInventoryItem obj)
    {
        itemDescription.SetDescription(title, type, description);
        listOfUiItens[0].Select();
    }

    public void Show()
    {
        inventoryPagePrefab.SetActive(true);
        itemDescription.ResetDescription();

        listOfUiItens[0].SetData(image,quantity);
    }

    public void Hide()
    {
        inventoryPagePrefab.SetActive(false);
    }
}
