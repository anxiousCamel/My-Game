using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private PlayerData_Mechanics Mechanics;
    private PlayerData_Movement Movement;
    private PlayerData_Collider Collider;
    private PlayerData_Physics Physics;
    private PlayerData_Input Input;
    private PlayerData_Stats Stats;

    void Awake()
    {
        Mechanics = GetComponent<PlayerData_Mechanics>();
        Movement = GetComponent<PlayerData_Movement>();
        Collider = GetComponent<PlayerData_Collider>();
        Physics = GetComponent<PlayerData_Physics>();
        Input = GetComponent<PlayerData_Input>();
        Stats = GetComponent<PlayerData_Stats>();
    }

    void Update()
    {
        if (Input.CheckInput.moveDirection.y == -1 && Input.CheckInput.inputJump && Collider.Check.isPlatform)
        {
            StartCoroutine(IgnoreLayers());
        }

        // canJump
        if (CanJump() && Input.CheckInput.inputJump)
        {
            Movement.Jump.isJumping = true;
            Movement.Jump.jumpRequest = true;
        }

        // Wall Jump
        if (CanJumpWall() && Input.CheckInput.inputJump && Stats.stamina >= 0)
        {
            Movement.Jump.isJumping = true;
            Movement.Jump.jumpWallRequest = true;
        }

        // devolver jump
        if (Movement.Jump.isJumping && Collider.Check.isSolid == true)
        { Movement.Jump.isJumping = false; Movement.Jump.trampolineJump = false; }


        // BetterJump
        if (Physics.Component.body.velocity.y < 0)
        {
            Physics.Component.body.gravityScale = Movement.Jump.fallMultiplier;
        }

        else if (Physics.Component.body.velocity.y > 0 && Input.CheckInput.keepPressingJump == false && Movement.Jump.trampolineJump == false)
        {
            Physics.Component.body.gravityScale = Movement.Jump.lowJumpMultiplier;
        }

        else
        {
            Physics.Component.body.gravityScale = Physics.normalGravity;
        }

    }

    void FixedUpdate()
    {
        // Input
        if (Movement.Jump.jumpRequest)
        { Jump(); }

        if (Movement.Jump.jumpWallRequest)
        { WallJump(); }
    }

    private void Jump()
    {
        Movement.Jump.particleJump = true;
        Movement.Jump.jumpRequest = false;
        if (Collider.Check.isSolid == false)
        {
            Physics.ResetVelocityY();
        }

        Physics.Component.body.AddForce(Vector2.up * (Mechanics.Carry.identifiedGameObject != null || Mechanics.Carry.identifiedTile != null ? Movement.Jump.debuffJumpVelocity : Movement.Jump.jumpVelocity), ForceMode2D.Impulse);

    }

    public void WallJump()
    {
        Movement.Jump.particleWallJump = true;
        Movement.WallMove.isClimb = false;
        Movement.Jump.jumpWallRequest = false;

        Physics.ResetVelocity();
        if (Collider.Check.isWall && Mechanics.Carry.identifiedGameObject == null && Mechanics.Carry.identifiedTile == null)
        {
            Vector2 jumpForce = Input.CheckInput.facingRight
                ? new Vector2(-Movement.WallMove.jumpWallVelocity.x, Movement.WallMove.jumpWallVelocity.y)
                : new Vector2(Movement.WallMove.jumpWallVelocity.x, Movement.WallMove.jumpWallVelocity.y);

            Physics.Component.body.AddForce(jumpForce, ForceMode2D.Impulse);
        }

        Stats.RemoveStamina(Movement.Jump.costWallJump);
    }

    private bool CanJump()
    {
        return Collider.Time.lastOnSolid <= Movement.Jump.coyoteJump && Input.Time.lastInputJump <= 0 && Movement.Controllers.canMove == true && !Collider.Check.isRoof;
    }

    private bool CanJumpWall()
    {
        return Collider.Check.isWall && Input.Time.lastInputJump <= 0 && Movement.Controllers.canMove == true && Collider.Check.isSolid == false && !Collider.Check.isRoof && Mechanics.Carry.identifiedGameObject == null && Mechanics.Carry.identifiedTile == null;
    }

    IEnumerator IgnoreLayers()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        yield return new WaitForSeconds(Movement.Jump.ignorePlataform);
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }

}
