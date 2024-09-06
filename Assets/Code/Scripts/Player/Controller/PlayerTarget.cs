using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : MonoBehaviour
{
    private PlayerData_Input Input;

    private PlayerData_Mechanics Mechanics;
    private PlayerData_Stats Stats;

    private void Awake()
    {
        Stats = GetComponent<PlayerData_Stats>();
        Input = GetComponent<PlayerData_Input>();
        Mechanics = GetComponent<PlayerData_Mechanics>();
    }

    private void Update()
    {
        if (Mechanics.Target.canMoveTarget && Stats.stamina >= Mechanics.Target.costTarget)
        {
            Vector2 initialPlayerPosition = transform.position;
            // Calcula a direção do movimento do alvo
            Vector2 moveDirection = new Vector2(Input.CheckInput.moveDirection.x, Input.CheckInput.moveDirection.y);
            
            // Calcula a posição desejada do alvo
            Vector2 desiredPosition = Mechanics.Target.targetPosition + Mechanics.Target.speedTarget * Time.deltaTime * moveDirection;

            // Calcula a distância entre a posição inicial do jogador e a posição desejada do alvo
            float distanceFromInitialPosition = Vector2.Distance(initialPlayerPosition, desiredPosition);

            // Se a distância for menor do que a distância máxima permitida, move o alvo
            if (distanceFromInitialPosition < Mechanics.Target.maxDistanceFromPlayer)
            {
                Mechanics.Target.targetPosition = desiredPosition;
                Mechanics.Target.targetObject.transform.position = Mechanics.Target.targetPosition;
            }
            else
            {
                // Calcula a direção do vetor da posição atual do alvo em relação à posição inicial do jogador
                Vector2 direction = (desiredPosition - initialPlayerPosition).normalized;
                
                // Calcula a posição dentro do limite
                Vector2 limitedPosition = initialPlayerPosition + direction * Mechanics.Target.maxDistanceFromPlayer;
                
                Mechanics.Target.targetPosition = limitedPosition;
                Mechanics.Target.targetObject.transform.position = Mechanics.Target.targetPosition;
            }

            //Remover stamina
            Stats.RemoveStamina(Mechanics.Target.costTarget);
        }
        else
        {
            if (Mechanics.Throw.throwingObject == false)
            {
                Mechanics.TargetResetPosition();
            }
        }

        // Atualiza a visibilidade do alvo
        Mechanics.Target.targetObject.GetComponent<SpriteRenderer>().enabled = Mechanics.Target.canMoveTarget;
    }
}
