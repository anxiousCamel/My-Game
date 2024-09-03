using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AttachToParentCompositeCollider : MonoBehaviour
{
    private void Start()
    {
        // Busca o CompositeCollider2D no objeto pai
        CompositeCollider2D parentCompositeCollider = GetComponentInParent<CompositeCollider2D>();
        
        if (parentCompositeCollider != null)
        {
            // Obtém o Collider2D do GameObject atual
            Collider2D thisCollider = GetComponent<Collider2D>();

            // Define o Collider2D para ser usado pelo CompositeCollider2D do pai
            if (thisCollider != null)
            {
                thisCollider.usedByComposite = true;
                // Força a atualização do CompositeCollider2D
                parentCompositeCollider.geometryType = CompositeCollider2D.GeometryType.Polygons;
                parentCompositeCollider.GenerateGeometry();

                Debug.Log("Collider adicionado ao CompositeCollider2D do objeto pai e forçado a atualizar.");
            }
            else
            {
                Debug.LogError("Nenhum Collider2D encontrado neste GameObject.");
            }
        }
        else
        {
            Debug.LogError("Nenhum CompositeCollider2D encontrado no objeto pai.");
        }
    }
}
