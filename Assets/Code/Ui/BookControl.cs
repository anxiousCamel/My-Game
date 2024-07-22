using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class BookControl : MonoBehaviour
{
    [ReadOnly] public bool inventoryState;
    [ReadOnly] public bool animInExecution;
    public GameObject referenceBook;
    public PlayerData_Input Input;
    public GameObject defalthOnEnable;

    #region Animation
    [Header("Anim")]
    public Animator anim;
    public AnimationState currentState;
    public enum AnimationState
    {
        BookIdle,
        BookOpen,
        BookClose,
        BookLeafThrough
    }
    #endregion

    public MaskControl maskControl;

    void Start()
    {
        if (Input == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Input = player != null ? player.GetComponent<PlayerData_Input>() : null;

            if (player == null)
            {
                Debug.LogError("Objeto Player com a tag 'Player' não encontrado.");
            }
            else if (Input == null)
            {
                Debug.LogError("Componente PlayerData_Input não encontrado no objeto Player.");
            }
        }
    }

    void Update()
    {
        if (Input.Time.lastInputInventory <= 0.1 && animInExecution == false)
        {
            StartCoroutine(ToggleInventory());
        }
    }

    private IEnumerator ToggleInventory()
    {
        if (inventoryState)
        {
            animInExecution = true;
            ChangeAnimationState(AnimationState.BookClose);
            yield return new WaitForSeconds(GetAnimationDuration(AnimationState.BookClose));
            inventoryState = false;
            referenceBook.SetActive(false);
            animInExecution = false;
        }
        else
        {
            animInExecution = true;
            referenceBook.SetActive(true);
            ChangeAnimationState(AnimationState.BookOpen);
            DefalthOnEnable();
            yield return new WaitForSeconds(GetAnimationDuration(AnimationState.BookOpen));
            inventoryState = true;
            animInExecution = false;
        }

        yield return null;
    }

    public void ChangeAnimationState(AnimationState newState)
    {
        // Se o novo estado for o mesmo que o estado atual, não faz nada
        if (currentState == newState)
        {
            return;
        }

        // Inicia a animação do novo estado e atualiza o estado atual
        anim.Play(newState.ToString());
        currentState = newState;
    }

    public float GetAnimationDuration(AnimationState state)
    {
        // Obtém as informações do estado atual da animação
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        // Obtém a duração da animação para o estado fornecido
        float duration = stateInfo.length;

        // Retorna a duração da animação
        return duration;
    }

    public void DefalthOnEnable()
    {
        defalthOnEnable.SetActive(true);
        maskControl.PageLoaded = defalthOnEnable;
    }
}