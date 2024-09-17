using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    private PersistentObject instance;

    private void Awake()
    {
        if (instance == null)
        {
            // Se não existe outra instância, esta se torna a única e não é destruída
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Se já existe uma instância, destrua a duplicata
            Destroy(gameObject);
        }
    }
}
