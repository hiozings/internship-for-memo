using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpeedUp : Loot
{
    public float speedMultiplier;
    public void OnSpeedUp(Transform player)
    {
        Character character = player.GetComponent<Character>();
        
        character.ResetBuff(BuffType.SpeedUp);
        character.isBuff = true;
        PlayerControl playerControl = player.GetComponent<PlayerControl>();
        playerControl.currentSpeed = playerControl.currentSpeed * speedMultiplier;
        playerControl.StartCoroutine(playerControl.ResetSpeed(playerControl, buffDuration, character));
        //character.buffType = BuffType.Nobuff;
        //character.OnBuffChange?.Invoke(character);
    }

    //public void StopSpeedUp(Transform player)
    //{
    //    PlayerControl playerControl = player.GetComponent<PlayerControl>();
    //    playerControl.currentSpeed = playerControl.normalSpeed;
    //}
}
