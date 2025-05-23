using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UiInventoryPage : MonoBehaviour
    {
        private bool isInventoryOpen = false;
        private int previousSelectedIndex = -1;
        [SerializeField] private UiInventoryDescription description;

        [Header("Inventory")]
        [SerializeField] private UiInventoryItem itemPrefab;
        [SerializeField] private RectTransform contentPanelInventory;
        [SerializeField] private RectTransform contentPanelBar;

        List<UiInventoryItem> listOfUIItems = new List<UiInventoryItem>();
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

        public void InitializeInventoryUi(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {

                UiInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);

                // Usa operador ternário para definir o pai (parent) do item
                uiItem.transform.SetParent(i < 9 ? contentPanelBar : contentPanelInventory);

                listOfUIItems.Add(uiItem);

                // Configura os eventos para o item
                uiItem.OnItemClick += HandleItemSelection;
                uiItem.OnItemBegginDrag += HandleBeginDrag;
                uiItem.OnItemDroppedOn += HandleSwag;
                uiItem.OnItemEndedDrag += HandleEndDrag;
                uiItem.OnRightClick += HandleRightClick;
                uiItem.OnItemHovered += HandleItemHovered;
                uiItem.OnItemHoverExit += HandleItemHoverExit;

            }
        }

        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {
            if (listOfUIItems.Count > itemIndex)
            {
                listOfUIItems[itemIndex].SetData(itemImage, itemQuantity);
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
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
            {
                return;
            }

            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
            HandleItemSelection(inventoryItemUI);
        }

        private void ResetDraggtedItem()
        {
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
        }

        private void HandleBeginDrag(UiInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
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
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
            {
                return;
            }

            //OnDescriptionRequested?.Invoke(index);
        }

        // lista os itens no inventario
        public void Show()
        {
            // Armazena o índice do slot selecionado na hotbar antes de abrir o inventário
            previousSelectedIndex = FindObjectOfType<HotbarController>().GetSelectedIndex();
            inventoryPagePrefab.SetActive(true);
            ResetSelection();
            isInventoryOpen = true;
        }

        public void ResetSelection()
        {
            itemDescription.ResetDescription();
            DeselectAllItems();
        }

        private void DeselectAllItems()
        {
            foreach (UiInventoryItem item in listOfUIItems)
            {
                item.Deselect();
            }
        }

        public void Hide()
        {
            inventoryPagePrefab.SetActive(false);
            DeselectAllItems();
            description.ResetDescription();
            isInventoryOpen = false;
            ResetDraggtedItem();
            RestoreHotbarSelection();
        }

        private void HandleItemHovered(UiInventoryItem inventoryItemUI)
        {
            if (isInventoryOpen)
            {
                int index = listOfUIItems.IndexOf(inventoryItemUI);
                if (index != -1)
                {
                    OnDescriptionRequested?.Invoke(index);
                }
            }
        }

        private void HandleItemHoverExit(UiInventoryItem inventoryItemUI)
        {
            itemDescription.ResetDescription();
        }

        internal void UpdateDescription(int itemIndex, string name, string type, string description)
        {
            itemDescription.SetDescription(name, type, description);
            DeselectAllItems();
            listOfUIItems[itemIndex].Select();
        }

        internal void ResetAllItems()
        {
            foreach (var item in listOfUIItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }

        // Método para armazenar o índice do slot selecionado antes de abrir o inventário
        public void StoreHotbarSelection(int selectedIndex)
        {
            previousSelectedIndex = selectedIndex;
        }

        // Método para restaurar a seleção do slot na hotbar após fechar o inventário
        private void RestoreHotbarSelection()
        {
            if (previousSelectedIndex != -1)
            {
                // Simula a seleção do slot anterior
                FindObjectOfType<HotbarController>().SelectSlot(previousSelectedIndex);
            }
        }
    }
}