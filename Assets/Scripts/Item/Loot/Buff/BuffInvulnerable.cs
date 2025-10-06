using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffInvulnerable : Loot
{

    public void OnAddInvulnerable(Transform player)
    {
        Character character = player.GetComponent<Character>();
       
        character.ResetBuff(BuffType.Invul);
        character.isBuff = true;
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
