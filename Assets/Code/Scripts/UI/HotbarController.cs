using UnityEngine;
using Inventory.Model;
using Inventory.UI;
using System.Collections;

namespace Inventory.UI
{
    public class HotbarController : MonoBehaviour
    {
        [SerializeField] private RectTransform contentPanelBar;
        [SerializeField] private InventorySO inventoryData;
        [SerializeField] private int selectedIndex = 0;
        [SerializeField] private UiInventoryPage inventoryUi; // Adicionado para usar eventos já configurados

        // Adicionado um evento para quando o item selecionado mudar
        public event System.Action<InventoryItem> OnSelectedItemChanged;

        private void Start()
        {
            // Atualiza a seleção da hotbar
            StartCoroutine(InitializeSelection());

            // Certifique-se de que a hotbar esteja armazenando a seleção ao abrir o inventário
            inventoryUi.StoreHotbarSelection(selectedIndex);
        }

        private IEnumerator InitializeSelection()
        {
            // Espera um quadro para garantir que a interface do usuário esteja completamente pronta
            yield return null;

            // Seleciona o primeiro slot na inicialização
            SelectSlot(0);
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
                // Obtém o item selecionado e chama o evento
                InventoryItem selectedItem = GetSelectedItem();
                OnSelectedItemChanged?.Invoke(selectedItem);
                // Imprime as informações do item selecionado
                PrintSelectedItemInfo(selectedItem);
            }
            else
            {
                // Mesmo que o índice não tenha mudado, ainda precisamos garantir que o slot esteja selecionado na inicialização
                UpdateHotbarItemSelection();
            }
        }

        private void UpdateHotbarItemSelection()
        {
            // Itera sobre todos os slots na hotbar
            for (int i = 0; i < contentPanelBar.childCount; i++)
            {
                var uiItem = contentPanelBar.GetChild(i).GetComponent<UiInventoryItem>();
                if (uiItem != null)
                {
                    if (i == selectedIndex)
                    {
                        uiItem.Select();
                    }
                    else
                    {
                        uiItem.Deselect();
                    }
                }
            }
        }

        // Método para obter o item selecionado
        public InventoryItem GetSelectedItem()
        {
            return inventoryData.GetItemAt(selectedIndex);
        }

        public int GetSelectedIndex()
        {
            return selectedIndex;
        }

        // Método para imprimir as informações do item selecionado
        private void PrintSelectedItemInfo(InventoryItem item)
        {
            if (item.item != null)
            {

                Debug.Log($"Nome: {item.item.Name} Tipo: {item.item.Type} Descrição: {item.item.Description} Imagem: {item.item.ItemImage} Max Stack Size: {item.item.MaxStackSize} Is Stackable: {item.item.IsStackable}");
            }
            else
            {
                Debug.Log("Nenhum item selecionado.");
            }
        }
    }
}
