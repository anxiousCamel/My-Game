using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData_Movement : MonoBehaviour
{
    // class
    [Space(5)] public run Run = new();
    [Space(5)] public jump Jump = new();
    [Space(5)] public controllers Controllers = new();
    [Space(5)] public wallMove WallMove = new();


    [System.Serializable]
    public class controllers
    {
        public bool canMove = true;
    }

    [System.Serializable]
    public class run
    {
        public float runMaxSpeed; //Target speed we want the player to reach.
        public float debuffRunMaxSpeed;
        public float runAcceleration; //Time (approx.) time we want it to take for the player to accelerate from 0 to the runMaxSpeed.
        public float runAccelAmount; //The actual force (multiplied with speedDiff) applied to the player.
        public float runDecceleration; //Time (approx.) we want it to take for the player to accelerate from runMaxSpeed to 0.
        public float runDeccelAmount; //Actual force (multiplied with speedDiff) applied to the player .
        public float frictionAmount;
        [Space(10)]
        [Range(0.01f, 1)] public float accelInAir; //Multipliers applied to acceleration rate when airborne.
        [Range(0.01f, 1)] public float deccelInAir;
        [Space(10)]
        public bool doConserveMomentum;
    }
    [System.Serializable]
    public class jump
    {
        public float ignorePlataform;
        public float jumpVelocity;
        public float debuffJumpVelocity;
        public float fallMultiplier;
        public float lowJumpMultiplier;
        public bool isJumping;
        public float coyoteJump;
        public bool jumpRequest;
        public bool jumpWallRequest;
        public bool particleJump;
        public bool particleWallJump;
        public bool trampolineJump;
    }

    [System.Serializable]
    public class wallMove
    {
        public Vector2 jumpWallVelocity;
        public bool isSliding;
        public bool isClimb;
        public float SpeedWallClimb;
        public Transform checkLedgeUp;
        public Transform checkLedgeDown;
        public Transform checkLedgeGround;
        public float ledgeRange;
        public float ledgeRangeGround;
    }

    // Referencias
    private PlayerData_Mechanics Mechanics;
    private PlayerData_Collider Collider;
    private PlayerData_Physics Physic;
    private PlayerData_Input Input;

    void Awake()
    {
        Mechanics = GetComponent<PlayerData_Mechanics>();
        Collider = GetComponent<PlayerData_Collider>();
        Physic = GetComponent<PlayerData_Physics>();
        Input = GetComponent<PlayerData_Input>();
    }

    public float CalculateMovement()
    {
        //Calcula a direção em que queremos nos mover e nossa velocidade desejada
        float targetSpeed = Mechanics.Carry.identifiedGameObject != null || Mechanics.Carry.identifiedTile != null ? Input.CheckInput.moveDirection.x * Run.debuffRunMaxSpeed : Input.CheckInput.moveDirection.x * Run.runMaxSpeed;

        //Calcula a diferença entre a velocidade atual e a velocidade desejada
        float speedDif = targetSpeed - Physic.Component.body.velocity.x;

        //Obtém um valor de aceleração com base em se estamos acelerando ou tentando desacelerar. Bem como aplicar um multiplicador se estivermos no ar.
        float accelRate;
        if (Collider.Check.isSolid) accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Run.runAccelAmount : Run.runDeccelAmount;
        else accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Run.runAccelAmount * Run.accelInAir : Run.runDeccelAmount * Run.deccelInAir;

        //Não vamos diminuir a velocidade do jogador se ele estiver se movendo na direção desejada, mas a uma velocidade maior do que sua velocidade máxima.
        if (Run.doConserveMomentum && Mathf.Abs(Physic.Component.body.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(Physic.Component.body.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && Collider.Time.lastOnGround < 0)
        { accelRate = 0; }

        return speedDif * accelRate;
    }


    public float AmountFriction()
    {
        float amount = Mathf.Min(Mathf.Abs(Physic.Component.body.velocity.x), Mathf.Abs(Run.frictionAmount));

        return amount *= Mathf.Sign(Physic.Component.body.velocity.x);
    }

    public void CanMove()
    {
        //Ativa e Desativa o movimento de acordo com o ultimo estado de canMove ao chamar a função
        Controllers.canMove = (Controllers.canMove == false) ? true : false;
    }

}
