using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffFly : Loot
{
    public void OnFly(Transform player)
    {
        Character character = player.GetComponent<Character>();
        if (character.isBuff) return;
        else character.isBuff = true;
        PlayerControl playerControl = player.GetComponent<PlayerControl>();
        //PhysicsCheck physicsCheck = player.GetComponent<PhysicsCheck>();

        playerControl.canFly = true;
        //playerControl.StartCoroutine(playerControl.StopFly(playerControl, physicsCheck, buffDuration));
    }
}
