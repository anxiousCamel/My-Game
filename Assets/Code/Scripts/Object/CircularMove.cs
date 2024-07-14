using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMove : MonoBehaviour
{
    [Header("Movement")]
    public float radius = 5f;
    public float angularSpeed = 1f;
    [ReadOnly] public float angle;
    private int segments = 100;
    private Vector3 center;

    void Start()
    {
        // Inicializar o centro do círculo como a posição inicial do objeto
        center = transform.position;
    }

    void Update()
    {
        // Atualizar o ângulo
        angle += angularSpeed * Time.deltaTime;

        // Calcular a nova posição
        float x = center.x + radius * Mathf.Cos(angle);
        float y = center.y + radius * Mathf.Sin(angle);
        float z = center.z; // Se for 3D, mantenha a posição z do objeto

        // Atualizar a posição do objeto
        transform.position = new Vector3(x, y, z);
    }

    void OnDrawGizmos()
    {
        // Definir a cor dos gizmos
        Gizmos.color = Color.red;

        // Desenhar o círculo
        if (!Application.isPlaying)
        {
            // No modo de edição, o centro é a posição atual do objeto
            center = transform.position;
        }

        DrawCircle(center, radius, segments);
    }

    void DrawCircle(Vector3 center, float radius, int segments)
    {
        float angleStep = 2 * Mathf.PI / segments;
        Vector3 prevPoint = center + new Vector3(radius, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            float currentAngle = i * angleStep;
            Vector3 currentPoint = center + new Vector3(radius * Mathf.Cos(currentAngle), radius * Mathf.Sin(currentAngle), 0);
            Gizmos.DrawLine(prevPoint, currentPoint);
            prevPoint = currentPoint;
        }
    }
}
