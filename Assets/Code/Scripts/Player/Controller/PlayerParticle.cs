using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticle : MonoBehaviour
{
    [ReadOnly] public PlayerData_Input Input;
    [ReadOnly] public PlayerData_Collider Collider;
    [ReadOnly] public PlayerData_Movement Movement;
    [ReadOnly] public PlayerData_Particles particles;
    [ReadOnly] public PlayerData_Mechanics Mechanics;
    void Awake()
    {
        Input = GetComponent<PlayerData_Input>();
        Collider = GetComponent<PlayerData_Collider>();
        Movement = GetComponent<PlayerData_Movement>();
        particles = GetComponent<PlayerData_Particles>();
        Mechanics = GetComponent<PlayerData_Mechanics>();
    }
    private void Update() 
    {
        if(Movement.WallMove.isSliding == true && Input.CheckInput.moveDirection.y <= -1)
        {
            particles.wallSlideDust.Emit(1);
            particles.wallSlideDust.Play();
        }

        if(Movement.WallMove.isClimb)
        {
            particles.wallClimbDust.Emit(1);
            particles.wallClimbDust.Play();
        }

        if(Movement.Jump.particleJump)
        {
            particles.dustJump.Emit(1);
            particles.dustJump.Play();
            Movement.Jump.particleJump = false;
        }

        if(Movement.Jump.particleWallJump)
        {
            particles.dustWallJump.Emit(1);
            particles.dustWallJump.Play();
            Movement.Jump.particleWallJump = false;
        }
    }
}
