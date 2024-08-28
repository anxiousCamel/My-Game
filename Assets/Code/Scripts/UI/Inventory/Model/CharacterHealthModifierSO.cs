using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class CharacterHealthModifierSO : CharacterStatsModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        PlayerStats stats = character.GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.AddHealth((int)val);
        }
    }
}
