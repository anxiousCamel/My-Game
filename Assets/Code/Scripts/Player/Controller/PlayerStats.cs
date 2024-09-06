using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private PlayerData_Stats stats;
    private PlayerData_Collider col;
    private PlayerData_Mechanics mechanics;

    private void Start()
    {
        mechanics = GetComponent<PlayerData_Mechanics>();
        stats = GetComponent<PlayerData_Stats>();
        col = GetComponent<PlayerData_Collider>();
        stats.life = stats.maxLife;
        stats.stamina = stats.maxStamina;
    }

    private void Update()
    {
        // Verifica se a stamina atual é menor que 50% da stamina máxima
        if (stats.stamina < stats.maxStamina * 0.5f)
        {
            if (stats.blinkCoroutine == null)
            {
                stats.blinkCoroutine = StartCoroutine(BlinkSpriteRed());
            }
        }

        else
        {
            if (stats.blinkCoroutine != null)
            {
                StopCoroutine(stats.blinkCoroutine);
                stats.blinkCoroutine = null;
                ResetSpriteColor();
            }
        }

        //Reset stamina
        if (col.Check.isGround && !mechanics.Target.canMoveTarget)
        {
            stats.stamina = stats.maxStamina;
        }

    }

    private IEnumerator BlinkSpriteRed()
    {
        while (true)
        {
            // Calcula a taxa de piscar baseada na stamina, quanto menor a stamina, mais rápido pisca.
            float blinkRate = Mathf.Lerp(0.5f, 0.05f, 1 - (stats.stamina / (stats.maxStamina * 0.5f)));

            // Pisca em vermelho
            stats.playerSprite.color = Color.red;
            yield return new WaitForSeconds(blinkRate / 2);

            // Pisca em branco
            stats.playerSprite.color = Color.white;
            yield return new WaitForSeconds(blinkRate / 2);

            // Recalcula o blinkRate a cada loop para refletir imediatamente a mudança de stamina
        }
    }

    private IEnumerator BlinkSpriteGreen()
    {
        int blinkCount = 0; // Contador de piscadas
        int maxBlinks = 3;  // Número máximo de piscadas

        while (blinkCount < maxBlinks)
        {
            // Pisca em verde
            stats.playerSprite.color = Color.green;
            yield return new WaitForSeconds(0.05f);

            // Pisca em branco
            stats.playerSprite.color = Color.white;
            yield return new WaitForSeconds(0.1f);

            blinkCount++; // Incrementa o contador de piscadas
        }

        // Após completar as piscadas, pode resetar a cor, se necessário:
        stats.playerSprite.color = Color.white;
    }

    private void ResetSpriteColor()
    {
        stats.playerSprite.color = Color.white;
    }
}
