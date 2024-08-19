using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Inventory.UI
{
    public class UiInventoryItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI quantityText;

        [SerializeField] private Image borderImage;

        public event Action<UiInventoryItem> 
        OnItemClick, 
        OnItemDroppedOn, 
        OnItemBegginDrag, 
        OnItemEndedDrag, 
        OnRightClick, 
        OnItemHovered, 
        OnItemHoverExit;

        private bool empty = true;


        private void Awake()
        {
            ResetData();
            Deselect();
        }

        public void ResetData()
        {
            this.itemImage.gameObject.SetActive(false);
            this.quantityText.text = "";
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
            empty = false;
        }

        public void Select()
        {
            borderImage.enabled = true;
        }

        public void OnPointerClick(PointerEventData pointerData)
        {
            //if (empty) { return; }

            if (pointerData.button == PointerEventData.InputButton.Right)
            {
                OnRightClick?.Invoke(this);
            }
            else
            {
                OnItemClick?.Invoke(this);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        /*
        {
        if (empty) { return; }
        else
        */
        { OnItemBegginDrag?.Invoke(this); }
        //}

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndedDrag?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {

        }

        // Adicionando os eventos para quando o mouse entra e sai do slot
        public void OnPointerEnter(PointerEventData eventData)
        {
            OnItemHovered?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnItemHoverExit?.Invoke(this);
        }

    }

}