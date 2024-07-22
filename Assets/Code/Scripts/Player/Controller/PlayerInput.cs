using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    private PlayerData_Input Input;
    private PlayerData_Movement Movement;

    void Awake()
    {
        Input = GetComponent<PlayerData_Input>();
        Movement = GetComponent<PlayerData_Movement>();
    }

    void Update()
    {
        #region MoveDirection   
        Input.CheckInput.moveDirection = new Vector2(UnityEngine.Input.GetAxisRaw("Horizontal"), UnityEngine.Input.GetAxisRaw("Vertical"));
        #endregion
        
        #region CanMove   
        if (Movement.Controllers.canMove == true)
        {
            #region Flip
            if (Input.CheckInput.moveDirection.x >= 0.1)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                Input.CheckInput.facingRight = true;
                Input.CheckInput.direction = 1;
            }

            else if (Input.CheckInput.moveDirection.x <= -0.1)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                Input.CheckInput.facingRight = false;
                Input.CheckInput.direction = -1;
            }
            #endregion


            #region Jump
            Input.CheckInput.inputJump = UnityEngine.Input.GetButtonDown("Jump");
            Input.CheckInput.keepPressingJump = UnityEngine.Input.GetButton("Jump");
            Input.Time.lastInputJump = Input.CheckInput.inputJump ? 0 : Input.Time.lastInputJump + 0.1f;
            #endregion

            #region Object Interaction
            Input.CheckInput.inputObjectInteraction = UnityEngine.Input.GetButtonDown("ObjectInteraction");
            Input.CheckInput.keepPressingObjectInteraction = UnityEngine.Input.GetButton("ObjectInteraction");

            Input.Time.lastInputObjectInteraction = Input.CheckInput.inputObjectInteraction ? 0 : Input.Time.lastInputObjectInteraction + 0.1f;
            Input.Time.durationInputObjectInteraction = Input.CheckInput.keepPressingObjectInteraction ? Input.Time.durationInputObjectInteraction + 0.1f : 0;
            #endregion
        }
        #endregion

        Input.CheckInput.releasedPressingObjectInteraction = UnityEngine.Input.GetButtonUp("ObjectInteraction");
        Input.Time.lastInputUpObjectInteraction = Input.CheckInput.releasedPressingObjectInteraction ? 0 : Input.Time.lastInputUpObjectInteraction + 0.1f;

        #region Inventory
            Input.CheckInput.inputInventory = UnityEngine.Input.GetButtonDown("Inventory");
            Input.Time.lastInputInventory = Input.CheckInput.inputInventory ? 0 : Input.Time.lastInputInventory + 0.1f;
        #endregion
    }
}