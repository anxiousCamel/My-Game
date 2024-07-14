using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyGenerics : MonoBehaviour
{
    [Header("Variaveis")]
    [ReadOnly] public string[] cores = { "f68187", "f9e6cf", "fdd2ed", "ffffff", "92a1b9", "99e65f" };
    [ReadOnly] public String LaunchedTag = "Launched";
    [ReadOnly] public bool canMove;


    private void Awake() 
    {
        canMove = true;
    }
    
    public GameObject GetPlayer()
    {
        return GameObject.FindWithTag("Player");
    }

    public Vector2 GetRandomPosition(Collider2D colliderArea)
    {
        // Calcula os limites do Collider2D especificado
        Bounds bounds = CalculateBounds(colliderArea);
        // Gera uma posição aleatória dentro dos limites calculados
        return GenerateRandomPosition(bounds);
    }

    public Color RandomColor()
    {
        // Seleciona uma cor aleatória da lista
        string corHex = cores[UnityEngine.Random.Range(0, cores.Length)];

        // Converte a cor hexadecimal em UnityEngine.Color
        Color cor = HexToColor(corHex);
        return cor;
    }

    public float RandomPitch()
    {
        return UnityEngine.Random.Range(1f, 3f);
    }

    Color HexToColor(string hex)
    {
        // Remove o "#" do início da string, se presente    
        hex = hex.Replace("#", "");

        // Converte a string hexadecimal em valores RGB
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        // Retorna a cor UnityEngine.Color
        return new Color32(r, g, b, 255);
    }

    Bounds CalculateBounds(Collider2D colliderArea)
    {
        if (colliderArea != null)
        {
            // Obtém os limites do Collider2D
            return colliderArea.bounds;
        }
        else
        {
            Debug.LogError("Collider2D não encontrado!");
            return new Bounds();
        }
    }

    Vector2 GenerateRandomPosition(Bounds bounds)
    {
        // Calcula os limites da área
        float left = bounds.min.x;
        float right = bounds.max.x;
        float top = bounds.max.y;
        float bottom = bounds.min.y;

        // Gera uma posição aleatória dentro da área delimitada
        float randomX = UnityEngine.Random.Range(left, right);
        float randomY = UnityEngine.Random.Range(bottom, top);

        return new Vector2(randomX, randomY);
    }

    public bool RaycastCheckDirection(Vector2 objectCenter, float rayDistance, LayerMask layerMask, Vector2 direction)
    {
        // Desenha o raio na cena para fins de depuração
        Debug.DrawRay(objectCenter, direction * rayDistance, Color.green);

        // Executa um raio na direção especificada e retorna true se atingir algo dentro da distância especificada
        return Physics2D.Raycast(objectCenter, direction, rayDistance, layerMask);
    }

    public bool CapsuleCastCheckDirection(CapsuleCollider2D col, LayerMask layer, Vector2 direction)
    {
        Vector2 capsuleSize = new Vector2(col.bounds.size.x, col.bounds.size.y);
        return Physics2D.CapsuleCast(col.bounds.center, capsuleSize, CapsuleDirection2D.Vertical, 0f, direction, 0.1f, layer);
    }
    public bool BoxCastCheckDirection(BoxCollider2D col, LayerMask layer, Vector2 direction)
    {
        return Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, direction, 0.1f, layer);
    }

    public bool InRange(Vector2 myPosition, Vector2 targetPosition, float range)
    {
        return Vector2.Distance(myPosition, targetPosition) <= range;
    }
}
