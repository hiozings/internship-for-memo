using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffInvulnerable : Loot
{

    public void OnAddInvulnerable(Transform player)
    {
        Character character = player.GetComponent<Character>();
        if (character.isBuff) return;
        else character.isBuff = true;
        character.invulnerable = true;
        character.invulnerableConunter = buffDuration;
    }

    //public void StopInvulnerable(Transform player)
    //{
    //    Character character = player.GetComponent<Character>();
    //    character.invulnerable = false;
    //    character.invulnerableConunter = 0;
    //}
}
