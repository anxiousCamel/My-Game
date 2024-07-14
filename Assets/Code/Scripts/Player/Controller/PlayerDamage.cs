using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [ReadOnly] public PlayerData_Mechanics Mechanics;
    [ReadOnly] public PlayerData_Collider Collider;
    [ReadOnly] public PlayerData_Movement Movement;
    [ReadOnly] public PlayerData_Physics Physics;
    [ReadOnly] public PlayerData_Input Input;

    [Header("ShakeCamera")]
    [ReadOnly] public CameraShake cameraShake;
    public float shakeDuration;
    public float amplitude;
    void Awake()
    {
        Mechanics = GetComponent<PlayerData_Mechanics>();
        Collider = GetComponent<PlayerData_Collider>();
        Movement = GetComponent<PlayerData_Movement>();
        Physics = GetComponent<PlayerData_Physics>();
        Input = GetComponent<PlayerData_Input>();

        GetCamera();
    }

    private void GetCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            cameraShake = mainCamera.GetComponentInChildren<CameraShake>();
        }

        // Certifique-se de que a referência foi encontrada
        if (cameraShake == null)
        {
            Debug.LogError("CameraShake script not found in camera's children.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Colission(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Colission(collision.collider);
    }

    void Colission(Collider2D colission)
    {
        // Verifica se o objeto colidido tem a tag "Predator"
        if (colission.CompareTag(Collider.Tag.predatorTag))
        {
            Hurt(colission);

            if (Mechanics.Carry.identifiedGameObject != null || Mechanics.Carry.identifiedTile != null)
            {
                // Inicia particula
                Mechanics.Hurt.particleDestroyHolderItem.Play();

                // Desativa o sprite e o collider
                Mechanics.CleanObject();
            }
        }
    }

    void Hurt(Collider2D enemy)
    {
        Mechanics.Hurt.isHurt = true;
        Movement.Controllers.canMove = false;

        cameraShake.ShakeCamera(amplitude,shakeDuration);

        Physics.ResetVelocity();
        Vector3 enemyPosition = enemy.GetComponent<Transform>().position;
        Vector2 direction = enemyPosition - transform.position;

        Vector2 hurtForce = direction.x >= 0.01
            ? new Vector2(-Mechanics.Hurt.hurtForce.x, Mechanics.Hurt.hurtForce.y)
            : new Vector2(Mechanics.Hurt.hurtForce.x, Mechanics.Hurt.hurtForce.y);

        Physics.Component.body.AddForce(hurtForce, ForceMode2D.Impulse);
    }

    void FinishHurt()
    {
        Mechanics.Hurt.isHurt = false;
        Movement.Controllers.canMove = true;
    }

}
