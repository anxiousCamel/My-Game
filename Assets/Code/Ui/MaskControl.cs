using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskControl : MonoBehaviour
{
    [ReadOnly] public GameObject PageLoaded;
    [ReadOnly] public GameObject PageToLoad;
    public BookControl bookControl;

    #region Animation
    [Header("Anim")]
    public Animator anim;
    public AnimationState currentState;
    public enum AnimationState
    {
        Mask_FadeIn,
        Mask_FadeOut,
        mask,
    }
    #endregion

    public void FadeIn(GameObject page)
    {
        PageToLoad = page;

        if (PageToLoad != null)
        {
            PageToLoad.SetActive(true);
        }

        ChangeAnimationState(AnimationState.Mask_FadeIn);
    }

    public void FadeOut(GameObject page)
    {
        PageToLoad = page;
        ChangeAnimationState(AnimationState.Mask_FadeOut);
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

    // Esse método deve ser chamado no fim da animação de fade out
    public void OnFadeOutComplete()
    {
        if (PageLoaded != null)
        {
            PageLoaded.SetActive(false);
        }

        // Iniciar a corrotina para esperar o fim da animação de folhear a página
        StartCoroutine(WaitForPageFlip());
    }

    private IEnumerator WaitForPageFlip()
    {
        // Muda o estado de animação do livro para folhear a página
        bookControl.ChangeAnimationState(BookControl.AnimationState.BookLeafThrough);

        // Aguarda o término da animação de folhear a página
        yield return new WaitForSeconds(bookControl.GetAnimationDuration(BookControl.AnimationState.BookLeafThrough));

        // Após a animação de folhear a página, começa o fade in para a nova página
        FadeIn(PageToLoad);
    }

    public void OnFadeInComplete()
    {
        PageLoaded = PageToLoad;
        PageToLoad = null;

        ChangeAnimationState(AnimationState.mask);
        bookControl.ChangeAnimationState(BookControl.AnimationState.BookIdle);
    }
}