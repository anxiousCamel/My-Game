using UnityEngine;

public class Minhoco : MonoBehaviour
{
    [Header("Componentes")]
    [ReadOnly] public AudioManager audioManager;
    [ReadOnly] public ParticleSystem particle;
    [ReadOnly] public EnemyGenerics generics;
    [ReadOnly] public Animator anim;
    [ReadOnly] public SpriteRenderer sprite;

    [Header("Particulas")]
    public ParticleSystem particleEarth;

    [Header("Animation")]
    public AnimationState currentState;
    public enum AnimationState
    {
        Idle,
        Attack
    }

    private void Awake()
    {
        audioManager = GetComponent<AudioManager>();
        generics = GetComponent<EnemyGenerics>();
        particle = GetComponentInChildren<ParticleSystem>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

    }

    public void IsAttack()
    {
        ChangeAnimationState(AnimationState.Attack);
    }

    public void ParticleEarthPlay()
    {
        particleEarth.Play();
    }

    public void IsIdle()
    {
        ChangeAnimationState(AnimationState.Idle);
    }

    private void ChangeAnimationState(AnimationState newState)
    {
        //Se o novo estado for o mesmo que o estado atual, não faz nada
        if (currentState == newState)
        {
            return;
        }

        //Inicia a animação do novo estado e atualiza o estado atual
        anim.Play(newState.ToString());
        currentState = newState;
    }
}
