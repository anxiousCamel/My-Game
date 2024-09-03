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
    private PlayerData_Collider Collider;
    public PlayerData_Stats stats;
    public ParticleSystem particleEat;

    private void Awake()
    {
        Input = GetComponent<PlayerData_Input>();
        stats = GetComponent<PlayerData_Stats>();
        Collider = GetComponent<PlayerData_Collider>();
        Mechanics = GetComponent<PlayerData_Mechanics>();
    }

    void Update()
    {

        if (//Input.CheckInput.inputObjectInteraction
        Input.Time.lastInputObjectInteraction == 0
             && !Mechanics.Carry.downloadToGetItem
             && !Mechanics.Carry.tileSprite
             && Mechanics.Throw.lastInteractionPlaceOrThrow >= Mechanics.Throw.cooldownThrow
             )
        {
            var selectedItem = Mechanics.GetSelectedItem();

            if (selectedItem.item != null && selectedItem.item.IsConsumable && Collider.Check.isGround)
            {
                Mechanics.Carry.consumableAnimgTriggered = true;
            }
        }
    }

    public void ParticleEating()
    {
        particleEat.Play();
    }

    public void UseItemHotbar()
    {
        inventoryController.HandleItemActionRequested(Mechanics.hotbarController.GetSelectedIndex(), gameObject);
    }

}