using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDroped : MonoBehaviour
{
    [ReadOnly] public Vector2 impulse;
    public Vector2 minImpulse;
    public Vector2 maxImpulse;

    void Start()
    {
        Rigidbody2D Body = GetComponent<Rigidbody2D>();
        impulse = new Vector2(Body.velocity.x + Random.Range(minImpulse.x, maxImpulse.x), Body.velocity.y + Random.Range(minImpulse.y, maxImpulse.y));
        Body.AddForce(impulse);
    }
}
