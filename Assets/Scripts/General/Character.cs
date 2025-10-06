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

    public PlayAudioEventSO PlayAudioEvent;
    public AudioClip takeDamageFX;
    public AudioClip dieFX;

    public BuffType buffType = BuffType.Nobuff;

    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Character> OnBuffChange;
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDie;
    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChange?.Invoke(this);
    }

    private void Update()
    {
        if (invulnerable)
        {
            invulnerableConunter -= Time.deltaTime;
            if (invulnerableConunter <= 0)
            {
                invulnerable = false;
                if(buffType == BuffType.Invul)
                {
                    buffType = BuffType.Nobuff;
                    OnBuffChange?.Invoke(this);
                    isBuff = false;
                }
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
            PlayAudioEvent.RaiseEvent(takeDamageFX);
        }
        else
        {
            currentHealth = 0;
            OnDie?.Invoke();
            PlayAudioEvent.RaiseEvent(dieFX);
        }
        OnHealthChange?.Invoke(this);
    }

    private void TriggerInvulnerable()
    {
        if(!invulnerable)
        {
            invulnerable = true;
            invulnerableConunter = invulnerableDuration;
        }
    }

    public void ResetBuff(BuffType newbuff)
    {
        PlayerControl playerControl = GetComponent<PlayerControl>();
        //Debug.Log(buffType);
        switch (buffType)
        {
            case BuffType.SpeedUp:
                
                if (playerControl != null)
                {
                    playerControl.currentSpeed = playerControl.normalSpeed;
                    isBuff = false;
                }
                break;
            case BuffType.Invul:
                invulnerable = false;
                invulnerableConunter = 0;
                isBuff = false;
                break;
            case BuffType.Health:
                isBuff = false;
                break;
            case BuffType.Fly:
                
                if (playerControl != null)
                {
                    playerControl.canFly = false;
                    //Debug.Log("Buff");
                    isBuff = false;
                }
                break;
            default:
                isBuff = false;
                break;
        }
        buffType = newbuff;
        OnBuffChange?.Invoke(this);
    }

}
