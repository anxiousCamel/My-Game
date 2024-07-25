using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public PlayerInventory inventory;
    public Sprite nullImage;
    public GameObject[] portraitItensBar;
    [ReadOnly] public int maxSpaceIventoryBar;
    [ReadOnly] public Image[] ItensImage;
    [ReadOnly] public TextMeshProUGUI[] ItensText;

    void Start()
    {
        maxSpaceIventoryBar = portraitItensBar.Length;

        // Inicializar arrays
        ItensImage = new Image[maxSpaceIventoryBar];
        ItensText = new TextMeshProUGUI[maxSpaceIventoryBar];

        for (int index = 0; index < maxSpaceIventoryBar; index++)
        {
            Transform childTransform = portraitItensBar[index].transform.GetChild(0);
            ItensImage[index] = childTransform.GetComponent<Image>();
            ItensText[index] = portraitItensBar[index].GetComponentInChildren<TextMeshProUGUI>();
        }

        // Atualizar as imagens do inventário na inicialização
        UpdateInventoryBar();
    }

    public void UpdateInventoryBar()
    {
        int i = 0;
        foreach (var item in inventory.itemsDictionary)
        {
            if (i >= 9) break; // Limitar a 9 itens
            ItensImage[i].sprite = item.Value.item.icon; // Atualizar a imagem
            ItensText[i].text = item.Value.quantity.ToString();
            i++;
        }

        // Caso o número de itens seja menor que 9, limpar as imagens restantes
        for (; i < 9; i++)
        {
            ItensImage[i].sprite = nullImage;
            ItensText[i].text = ""; // Limpar o texto
        }
    }
}
