using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public PlayerInventory inventory;
    public Sprite nullImage;

    [Space(20)]
    [ReadOnly] public int maxSpaceIventoryBar;
    public GameObject[] portraitItensBar;
    [ReadOnly] public Image[] ItensBarImage;
    [ReadOnly] public TextMeshProUGUI[] ItensBarText;

    [Space(20)]

     [ReadOnly] public int maxSpaceIventory;
    public GameObject[] portraitItens;
    [ReadOnly] public Image[] ItensImage;
    [ReadOnly] public TextMeshProUGUI[] ItensText;


    void Awake()
    {
        // Inicializar barra
        StartBarInventory();

        // Inicializar Inventario
        StartInventory();

        // Atualizar as imagens do inventário na inicialização
        UpdateInventoryBar();
    }

    private void StartInventory()
    {
        maxSpaceIventory = portraitItens.Length;

        // Inicializar arrays
        ItensImage = new Image[maxSpaceIventory];
        ItensText = new TextMeshProUGUI[maxSpaceIventory];

        for (int index = 0; index < maxSpaceIventory; index++)
        {
            Transform childTransform = portraitItens[index].transform.GetChild(0);
            ItensImage[index] = childTransform.GetComponent<Image>();
            ItensText[index] = portraitItens[index].GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    void StartBarInventory()
    {
        maxSpaceIventoryBar = portraitItensBar.Length;

        // Inicializar arrays
        ItensBarImage = new Image[maxSpaceIventoryBar];
        ItensBarText = new TextMeshProUGUI[maxSpaceIventoryBar];

        for (int index = 0; index < maxSpaceIventoryBar; index++)
        {
            Transform childTransform = portraitItensBar[index].transform.GetChild(0);
            ItensBarImage[index] = childTransform.GetComponent<Image>();
            ItensBarText[index] = portraitItensBar[index].GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public void UpdateInventoryBar()
    {
        int i = 0;
        foreach (var item in inventory.itemsDictionary)
        {
            if (i < maxSpaceIventoryBar) // Atualizar a barra de inventário
            {
                ItensBarImage[i].sprite = item.Value.item.icon; // Atualizar a imagem
                ItensBarText[i].text = item.Value.quantity.ToString();
            }
            else if (i < maxSpaceIventory) // Atualizar o inventário completo
            {
                int inventoryIndex = i - maxSpaceIventoryBar;
                ItensImage[inventoryIndex].sprite = item.Value.item.icon; // Atualizar a imagem
                ItensText[inventoryIndex].text = item.Value.quantity.ToString();
            }
            i++;
        }

        // Caso o número de itens seja menor que o espaço do inventário, limpar as imagens e textos restantes
        for (int j = i; j < maxSpaceIventoryBar; j++)
        {
            ItensBarImage[j].sprite = nullImage;
            ItensBarText[j].text = ""; // Limpar o texto
        }

        for (int k = i; k < maxSpaceIventory; k++)
        {
            ItensImage[k].sprite = nullImage;
            ItensText[k].text = ""; // Limpar o texto
        }
    }
}
