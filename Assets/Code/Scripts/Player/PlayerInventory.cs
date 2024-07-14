using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Dictionary<int, Item> itemsDictionary = new Dictionary<int, Item>();

    // Adicionar item ao inventário
    public void AddItem(int itemId, Item item)
    {
        if (item.stackable && itemsDictionary.ContainsKey(itemId))
        {
            // Se o item for empilhável e já estiver no inventário, aumentar a quantidade
            itemsDictionary[itemId].MinQuantityDrop += Random.Range(item.MinQuantityDrop, item.MaxQuantityDrop + 1);
        }
        else
        {
            // Adicionar novo item ao inventário
            itemsDictionary.Add(itemId, item);
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
                Debug.Log($"Item ID: {kvp.Key}, Item Name: {kvp.Value.itemName}");
                // Você pode adicionar mais propriedades aqui conforme necessário
            }
        }
    }
}
