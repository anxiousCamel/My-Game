using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PlayerCarry : MonoBehaviour
{
    private PlayerData_Input Input;
    private PlayerData_Mechanics Mechanics;
    private PlayerData_Collider Collider;
    private PlayerData_Physics Physic;

    private PlayerUseItem playerUseItem;


    private void Awake()
    {
        Input = GetComponent<PlayerData_Input>();
        Mechanics = GetComponent<PlayerData_Mechanics>();
        Collider = GetComponent<PlayerData_Collider>();
        Physic = GetComponent<PlayerData_Physics>();
        playerUseItem = GetComponent<PlayerUseItem>();
    }

    private void Update()
    {

        #region Interaction throw or place Cooldown
        Mechanics.Throw.lastInteraction += 0.01f;
        Mechanics.Throw.lastInteractionPlaceOrThrow += 0.01f;
        #endregion

        #region BugFix When hurt
        if (Mechanics.Hurt.isHurt)
        {
            Mechanics.Throw.throwingObject = false;
            Mechanics.Throw.throwingObjectInExecution = false;
            Mechanics.Carry.downloadToGetItem = false;
        }
        #endregion


        #region Lançamento de Objetos
        // Verifica se é possível iniciar o lançamento de um objeto e, em caso afirmativo, inicia o lançamento.
        if (CanThrow())
        {
            Mechanics.Throw.throwingTriggered = true;
        }

        if (Mechanics.Throw.throwingTriggered == true && Input.Time.lastInputUpObjectInteraction == 0)
        {
            StartThrowingObject();
        }

        if (CanToPlace())
        {
            Mechanics.Throw.lastInteraction = 0;
            if (Mechanics.Carry.identifiedGameObject != null)
            {
                // Instanciar
                GameObject instantiatedObject = Instantiate(Mechanics.Carry.identifiedGameObject, Mechanics.ToPlace.Tilemap.transform);

                // Descontar
                if (Mechanics.GetSelectedItem().item.IsPlaceable)
                {
                    instantiatedObject.GetComponent<SpriteRenderer>().sprite = Mechanics.GetSelectedItem().item.ItemImage;
                    playerUseItem.UseItemHotbar();
                }

                // Cooldown use item
                Mechanics.Throw.lastInteractionPlaceOrThrow = 0;

                // Mover para posição
                instantiatedObject.transform.localPosition = (Vector2)Mechanics.ToPlace.Preview.transform.position + Mechanics.ToPlace.offsetToPlace;

                // Ativar
                instantiatedObject.SetActive(true);

                // Limpar
                if (Mechanics.Carry.identifiedGameObject != null && !EditorUtility.IsPersistent(Mechanics.Carry.identifiedGameObject))
                {
                    Destroy(Mechanics.Carry.identifiedGameObject);
                }


                Mechanics.CleanObject();
            }

            else if (Mechanics.Carry.identifiedTile != null)
            {
                // Instanciar
                Mechanics.ToPlace.prefabToPlace.GetComponent<SpriteRenderer>().sprite = Mechanics.Carry.tileSprite;
                GameObject instantiatedObject = Instantiate(Mechanics.ToPlace.prefabToPlace, Mechanics.ToPlace.Tilemap.transform);

                // Cooldown use item
                Mechanics.Throw.lastInteractionPlaceOrThrow = 0;

                // Descontar
                if (Mechanics.GetSelectedItem().item.IsPlaceable)
                {
                    instantiatedObject.GetComponent<SpriteRenderer>().sprite = Mechanics.GetSelectedItem().item.ItemImage;
                    playerUseItem.UseItemHotbar();
                }

                // Mover para posição
                instantiatedObject.transform.localPosition = (Vector2)Mechanics.ToPlace.Preview.transform.position + Mechanics.ToPlace.offsetToPlace;

                // Ativar
                instantiatedObject.SetActive(true);

                // Limpar
                Mechanics.CleanObject();
            }
        }

        bool CanToPlace()
        {
            return Input.Time.lastInputUpObjectInteraction <= Mechanics.ToPlace.TimeToPlace
            && Mechanics.ToPlace.canPlace
            && Mechanics.Carry.tileSprite != null
            && !Mechanics.Throw.throwingObject
            && !Mechanics.Throw.throwingObjectInExecution
            && !Mechanics.Hurt.isHurt
            && !Mechanics.Carry.downloadToGetItem
            && Mechanics.Throw.lastInteraction >= Mechanics.Throw.cooldownThrow;
        }


        bool CanThrow()
        {
            return Input.Time.durationInputObjectInteraction >= Mechanics.ToPlace.TimeToPlace
                && Mechanics.Carry.tileSprite != null
                && Mechanics.Target.canMoveTarget
                && !Mechanics.Throw.throwingObject
                && !Mechanics.Throw.throwingObjectInExecution
                && !Mechanics.Hurt.isHurt
                && Mechanics.Throw.lastInteraction >= Mechanics.Throw.cooldownThrow;
        }

        void StartThrowingObject()
        {
            Mechanics.Throw.throwingTriggered = false;
            Mechanics.Throw.throwingObject = true;
            Mechanics.Throw.lastInteraction = 0;
        }

        #endregion

        #region Atualização da Posição do Objeto Identificado
        if (Mechanics.Carry.downloadToGetItem == false)
        {
            Mechanics.Carry.newPosition = (Input.CheckInput.moveDirection.y == -1)
                ? new Vector2(Mechanics.Carry.positionPickupDown.x * Input.CheckInput.direction, Mechanics.Carry.positionPickupDown.y)
                : new Vector2(Mechanics.Carry.positionPickupFront.x * Input.CheckInput.direction, Mechanics.Carry.positionPickupFront.y);

            Collider.Component.Identifier.transform.position = Physic.Component.playerGameObject.transform.position + new Vector3(Mechanics.Carry.newPosition.x, Mechanics.Carry.newPosition.y);
        }
        #endregion

        #region FreezePositionDownloadToGetItem
        else { Physic.FreezePosition(); }
        #endregion

        #region Manipulação de Carregamento de Objetos [Carry]
        if (Input.CheckInput.inputObjectInteraction && Mechanics.Carry.tileSprite == null && !Mechanics.Hurt.isHurt && !Mechanics.Throw.throwingObject && !Mechanics.Throw.throwingObjectInExecution)
        {
            // Verifica antes se tem objeto
            Mechanics.IdentifiedObject();

            if (Mechanics.Carry.identifiedGameObject != null || Mechanics.Carry.identifiedTile != null)
            {
                Mechanics.Carry.downloadToGetItem = true;
            }

            // Limpa objeto se por acaso não pegou
            if (Mechanics.Carry.identifiedGameObject != null || Mechanics.Carry.identifiedTile != null && Mechanics.Carry.tileSprite == null)
            {
                Mechanics.CleanObject();
            }
        }

        if (Mechanics.Hurt.isHurt)
        {
            Mechanics.CleanObject();
        }
        #endregion

        #region Target 
        // Verifica se há um objeto para carregar antes de prosseguir
        else if (
            Input.Time.durationInputObjectInteraction >= Mechanics.ToPlace.TimeToPlace
            && Mechanics.Carry.tileSprite != null
            && !Mechanics.Hurt.isHurt
            && Mechanics.Throw.lastInteraction >= Mechanics.Throw.cooldownThrow)
        {
            if (Mechanics.Target.canMoveTarget == false)
            {
                Mechanics.TargetCanMove();
            }
        }

        else
        {
            if (Mechanics.Target.canMoveTarget == true)
            {
                Mechanics.TargetCanMove();
            }
        }
        #endregion
    }
}