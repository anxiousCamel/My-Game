using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    private PlayerData_Movement Movement;
    private PlayerData_Collider Collider;
    private PlayerData_Physics Physic;
    private PlayerData_Input Input;


    void Awake()
    {
        Movement = GetComponent<PlayerData_Movement>();
        Collider = GetComponent<PlayerData_Collider>();
        Physic = GetComponent<PlayerData_Physics>();
        Input = GetComponent<PlayerData_Input>();
    }

    private void FixedUpdate() 
    { 
        if(Movement.Controllers.canMove == true)
        {
            Run();
            Friction();
        }
    }

    private void Friction()
    {
        if(Collider.Check.isSolid && Mathf.Abs(Input.CheckInput.moveDirection.x) < 0.01f)
        {
            Physic.Component.body.AddForce(Vector2.right * - Movement.AmountFriction(), ForceMode2D.Impulse);
        }
    } 

    private void Run() 
    { 
        Physic.Component.body.AddForce(Movement.CalculateMovement() * Vector2.right, ForceMode2D.Force); 
    }
}
