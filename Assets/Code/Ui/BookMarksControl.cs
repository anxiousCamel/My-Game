using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookMarksControl : MonoBehaviour
{
    [ReadOnly] public RectTransform rectTransform;
    private bool isMouseOver = false;
    public MaskControl maskControl;
    public GameObject gameObjectOnEnable;

    #region Animation
    [Header("Anim")]
    public Animator anim;
    public AnimationState currentState;
    public enum AnimationState
    {
        BookMarkIdle,
        BookMarkReturn,
        BookMarkAdvance,
        BookLeafThrough,
        BookMarkAdvancedDefalth,
        BookMarkAdvancedIdle
    }
    #endregion


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Verifica se o mouse está dentro dos limites do RectTransform
        if (IsMouseOver())
        {
            if (!isMouseOver)
            {
                // Mouse entrou na área
                isMouseOver = true;
                if (maskControl.PageLoaded != gameObjectOnEnable)
                {
                    ChangeAnimationState(AnimationState.BookMarkAdvance);
                }
            }

            // Verifica se houve um clique do mouse
            if (Input.GetMouseButtonDown(0))
            {
                if (maskControl.PageLoaded != gameObjectOnEnable)
                {
                    OnBookmarkClick();
                }
            }
        }
        else
        {
            if (isMouseOver)
            {
                // Mouse saiu da área
                isMouseOver = false;
                if (maskControl.PageLoaded != gameObjectOnEnable)
                {
                    ChangeAnimationState(AnimationState.BookMarkReturn);
                }
            }
        }
        
        if (maskControl.PageLoaded == gameObjectOnEnable)
        {
            ChangeAnimationState(AnimationState.BookMarkAdvancedIdle);
        }
        /*
        else
        {

            ChangeAnimationState(AnimationState.BookMarkIdle);

        }
        */
    }

    private bool IsMouseOver()
    {
        // Verifica se o ponteiro do mouse está dentro dos limites do RectTransform
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out localPoint);
        bool isInside = rectTransform.rect.Contains(localPoint);
        return isInside;
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

    private void OnBookmarkClick()
    {
        maskControl.FadeOut(gameObjectOnEnable);
        ChangeAnimationState(AnimationState.BookMarkAdvancedDefalth);
    }
}
