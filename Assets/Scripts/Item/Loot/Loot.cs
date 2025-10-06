using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class Loot : MonoBehaviour
{
    public float buffDuration;
    public int score;
    private float timer;
    public float lifeTime;
    public float blinkStartTime;  // 开始闪烁的时间
    public float minBlinkInterval; // 最小闪烁间隔
    public float maxBlinkInterval;  // 最大闪烁间隔

    private SpriteRenderer spriteRenderer;

    public PlayAudioEventSO PlayAudioEvent;
    public AudioClip pickupFX;

    public UnityEvent<Transform> OnPickup;
    public event System.Action<GameObject> OnRemoveBuff;
    private Coroutine blinkCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        float remainingTime = lifeTime - timer;

        if (remainingTime <= blinkStartTime && blinkCoroutine == null)
        {
            StartBlinking(remainingTime);
        }

        if (timer >= lifeTime)
        {
            OnRemoveBuff?.Invoke(gameObject);
            Destroy(gameObject);
        }
    }

    #region Blinking Logic
    private void StartBlinking(float remainingTime)
    {
        blinkCoroutine = StartCoroutine(BlinkCoroutine(remainingTime));
    }

    // 闪烁协程
    private IEnumerator BlinkCoroutine(float initialRemainingTime)
    {
        while (spriteRenderer != null)
        {
            float currentRemainingTime = lifeTime - timer;
            float blinkInterval = CalculateBlinkInterval(currentRemainingTime);

            spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(blinkInterval);
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
    }

    // 计算当前闪烁间隔
    private float CalculateBlinkInterval(float remainingTime)
    {
        if (remainingTime <= 0) return minBlinkInterval;
        float normalizedTime = 1f - Mathf.Clamp01(remainingTime / blinkStartTime);
        float blinkSpeed = Mathf.Pow(normalizedTime, 2f); // 平方加速

        return Mathf.Lerp(maxBlinkInterval, minBlinkInterval, blinkSpeed);
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnPickup?.Invoke(collision.transform);
            OnRemoveBuff?.Invoke(gameObject);
            PlayAudioEvent.RaiseEvent(pickupFX);
            Destroy(gameObject);
        }
    }
}
