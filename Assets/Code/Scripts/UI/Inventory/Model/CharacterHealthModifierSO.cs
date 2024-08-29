using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class CharacterHealthModifierSO : CharacterStatsModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        PlayerData_Stats stats = character.GetComponent<PlayerData_Stats>();
        if (stats != null)
        {
            stats.AddHealth((int)val);
        }
    }
}
