using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData_Physics : MonoBehaviour
{

    [Space(5)] public Components Component = new();
    [Space(5)] public Physics Physic = new();



    public float normalGravity;


    [System.Serializable]
    public class Components
    {
        public Rigidbody2D body;
        public GameObject playerGameObject;
    }

    void Start()
    {
        normalGravity = Component.body.gravityScale;
    }

    public Vector2 BodyVelocity()
    {
        float axisX = Mathf.Abs(Component.body.velocity.x);
        float axisY = Component.body.velocity.y;

        return new Vector2(axisX, axisY);
    }

    public void ResetVelocity()
    {
        Component.body.velocity = Vector2.zero;
        Component.body.AddForce(Vector2.zero);
    }

    public void ResetVelocityY()
    {
        Component.body.velocity = new Vector2(Component.body.velocity.x, 0);
        Component.body.AddForce(new Vector2(Component.body.velocity.x, 0));
    }

    public void ResetVelocityX()
    {
        Component.body.velocity = new Vector2(0, Component.body.velocity.y);
        Component.body.AddForce(new Vector2(0, Component.body.velocity.y));
    }

    public Vector2 PlayerPosition()
    {
        Vector2 PositionPlayer = new(Component.body.position.x, Component.body.position.y);
        return PositionPlayer;
    }

    public void FreezePosition()
    {
        ResetVelocity();
        Component.body.gravityScale = 0;
    }
}
