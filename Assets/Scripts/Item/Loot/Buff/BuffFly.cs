using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffFly : Loot
{
    public void OnFly(Transform player)
    {
        Character character = player.GetComponent<Character>();
        character.ResetBuff(BuffType.Fly);
        character.isBuff = true;
        PlayerControl playerControl = player.GetComponent<PlayerControl>();
        //PhysicsCheck physicsCheck = player.GetComponent<PhysicsCheck>();

        playerControl.canFly = true;
        playerControl.StartCoroutine(playerControl.ResetFly(playerControl, buffDuration, character));

    }
}
