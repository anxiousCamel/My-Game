using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDroped : MonoBehaviour
{

    [ReadOnly] public Item item;
    [ReadOnly] public Vector2 impulse;
    public Vector2 minImpulse;
    public Vector2 maxImpulse;

    void Start()
    {
        Rigidbody2D Body = GetComponent<Rigidbody2D>();
        impulse = new Vector2(Body.velocity.x + Random.Range(minImpulse.x, maxImpulse.x), Body.velocity.y + Random.Range(minImpulse.y, maxImpulse.y));
        Body.AddForce(impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                // Adicionar o item ao inventário
                int itemId = GetUniqueItemId(); // Implemente uma função para gerar IDs únicos
                playerInventory.AddItem(itemId, item);
                Destroy(gameObject); // Destruir o objeto após ser coletado
            }
        }
    }

    // Função para gerar ID único para o item
    private int GetUniqueItemId()
    {
        // Implemente lógica para gerar IDs únicos aqui
        return Random.Range(1000, 10000); // Exemplo simples de ID único
    }
}
