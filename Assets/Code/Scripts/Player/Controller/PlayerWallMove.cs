using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallMove : MonoBehaviour
{
    private PlayerData_Mechanics Mechanics;
    private PlayerData_Movement Movement;
    private PlayerData_Collider Collider;
    private PlayerData_Physics Physic;
    private PlayerData_Input Input;

    void Awake()
    {
        Mechanics = GetComponent<PlayerData_Mechanics>();
        Movement = GetComponent<PlayerData_Movement>();
        Collider = GetComponent<PlayerData_Collider>();
        Physic = GetComponent<PlayerData_Physics>();
        Input = GetComponent<PlayerData_Input>();
    }

    private void Update()
    {

        bool isSlidingRight = Collider.Check.isWallRight 
            && Input.CheckInput.moveDirection.x == 1 
            && Input.CheckInput.facingRight 
            && !Collider.Check.isSolid 
            && Physic.BodyVelocity().y < 1
            && Collider.Check.isLedgeDown
            && Collider.Check.isLedgeUp
            && !Collider.Check.isLedgeGround
            || Movement.WallMove.isClimb == true;

        bool isSlidingLeft = Collider.Check.isWallLeft 
            && Input.CheckInput.moveDirection.x == -1 
            && !Input.CheckInput.facingRight 
            && !Collider.Check.isSolid 
            && Physic.BodyVelocity().y < 1 
            && Collider.Check.isLedgeDown
            && Collider.Check.isLedgeUp
            && !Collider.Check.isLedgeGround
            || Movement.WallMove.isClimb == true;

        Movement.WallMove.isSliding = isSlidingRight || isSlidingLeft;


        float verticalInput = Input.CheckInput.moveDirection.y;

        if (Movement.WallMove.isSliding && Mechanics.Carry.identifiedGameObject == null && Mechanics.Carry.identifiedTile == null)
        {
            // Subir e Descer
            if (verticalInput != 0)
            {
                float targetVelocityY = verticalInput * Movement.WallMove.SpeedWallClimb;
                Physic.Component.body.velocity = Vector2.up * targetVelocityY;

                // Verifica se está subindo para marcar como escalada
                if (verticalInput == 1)
                {
                    Movement.WallMove.isClimb = true;
                }
            }
            else
            {
                Physic.FreezePosition();
            }
        }

        // Reseta a flag de escalada se não estiver subindo
        if (verticalInput != 1 || Collider.Check.isWall == false)
        {
            Movement.WallMove.isClimb = false;
        }

    }
}
