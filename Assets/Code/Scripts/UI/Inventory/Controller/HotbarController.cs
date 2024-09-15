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
            // Verifica se o item é nulo
            if (IsItemNull(item))
            {
                HandleNullItem(); // Lida com a lógica de item nulo
                return;
            }

            // Verifica se nenhum sprite está carregado
            if (IsNoSpriteLoaded())
            {
                HandleNoSpriteLoaded(item); // Lida com a lógica quando nenhum sprite está carregado
            }
            else
            {
                HandleSpriteLoaded(item); // Lida com a lógica quando um sprite já está carregado
            }

            // Verifica se o jogador está carregando algum objeto
            if (IsCarryingObject())
            {
                PlayerMechanics.ClearItemPickUp(); // Limpa a seleção de item caso esteja carregando algo
                print(3);
            }
        }

        /// <summary>
        /// Verifica se o item é nulo.
        /// </summary>
        /// <param name="item">O item a ser verificado.</param>
        /// <returns>Retorna true se o item for nulo.</returns>
        private bool IsItemNull(InventoryItem item)
        {
            return item.item == null;
        }

        /// <summary>
        /// Lida com a lógica quando o item é nulo.
        /// </summary>
        private void HandleNullItem()
        {
            PlayerMechanics.Carry.blockOfHotBar = false;

            // Se nenhum tile identificado está sendo carregado
            if (PlayerMechanics.Carry.identifiedTile == null)
            {
                PlayerMechanics.CleanObject(); // Limpa o objeto carregado
                print(0);
            }

            PlayerMechanics.ClearItemPickUp(); // Limpa a seleção de item na hotbar
            print(1);
        }

        /// <summary>
        /// Verifica se nenhum sprite está carregado.
        /// </summary>
        /// <returns>Retorna true se nenhum sprite está carregado.</returns>
        private bool IsNoSpriteLoaded()
        {
            // Verifica se PlayerMechanics não é nulo
            if (PlayerMechanics != null)
            {
                // Retorna true se o tileSprite for nulo
                return PlayerMechanics.Carry.tileSprite == null;
            }

            // Caso PlayerMechanics seja nulo, considera que não há sprite carregado
            return true;
        }

        /// <summary>
        /// Lida com a lógica quando nenhum sprite está carregado.
        /// </summary>
        /// <param name="item">O item a ser manuseado.</param>
        private void HandleNoSpriteLoaded(InventoryItem item)
        {
            if (PlayerMechanics != null)
            {
                PlayerMechanics.Carry.blockOfHotBar = item.item.IsPlaceable;

                if (item.item.IsPlaceable)
                {
                    PlayerMechanics.PickUpItem(item); // Pega o item se ele for colocável
                }
                else
                {
                    PlayerMechanics.GrabHoldItem(item); // Segura o item se ele não for colocável
                }
            }
        }

        /// <summary>
        /// Lida com a lógica quando um sprite já está carregado.
        /// </summary>
        /// <param name="item">O item a ser manuseado.</param>
        private void HandleSpriteLoaded(InventoryItem item)
        {
            if (!item.item.IsPlaceable && PlayerMechanics.Carry.blockOfHotBar)
            {
                PlayerMechanics.Carry.blockOfHotBar = false;
                PlayerMechanics.CleanObject(); // Limpa o objeto carregado
                PlayerMechanics.GrabHoldItem(item); // Segura o item da hotbar
            }
            else
            {
                PlayerMechanics.ClearItemPickUp(); // Limpa a seleção de item
                print(2);
            }
        }

        /// <summary>
        /// Verifica se o jogador está carregando algum objeto.
        /// </summary>
        /// <returns>Retorna true se o jogador estiver carregando um objeto.</returns>
        private bool IsCarryingObject()
        {
            if (PlayerMechanics != null)
            {

                return PlayerMechanics.Carry.tileSprite != null;
            }
            else

            return false;
        }

    }
}