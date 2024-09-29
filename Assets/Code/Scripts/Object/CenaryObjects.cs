using System.Collections;
using System.Collections.Generic;
using Inventory.Model;
using UnityEngine;
using UnityEngine.Tilemaps;
public class CenaryObjects : MonoBehaviour
{
    [field: SerializeField]
    public ItemSO InventoryItem { get; private set; }
    [ReadOnly] public static IdentifyTile identifyTile;
    [ReadOnly] public BoxCollider2D col;
    [ReadOnly] public Animator anim;

    [ReadOnly] public AnimationState currentState;
    [ReadOnly] public bool Shake;
    public enum AnimationState
    {
        Idle,
        Shake
    };

    public ParticleSystem bushLeafParticle;
    public Vector3 identifyOffSet;



    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        #region Anim
        if (!Shake)
        {
            Idle();
        }

        else
        {
            Shaker();
        }
        #endregion

        if (IdentifyTileCache.canIdentify == true)
        {
            if (!CheckTileBelow())
            {
                Destroy();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            Shake = true;
        }
    }

    public void ChangeAnimationState(AnimationState newState)
    {
        //Se o novo estado for o mesmo que o estado atual, não faz nada
        if (currentState == newState)
        { return; }

        //Inicia a animação do novo estado e atualiza o estado atual
        anim.Play(newState.ToString());
        currentState = newState;
    }

    public void Shaker()
    {
        ChangeAnimationState(AnimationState.Shake);
    }

    public void Idle()
    {
        Shake = false;
        ChangeAnimationState(AnimationState.Idle);
    }

    public void Particle()
    {
        bushLeafParticle.Play();
    }

    //! !!!!!!!!!! TRANSFORMAR ISSO AQUI EM UMA COROUTINE !!!!!!!!!! 
    public void Destroy()
    {
        // drop object
       DropItem();
        
        // particulas
        bushLeafParticle.Play();
        // destroy
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 2);

        IdentifyTileCache.canIdentify = false;
    }

    void DropItem()
    {
        int minQuantity = InventoryItem.MinQuantityDrop;
        int maxQuantity = InventoryItem.MaxQuantityDrop;
        float probability = InventoryItem.ProbabilityDrop;

        // Lógica para calcular se o item deve ser dropado com base na probabilidade
        if (Random.value <= probability)
        {
            int quantity = Random.Range(minQuantity, maxQuantity + 1);
            for (int i = 0; i < quantity; i++)
            {
                GameObject gameObjectInstantiate = Instantiate(InventoryItem.PrefabDrop, transform.position, Quaternion.identity);
                //gameObjectInstantiate.GetComponent<ItemDroped>().item = InventoryItem;
            }
        }
    }


    bool CheckTileBelow()
    {
        if (identifyTile.tileMaps.Count > 0)
        {
            foreach (Tilemap tileMap in identifyTile.tileMaps)
            {
                // Converte a posição do transform para uma posição de tilemap usando cada tilemap na lista
                Vector3Int tilePosition = tileMap.WorldToCell(transform.position);

                // Ajusta a posição para verificar o tile abaixo
                Vector3Int tileBelowPosition = new Vector3Int(tilePosition.x, tilePosition.y - 1, tilePosition.z);

                // Verifica se o tile ou GameObject está presente na posição abaixo
                IdentifyTile.IdentifiedObjectResult result = identifyTile.GetTileOrGameObject(tileBelowPosition);

                if (result != null)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
