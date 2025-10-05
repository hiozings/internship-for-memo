using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Character : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public float invulnerableDuration;
    public float invulnerableConunter;
    public bool invulnerable;
    public bool isBuff;

    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDie;
    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (invulnerable)
        {
            invulnerableConunter -= Time.deltaTime;
            if (invulnerableConunter <= 0)
            {
                invulnerable = false;
            }
        }
    }
    public void TakeDamage(Attack attacker)
    {
        if (invulnerable)
            return;
        //Debug.Log(attacker.damage);
        if(currentHealth- attacker.damage > 0)
        {
            currentHealth -= attacker.damage; 
            TriggerInvulnerable();
            OnTakeDamage?.Invoke(attacker.transform);
        }
        else
        {
            currentHealth = 0;
            OnDie?.Invoke();
        }
    }

    private void TriggerInvulnerable()
    {
        if(!invulnerable)
        {
            invulnerable = true;
            invulnerableConunter = invulnerableDuration;
        }
    }


}
