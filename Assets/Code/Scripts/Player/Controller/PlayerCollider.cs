using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private PlayerData_Input Input;
    private PlayerData_Collider Collider;
    private PlayerData_Movement Movement;
    private PlayerData_Mechanics Mechanics;
    void Awake()
    {
        Input = GetComponent<PlayerData_Input>();
        Collider = GetComponent<PlayerData_Collider>();
        Movement = GetComponent<PlayerData_Movement>();
        Mechanics = GetComponent<PlayerData_Mechanics>();
    }

    void Update()
    {
        // Verifica colisão com o teto
        Collider.Check.isRoof = Physics2D.BoxCast(Collider.Component.col.bounds.center, Collider.Component.col.bounds.size, 0f, Vector2.up, 0.1f, Collider.Layer.solid);

        // Verifica colisão com o Ground
        Collider.Check.isGround = Physics2D.BoxCast(Collider.Component.col.bounds.center, Collider.Component.col.bounds.size, 0f, Vector2.down, 0.1f, Collider.Layer.ground);
        Collider.Time.lastOnGround = Collider.Check.isGround ? 0 : Collider.Time.lastOnGround + 0.1f;

        // Verifica colisão com o plataform
        Collider.Check.isPlatform = Physics2D.BoxCast(Collider.Component.col.bounds.center, Collider.Component.col.bounds.size, 0f, Vector2.down, 0.1f, Collider.Layer.plataform);
        Collider.Time.lastOnPlatform = Collider.Check.isGround ? 0 : Collider.Time.lastOnPlatform + 0.1f;

        // Verifica colisão com solido
        Collider.Check.isSolid = Collider.Check.isPlatform || Collider.Check.isGround;
        Collider.Time.lastOnSolid = (Collider.Check.isGround || Collider.Check.isPlatform) ? 0 : Collider.Time.lastOnSolid + 0.1f;

        // Verifica colisão com o parede direita
        Collider.Check.isWallLeft = Physics2D.BoxCast(Collider.Component.col.bounds.center, Collider.Component.col.bounds.size, 0f, Vector2.left, 0.1f, Collider.Layer.solid);
        Collider.Time.lastOnWallLeft = Collider.Check.isWallLeft ? 0 : Collider.Time.lastOnWallLeft + 0.1f;

        // Verifica colisão com o parede esquerda
        Collider.Check.isWallRight = Physics2D.BoxCast(Collider.Component.col.bounds.center, Collider.Component.col.bounds.size, 0f, Vector2.right, 0.1f, Collider.Layer.solid);
        Collider.Time.lastOnWallRight = Collider.Check.isWallRight ? 0 : Collider.Time.lastOnWallRight + 0.1f;

        // facilitar verificação na parede
        Collider.Check.isWall = Collider.Check.isWallRight || Collider.Check.isWallLeft;

        #region CheckLedge
        // Função verificar a borda da parede
        bool IsLedge(Vector2 position, float range, Vector2 direction) =>
            Collider.RaycastCheckDirection(position, range, Collider.Layer.solid, direction);

        // Determina a direção
        Vector2 direction = Input.CheckInput.facingRight ? Vector2.right : Vector2.left;

        // Verifica a borda da parede
        Collider.Check.isLedgeDown = IsLedge(Movement.WallMove.checkLedgeDown.position, Movement.WallMove.ledgeRange, direction);
        Collider.Check.isLedgeUp = IsLedge(Movement.WallMove.checkLedgeUp.position, Movement.WallMove.ledgeRange, direction);

        // Verifica borda do chao
        Collider.Check.isLedgeGround = Collider.RaycastCheckDirection(Movement.WallMove.checkLedgeGround.position, Movement.WallMove.ledgeRangeGround, Collider.Layer.solid, Vector2.down);
        #endregion
    }
}
