using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnPlatform : MonoBehaviour
{


    private PlayerData_Collider Collider;

    void Awake()
    {
        Collider = GetComponent<PlayerData_Collider>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Collider.Tag.platform))
        {
            transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Collider.Tag.platform))
        {
            transform.parent = null;
        }
    }

}
