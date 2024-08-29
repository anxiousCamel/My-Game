using UnityEngine;
using Inventory.Model;
using Inventory.UI;
using System.Collections;
using System.Collections.Generic;


namespace Inventory.UI
{
    public class HotbarController : MonoBehaviour
    {
        public PlayerData_Mechanics PlayerMechanics;
        [SerializeField] private RectTransform contentPanelBar;
        [SerializeField] private InventorySO inventoryData;
        [SerializeField] private int selectedIndex = 0;
        [SerializeField] private int previousSelectedIndex = -1; // Armazena o índice anteriormente selecionado
        [SerializeField] private UiInventoryPage inventoryUi; // Adicionado para usar eventos já configurados

        // Adicionado um evento para quando o item selecionado mudar
        public event System.Action<InventoryItem> OnSelectedItemChanged;

        private void Start()
        {
            // Inscrever-se no evento OnInventoryUpdated
            inventoryData.OnInventoryUpdated += OnInventoryUpdated;

            // Atualiza a seleção da hotbar
            StartCoroutine(InitializeSelection());

            // Certifique-se de que a hotbar esteja armazenando a seleção ao abrir o inventário
            inventoryUi.StoreHotbarSelection(selectedIndex);
        }

        private void OnInventoryUpdated(Dictionary<int, InventoryItem> currentInventoryState)
        {
            StartCoroutine(SelectItemAfterUse());
        }

        private IEnumerator SelectItemAfterUse()
        {
            // Espera um quadro para garantir que a interface do usuário esteja completamente pronta
            yield return null;

            // Revalidar o item selecionado na Hotbar
            HandleSelectedItemPickup(SelectedItem());

            // Atualizar visualmente a Hotbar
            UpdateHotbarItemSelection();
        }

        private IEnumerator InitializeSelection()
        {
            // Espera um quadro para garantir que a interface do usuário esteja completamente pronta
            yield return null;

            // Seleciona o primeiro slot na inicialização
            SelectSlot(0);

            yield return null; // Espera um quadro para garantir 
            HandleSelectedItemPickup(SelectedItem());
        }

        private void Update()
        {
            HandleKeyboardInput();
        }

        private void HandleKeyboardInput()
        {
            for (int i = 0; i < 9; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    SelectSlot(i);
                    break; // Para evitar múltiplas seleções ao mesmo tempo
                }
            }
        }

        public void SelectSlot(int index)
        {
            // Atualiza o índice apenas se for diferente
            if (selectedIndex != index)
            {
                selectedIndex = index;
                UpdateHotbarItemSelection();

                HandleSelectedItemPickup(SelectedItem());
            }
            else
            {
                // Mesmo que o índice não tenha mudado, ainda precisamos garantir que o slot esteja selecionado na inicialização
                UpdateHotbarItemSelection();
            }
        }

        public InventoryItem SelectedItem()
        {
            InventoryItem Item = GetSelectedItem();
            OnSelectedItemChanged?.Invoke(Item);
            return Item;
        }



        private void UpdateHotbarItemSelection()
        {
            // Desseleciona o slot anterior, se houver
            if (previousSelectedIndex >= 0 && previousSelectedIndex != selectedIndex)
            {
                var previousItem = contentPanelBar.GetChild(previousSelectedIndex).GetComponent<UiInventoryItem>();
                if (previousItem != null)
                {
                    //desselecionou o anterior
                    previousItem.Deselect();
                }
            }

            // Seleciona o novo slot
            var currentItem = contentPanelBar.GetChild(selectedIndex).GetComponent<UiInventoryItem>();
            if (currentItem != null)
            {
                //selecionou o novo");
                currentItem.Select();
            }

            // Atualiza o índice anterior
            previousSelectedIndex = selectedIndex;
        }

        public InventoryItem GetSelectedItem()
        {
            return inventoryData.GetItemAt(selectedIndex);
        }

        public int GetSelectedIndex()
        {
            return selectedIndex;
        }

        public void HandleSelectedItemPickup(InventoryItem item)
        {
            if (item.item == null)
            {
                PlayerMechanics.Carry.blockOfHotBar = false;
                PlayerMechanics.ClearItemPickUp();
                return;
            }

            // Se não há um sprite carregado
            if (PlayerMechanics.Carry.tileSprite == null)
            {
                PlayerMechanics.Carry.blockOfHotBar = item.item.IsPlaceable;

                if (item.item.IsPlaceable)
                {
                    PlayerMechanics.PickUpItem(item);
                }
                else
                {
                    PlayerMechanics.GrabHoldItem(item);
                }
            }
            // Se há um sprite carregado
            else
            {
                if (!item.item.IsPlaceable && PlayerMechanics.Carry.blockOfHotBar)
                {
                    PlayerMechanics.Carry.blockOfHotBar = false;
                    PlayerMechanics.CleanObject();
                    PlayerMechanics.GrabHoldItem(item);
                }
                else
                {
                    PlayerMechanics.ClearItemPickUp();
                }
            }

            // Se carrega um objeto do mundo
            if (PlayerMechanics.Carry.tileSprite != null)
            {
                PlayerMechanics.ClearItemPickUp();
            }
        }
    }
}