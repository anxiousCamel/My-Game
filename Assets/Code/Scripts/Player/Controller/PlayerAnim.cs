using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private PlayerData_Mechanics Mechanics;
    private PlayerData_Movement Movement;
    private PlayerData_Collider Collider;
    private PlayerData_Physics Physic;
    private PlayerData_Input Input;
    private PlayerData_Anim Anim;

    void Awake()
    {
        Mechanics = GetComponent<PlayerData_Mechanics>();
        Movement = GetComponent<PlayerData_Movement>();
        Collider = GetComponent<PlayerData_Collider>();
        Physic = GetComponent<PlayerData_Physics>();
        Input = GetComponent<PlayerData_Input>();
        Anim = GetComponent<PlayerData_Anim>();
    }

    void Update()
    {
        if(Mechanics.Hurt.isHurt)
        {
            Anim.ChangeAnimationStateEvent(PlayerData_Anim.AnimationState.Hurt);
            Anim.ChangeAnimationState(PlayerData_Anim.AnimationState.Hurt);
        }
        #region CanMove
        if (Movement.Controllers.canMove == true && !Mechanics.Hurt.isHurt)
        {
            #region Idle 
            if (Mechanics.Carry.downloadToGetItem == false && Mechanics.Throw.throwingObjectInExecution == false)
            {
                if (Collider.Check.isSolid && Physic.BodyVelocity().x < 0.1f)
                {
                    if(Physic.BodyVelocity().y < 0.1f)
                    {
                        if (Mechanics.Carry.identifiedGameObject != null || Mechanics.Carry.identifiedTile != null)
                        {
                            Anim.ChangeAnimationState(PlayerData_Anim.AnimationState.IdleCarry);
                        }
                        else
                        {
                            Anim.ChangeAnimationState(PlayerData_Anim.AnimationState.Idle);
                        }
                    }
                }
            }
            #endregion

            #region Run      
            if (Physic.BodyVelocity().x > 0.1 && Collider.Check.isSolid && Physic.BodyVelocity().y < 0.1f)
            {
                if (Mechanics.Carry.identifiedGameObject != null || Mechanics.Carry.identifiedTile != null)
                {
                    Anim.ChangeAnimationState(PlayerData_Anim.AnimationState.RunCarry);
                }
                else
                {
                    Anim.ChangeAnimationState(PlayerData_Anim.AnimationState.Run);
                }
            }
            #endregion

            #region Jump
            if (Physic.BodyVelocity().y > 0.1 && !Collider.Check.isSolid && Movement.WallMove.isClimb == false)
            {
                if (Mechanics.Carry.identifiedGameObject != null || Mechanics.Carry.identifiedTile != null)
                {
                    Anim.ChangeAnimationState(PlayerData_Anim.AnimationState.JumpCarry);
                }
                else
                {
                    Anim.ChangeAnimationState(PlayerData_Anim.AnimationState.Jump);
                }
            }
            #endregion

            #region Fall
            if (Mechanics.Carry.downloadToGetItem == false && Movement.WallMove.isSliding == false && Movement.WallMove.isClimb == false && Mechanics.Throw.throwingObjectInExecution == false)
            {
                if (Physic.BodyVelocity().y < 0.1 && !Collider.Check.isSolid)
                {
                    if (Mechanics.Carry.identifiedGameObject != null || Mechanics.Carry.identifiedTile != null)
                    {
                        Anim.ChangeAnimationState(PlayerData_Anim.AnimationState.FallCarry);
                    }
                    else
                    {
                        Anim.ChangeAnimationState(PlayerData_Anim.AnimationState.Fall);
                    }
                }
            }
            #endregion

            #region PickUp
            if (Mechanics.Carry.downloadToGetItem == true)
            {
                if (Input.CheckInput.moveDirection.y == -1)
                {
                    Anim.ChangeAnimationStateEvent(PlayerData_Anim.AnimationState.PickupDown);
                    Anim.ChangeAnimationState(PlayerData_Anim.AnimationState.PickupDown);
                }

                else
                {
                    Anim.ChangeAnimationStateEvent(PlayerData_Anim.AnimationState.PickUpFront);
                    Anim.ChangeAnimationState(PlayerData_Anim.AnimationState.PickUpFront);
                }
            }
            #endregion

            #region Grab Wall

            if (Mechanics.Carry.identifiedGameObject == null && Mechanics.Carry.identifiedTile == null)
            {
                if (Movement.WallMove.isSliding && Movement.WallMove.isClimb == false && Input.CheckInput.moveDirection.y != 1)
                {
                    Anim.ChangeAnimationState(PlayerData_Anim.AnimationState.Grab);
                }


                if (Movement.WallMove.isSliding && Movement.WallMove.isClimb)
                {
                    Anim.ChangeAnimationState(PlayerData_Anim.AnimationState.Grabclimb);
                }
            }
            #endregion
            #endregion

            #region Throwing
            if (Mechanics.Throw.throwingObject && Mechanics.Throw.throwingObjectInExecution == false)
            {
                if (Collider.Check.isSolid == true)
                {
                    Anim.ChangeAnimationStateEvent(PlayerData_Anim.AnimationState.Throw);
                    Anim.ChangeAnimationState(PlayerData_Anim.AnimationState.Throw);
                }

                else
                {
                    Anim.ChangeAnimationStateEvent(PlayerData_Anim.AnimationState.ThrowAir);
                    Anim.ChangeAnimationState(PlayerData_Anim.AnimationState.ThrowAir);
                }
            }
        }
        #endregion

    }
}
