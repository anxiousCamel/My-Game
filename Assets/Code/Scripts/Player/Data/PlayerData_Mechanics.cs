using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Inventory.Model;
using Inventory.UI;

public class PlayerData_Mechanics : MonoBehaviour
{
    private PlayerData_Collider Collider;
    private PlayerData_Movement Movement;
    private PlayerData_Physics Physics;
    private PlayerData_Input Input;
    private PlayerUseItem playerUseItem;
    public IdentifyTile Tile;
    public HotbarController hotbarController;

    [Space(5)] public toPlace ToPlace = new();
    [Space(5)] public carry Carry = new();
    [Space(5)] public throwing Throw = new();
    [Space(5)] public target Target = new();
    [Space(5)] public time Time = new();
    [Space(5)] public hurt Hurt = new();

    [System.Serializable]
    public class toPlace
    {
        [ReadOnly] public bool canPlace;
        public float TimeToPlace;
        public GameObject Preview;
        public GameObject Tilemap;
        public GameObject prefabToPlace;
        public Vector2 offsetToPlace;
    }

    [System.Serializable]
    public class target
    {
        public bool canMoveTarget;
        public Vector2 targetPosition;
        public GameObject targetObject;
        public float speedTarget;
        [Range(0, 10)] public float maxDistanceFromPlayer;
    }

    [System.Serializable]
    public class time
    {
        public float startTimeScale;
        public float startTimeFixedDelta;
        [Range(0, 1)] public float slowTimeScale;
        [Range(1, 2)] public float fastTimeScale;
    }


    [System.Serializable]
    public class carry
    {
        public bool blockOfHotBar = false;
        public bool downloadToGetItem;
        public Transform holderPosition;
        public Sprite tileSprite;
        public TileBase identifiedTile;
        public GameObject identifiedGameObject;
        public Vector2 positionPickupDown;
        public Vector2 positionPickupFront;
        public BoxCollider2D colObjectHolder;
        public Vector3 newPosition;
    }



    [System.Serializable]
    public class throwing
    {
        public bool throwingTriggered;
        public float cooldownThrow;
        [ReadOnly] public float lastInteractionPlaceOrThrow;
        [ReadOnly] public float lastInteraction;
        public SpriteRenderer objectHolder;
        public SpriteRenderer ItemHolder;
        public bool throwingObject;
        public bool throwingObjectInExecution;
        public float throwForce;
        public GameObject prefabObjectThrow;
    }

    [System.Serializable]
    public class hurt
    {
        public ParticleSystem particleDestroyHolderItem;
        public bool isHurt;
        public Vector2 hurtForce;
    }



    private void Awake()
    {
        Collider = GetComponent<PlayerData_Collider>();
        Physics = GetComponent<PlayerData_Physics>();
        Input = GetComponent<PlayerData_Input>();
        Movement = GetComponent<PlayerData_Movement>();
        playerUseItem = GetComponent<PlayerUseItem>();
    }

    #region Hotbar
    public InventoryItem GetSelectedItem()
    {
        InventoryItem item = hotbarController.GetSelectedItem();
        return item;
    }
    #endregion


    #region Thorn
    // Retorna a posição de arremeso
    public Vector2 ThrowDirection()
    {
        return (Target.targetPosition - Physics.PlayerPosition()).normalized;
    }

    public void CreateObjectThorn()
    {
        // Cooldown use item
        Throw.lastInteractionPlaceOrThrow = 0;

        // Descontar
        if (GetSelectedItem().item.IsPlaceable)
        {
            playerUseItem.UseItemHotbar();
        }

        // Instanciar sem transformador pai (será colocado na cena)
        GameObject ProjectTile = Instantiate(Throw.prefabObjectThrow, Carry.holderPosition.position, Quaternion.identity, null);

        // Dar um sprite
        ProjectTile.GetComponent<SpriteRenderer>().sprite = Carry.tileSprite;

        // Aplicar velocidade (forças)
        ProjectTile.GetComponent<Rigidbody2D>().velocity = ThrowDirection() * Throw.throwForce;

        // Limpar 
        CleanObject();
    }

    public void DestroyObject(GameObject objectToDestroy)
    {
        // Destroi o objeto passado como argumento
        Destroy(objectToDestroy);
    }
    #endregion


    #region PickUp
    public void PickUpTile()
    {
        // Identificar
        IdentifiedObject();

        // Atribuir Sprite
        Throw.objectHolder.sprite = Carry.tileSprite;

        Carry.colObjectHolder.enabled = true;

        // garantir que não é um item da hotbar
        Carry.blockOfHotBar = false;
        ClearItemPickUp();

        // Remover tile
        RemoveTile();
    }

    // levantar o item nos braços
    public void PickUpItem(InventoryItem item)
    {
        Throw.objectHolder.sprite = item.item.ItemImage;
        Carry.colObjectHolder.enabled = true;

        Carry.identifiedGameObject = item.item.PrefabToPlace;
        Carry.tileSprite = item.item.ItemImage;
        Carry.identifiedTile = null;

        Throw.ItemHolder.sprite = null;
    }

    // colocar item da hotbar na mao
    public void GrabHoldItem(InventoryItem item)
    {
        Throw.ItemHolder.sprite = item.item.ItemImage;
    }

    public void ClearItemPickUp()
    {
        Throw.ItemHolder.sprite = null;
        Carry.colObjectHolder.enabled = false;
    }

    public void FinishDownloadToGetItem()
    {
        Carry.downloadToGetItem = false;
        IdentifyTileCache.canIdentify = true;
    }

    public void ThrowingObjectInExecution()
    {
        Throw.throwingObjectInExecution = true;
    }

    public void FinishObjectThorn()
    {
        Throw.throwingObject = false;
        Throw.throwingObjectInExecution = false;
    }



    void RemoveTile()
    {
        if (Tile.tileMaps.Count > 0)
        {
            Vector3Int tilePosition = Tile.tileMaps[0].WorldToCell(transform.position + Carry.newPosition); // Usar o primeiro Tilemap como referência

            IdentifyTile.IdentifiedObjectResult result = Tile.GetTileOrGameObject(tilePosition);

            if (result != null)
            {
                if (result.Tile != null)
                {
                    foreach (Tilemap tileMap in Tile.tileMaps)
                    {
                        tileMap.SetTile(tilePosition, null);
                    }
                }
                else if (result.GameObject != null)
                {
                    result.GameObject.SetActive(false);
                }
            }
            else
            {
                Debug.Log("No tile or GameObject to remove at position: " + transform.position + Carry.newPosition);
            }
        }
        else
        {
            Debug.LogWarning("No Tilemaps assigned to the IdentifyTile component.");
        }
    }

    public void IdentifiedObject()
    {
        IdentifyTile.IdentifiedObjectResult result = Tile.GetTileOrGameObject(PositionIdentifier(), Collider.Tag.downloadable);

        if (result != null)
        {
            if (result.Tile != null)
            {
                Carry.identifiedTile = result.Tile;
                Carry.tileSprite = Tile.GetTileSprite(Carry.identifiedTile, PositionIdentifier());
                Carry.identifiedGameObject = null;
            }
            else if (result.GameObject != null)
            {
                Carry.identifiedGameObject = result.GameObject;
                Carry.tileSprite = result.GameObject.GetComponent<SpriteRenderer>().sprite;
                Carry.identifiedTile = null;
            }
        }
        else
        {
            Carry.identifiedTile = null;
            Carry.identifiedGameObject = null;
            Carry.tileSprite = null;
        }
    }


    // Limpa Objeto
    public void CleanObject()
    {
        Carry.colObjectHolder.enabled = false;
        Carry.identifiedGameObject = null;
        Carry.identifiedTile = null;
        Carry.tileSprite = null;

        Throw.objectHolder.sprite = null;

        hotbarController.HandleSelectedItemPickup(hotbarController.SelectedItem());
    }

    public Vector3Int PositionIdentifier()
    {
        return new(
            Mathf.FloorToInt(Collider.Component.Identifier.transform.position.x),
            Mathf.FloorToInt(Collider.Component.Identifier.transform.position.y),
            Mathf.FloorToInt(Collider.Component.Identifier.transform.position.z)
        );
    }
    #endregion

    #region Target
    public void TargetResetPosition()
    {
        Target.targetObject.transform.position = Physics.Component.playerGameObject.transform.position;
        Target.targetPosition = Physics.Component.playerGameObject.transform.position; ;
    }

    public void TargetCanMove()
    {
        //Ativa e Desativa o movimento do target de acordo com o ultimo estado de canMove ao chamar a função
        Target.canMoveTarget = (Target.canMoveTarget == false) ? true : false;
    }
    #endregion


}

