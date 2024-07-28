using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UiInventoryItem : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI quantityText;

    [SerializeField] private Image borderImage;

    public event Action<UiInventoryItem>
        OnItemClick,
        OnItemDroppedOn,
        OnItemBegginDrag,
        OnItemEndedDrag,
        OnRightClick;

    private bool empty = true;


    private void Awake()
    {
        ResetData();
        Deselect();
    }

    public void ResetData()
    {
        this.itemImage.gameObject.SetActive(false);
        this.empty = true;
    }
    public void Deselect()
    {
        borderImage.enabled = false;
    }

    public void SetData(Sprite sprite, int quantity)
    {
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.quantityText.text = quantity + "";
    }

    public void Select()
    {
        borderImage.enabled = true;
    }

    public void OnBeginDrag()
    {
        if (empty) { return; }
        else { OnItemBegginDrag?.Invoke(this); }

    }

    public void OnDrop()
    {
        OnItemDroppedOn?.Invoke(this);
    }

    public void OnEndDrag()
    {
        OnItemEndedDrag?.Invoke(this);
    }

    public void OnPointerClick(BaseEventData data)
    {
        //if (empty) { return; }

        PointerEventData pointerData = (PointerEventData)data;
        if (pointerData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick?.Invoke(this);
        }
        else
        {
            OnItemClick?.Invoke(this);
        }
    }

}
