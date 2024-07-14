using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Scripts Reference")]
    [ReadOnly] EnemyGenerics generics;

    [Space(5)]
    [Header("Materiais")]
    public Material normalMaterial;
    public Material blinkMaterial;

    [Space(5)]
    [Header("Forces")]
    public Vector2 hurtImpulse;

    void Start()
    {
        generics = GetComponent<EnemyGenerics>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckCollision(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other);
    }

    private void CheckCollision(Collider2D collider)
    {
        // Verifica se o objeto colidido tem a tag "Predator"
        if (collider.CompareTag(generics.LaunchedTag))
        {
            Knockback(collider);
            collider.GetComponent<ObjectLaunched>().CreateParticlesAndDestroy();
            BlinkMaterial();
            Invoke("DestroyObject", 0.2f);
        }
    }

    private void Knockback(Collider2D objectLauched)
    {
        generics.canMove = false;
        if(GetComponent<Rigidbody2D>() != null)
        {
            Rigidbody2D body = GetComponent<Rigidbody2D>();
            body.velocity = Vector2.zero;

            Vector3 objectLauchedPosition = objectLauched.GetComponent<Transform>().position;
            Vector2 direction = objectLauchedPosition - transform.position;

            Vector2 hurtForce = direction.x >= 0.01
                ? new Vector2(-hurtImpulse.x, hurtImpulse.y)
                : new Vector2(hurtImpulse.x, hurtImpulse.y);

            body.AddForce(hurtForce, ForceMode2D.Impulse);
        }
    }

    public void RestoreMaterial()
    {
        GetComponent<SpriteRenderer>().material = normalMaterial;
    }


    public void BlinkMaterial()
    {
        GetComponent<SpriteRenderer>().material = blinkMaterial;
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
