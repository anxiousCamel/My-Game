using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData_Collider : MonoBehaviour
{
    private PlayerData_Mechanics Mechanics;

    [Space(5)] public Components Component = new();
    [Space(5)] public Layers Layer = new();
    [Space(5)] public Tags Tag = new();
    [Space(5)] public Checks Check = new();
    [Space(5)] public Times Time = new();

    [System.Serializable]
    public class Components
    {
        public BoxCollider2D col;
        public GameObject Identifier;
         public BoxCollider2D CheckCrossedWhithPlatform;
    }

    [System.Serializable]
    public class Layers
    {
        public LayerMask solid;
        public LayerMask ground;
        public LayerMask plataform;
    }

    [System.Serializable]
    public class Tags
    {
        [ReadOnly] public String downloadable = "Downloadable";
        [ReadOnly] public String predatorTag = "Predator";
        [ReadOnly] public String platform = "Platform";
    }

    [System.Serializable]
    public class Checks
    {
        [ReadOnly] public bool isRoof;
        [Space(5)]
        [ReadOnly] public bool isPlatform;
        [ReadOnly] public bool crossedWhithPlatform;
        [Space(5)]
        [ReadOnly] public bool isGround;
        [ReadOnly] public bool isSolid;

        [Space(5)]
        [ReadOnly] public bool isWall;
        [ReadOnly] public bool isWallLeft;
        [ReadOnly] public bool isWallRight;

        [Space(5)]
        [ReadOnly] public bool isLedgeUp;
        [ReadOnly] public bool isLedgeDown;
        [ReadOnly] public bool isLedgeGround;
    }

    [System.Serializable]
    public class Times
    {
        [ReadOnly] public float lastOnGround;
        [ReadOnly] public float lastOnWallLeft;
        [ReadOnly] public float lastOnWallRight;
        [ReadOnly] public float lastOnPlatform;
        [ReadOnly] public float lastOnSolid;
    }

    void Awake()
    {
        Mechanics = GetComponent<PlayerData_Mechanics>();
        Component.col = GetComponent<BoxCollider2D>();
    }

    public bool RaycastCheckDirection(Vector2 objectCenter, float rayDistance, LayerMask layerMask, Vector2 direction)
    {
        // Desenha o raio na cena para fins de depuração
        Debug.DrawRay(objectCenter, direction * rayDistance, Color.green);

        // Executa um raio na direção especificada e retorna true se atingir algo dentro da distância especificada
        return Physics2D.Raycast(objectCenter, direction, rayDistance, layerMask);
    }

    

}

