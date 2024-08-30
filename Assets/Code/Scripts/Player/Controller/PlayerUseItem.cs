using System.Collections;
using System.Collections.Generic;
using Inventory.Model;
using Inventory.UI;
using UnityEngine;
using Inventory;
using UnityEditor.Rendering;


public class PlayerUseItem : MonoBehaviour
{
    private PlayerData_Input Input;
    private PlayerData_Mechanics Mechanics;
    public InventoryController inventoryController;
    public PlayerData_Stats stats;

    private void Awake()
    {
        Input = GetComponent<PlayerData_Input>();
        stats = GetComponent<PlayerData_Stats>();

        Mechanics = GetComponent<PlayerData_Mechanics>();
    }

    void Update()
    {

        if (//Input.CheckInput.inputObjectInteraction
        Input.Time.lastInputObjectInteraction == 0
             && !Mechanics.Carry.downloadToGetItem
             && !Mechanics.Carry.tileSprite
             // fazer um cooldown de quando coloca ou arremesa item para poder consumir item 
             && Mechanics.Throw.lastInteractionPlaceOrThrow >= Mechanics.Throw.cooldownThrow
             )
        {
            var selectedItem = Mechanics.GetSelectedItem();

            if (selectedItem.item != null && selectedItem.item.IsConsumable)
            {
                UseItemHotbar();
            }
        }
    }

    public void UseItemHotbar()
    {
        inventoryController.HandleItemActionRequested(Mechanics.hotbarController.GetSelectedIndex(), gameObject);
    }

}