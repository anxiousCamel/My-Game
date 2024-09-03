using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData_Anim : MonoBehaviour
{
    public Animator anim;
    public Animator animEvent;
    public AnimationState currentState;
    public AnimationState currentStateEvent;
    public enum AnimationState
    {
        Idle,
        IdleCarry,
        Run,
        RunCarry,
        Jump,
        JumpCarry,
        Eat,
        Fall,
        FallCarry,
        PickupDown,
        PickUpFront,
        Throw,
        ThrowAir,
        Rope,
        RopeAir,
        RopeUp,
        RopeUpAir,
        Advance,
        AdvanceUp,
        LedgeClimb,
        Grab,
        Grabclimb,
        Hurt
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

    public void ChangeAnimationStateEvent(AnimationState newStateEvent)
    {
        //Inicia a animação do novo estado e atualiza o estado atual
        animEvent.Play(newStateEvent.ToString());
        currentStateEvent = newStateEvent;
    }
}
