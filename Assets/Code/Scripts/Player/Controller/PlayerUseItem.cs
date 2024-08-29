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
    private HotbarController Hotbar;
    public InventoryController inventoryController;
    public PlayerData_Stats stats;

    private void Awake()
    {
        Input = GetComponent<PlayerData_Input>();
        stats = GetComponent<PlayerData_Stats>();

        Mechanics = GetComponent<PlayerData_Mechanics>();
        Hotbar = Mechanics.hotbarController;
    }

    void Update()
    {

        if (Input.CheckInput.inputObjectInteraction)
        {
            InventoryItem item = Hotbar.GetSelectedItem(); // colocar isso nas mechanics mover pra la q vai ser mais util pra outras coisas
            
            if (item.item.IsConsumable)
            {
                UseItemHotbar();
            }
        }
    }

    public void UseItemHotbar()
    {
        inventoryController.HandleItemActionRequested(Hotbar.GetSelectedIndex(), gameObject);
    }

}
/*
tem que melhorar isso aqui de place item, as vezes n da pra colocar 
o item e eles desconta verificar a logica melhor, para que a confirmação 
venha da function que coloca o item*/