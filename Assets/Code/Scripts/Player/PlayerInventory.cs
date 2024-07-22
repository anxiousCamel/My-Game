using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [ReadOnly] public Dictionary<int, InventoryItem> itemsDictionary = new Dictionary<int, InventoryItem>();

    // Adicionar item ao inventário
    public void AddItem(Item item)
    {
        // Verificar se o item é stackable e se já existe no inventário
        if (item.stackable && itemsDictionary.ContainsKey(item.id))
        {
            // Se o item for empilhável e já estiver no inventário, aumentar a quantidade
            itemsDictionary[item.id].quantity++;
        }
        else
        {
            // Adicionar novo item ao inventário
            InventoryItem newItem = new InventoryItem(item, 1); // Inicializar com quantidade 1
            itemsDictionary.Add(item.id, newItem);
        }
    }

    void Update()
    {
        // Verificar se a tecla 'i' foi pressionada
        if (Input.GetKeyDown(KeyCode.I))
        {
            // Imprimir o conteúdo do dicionário no console
            Debug.Log("Inventário:");
            foreach (var kvp in itemsDictionary)
            {
                Debug.Log($"Item ID: {kvp.Key}, Item Name: {kvp.Value.item.itemName}, Item Quantity: {kvp.Value.quantity}");
                // Você pode adicionar mais propriedades aqui conforme necessário
            }
        }
    }

    // Esta classe é usada para armazenar informações sobre um item no inventário, incluindo a quantidade
    [System.Serializable]
    public class InventoryItem
    {
        public Item item; // Referência ao objeto Item original
        public int quantity; // Quantidade do item no inventário

        // Construtor para inicializar um InventoryItem com um Item e uma quantidade
        public InventoryItem(Item item, int quantity)
        {
            this.item = item; // Atribui o objeto Item original
            this.quantity = quantity; // Define a quantidade inicial do item no inventário
        }
    }
}
