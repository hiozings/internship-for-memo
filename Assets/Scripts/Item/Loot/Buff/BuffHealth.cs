using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHealth : Loot
{ 
    public void OnAddHealth(Transform player)
    {
        Character character = player.GetComponent<Character>();
        
        character.ResetBuff(BuffType.Health);
        character.isBuff = true;
        character.currentHealth = Mathf.Min(character.currentHealth + 1, character.maxHealth);
        character.OnHealthChange?.Invoke(character);
    }
}
