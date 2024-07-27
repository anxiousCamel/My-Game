using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private UiInventoryPage inventoryUi;
    [SerializeField] private PlayerData_Input input;

    public int inventorySize;
    public int barSize;

    private void Start() {

        inventoryUi.InitializeInventoryUi(inventorySize, barSize);
    }

    private void Update() {

        if (input.CheckInput.inputInventory) {
            if (inventoryUi.inventoryPagePrefab.activeSelf) {
                inventoryUi.Hide();
            } else {
                inventoryUi.Show();
            }
        }
    }
}
