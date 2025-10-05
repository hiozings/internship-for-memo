using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Arrow : MonoBehaviour
{
    public float normalSpeed;
    public float currentSpeed;
    public float lifeTime;
    public int gravity;
    public float hurtForce;
    public float blinkStartTime;  // 开始闪烁的时间
    public float minBlinkInterval; // 最小闪烁间隔
    public float maxBlinkInterval;  // 最大闪烁间隔

    private Attack attack;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private PlatformEffector2D platformEffector;
    private CapsuleCollider2D capsuleCollider;
    private SpriteRenderer spriteRenderer;

    private Vector2 velo;
    private bool isLaunched;
    private bool touched;
    private float timer;
    private Vector2 dir;
    private Coroutine blinkCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        attack = GetComponent<Attack>();
        boxCollider = GetComponent<BoxCollider2D>();
        platformEffector = GetComponent<PlatformEffector2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentSpeed = normalSpeed;
    }

    private void Update()
    {
        if (isLaunched)
        {
            timer += Time.deltaTime;
            float remainingTime = lifeTime - timer;

            if (remainingTime <= blinkStartTime && blinkCoroutine == null)
            {
                StartBlinking(remainingTime);
            }

            if (timer >= lifeTime)
            {
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        if (isLaunched && !touched)
        {
            velo.y -= gravity * Time.fixedDeltaTime * rb.gravityScale;
            rb.velocity = velo;
            if(velo != Vector2.zero)
            {
                float angle = Mathf.Atan2(velo.y, velo.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }
    private void StartBlinking(float remainingTime)
    {
        blinkCoroutine = StartCoroutine(BlinkCoroutine(remainingTime));
    }

    // 闪烁协程
    private IEnumerator BlinkCoroutine(float initialRemainingTime)
    {
        while (isLaunched && spriteRenderer != null)
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

    
    public void Launch(Vector2 direction)
    {
        isLaunched = true;
        velo = direction.normalized * currentSpeed;
        rb.velocity = velo;
        dir = new Vector2(direction.x, direction.y);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        velo = Vector2.zero;
        touched = true;
        //boxCollider.enabled = false;
        
        if (collision?.GetComponent<Tilemap>() != null || collision?.gameObject.layer == LayerMask.NameToLayer("AirWall"))
        {
            rb.bodyType = RigidbodyType2D.Static;
            BecomePlatform();
        }
        if(collision?.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //Rigidbody2D rigidbody2D = collision.GetComponent<Rigidbody2D>();
            //Vector2 dir = new Vector2(collision.transform.position.x - transform.position.x, 0).normalized;
            //rigidbody2D.AddForce(dir * hurtForce, ForceMode2D.Impulse);
            StartCoroutine(Wait());
            boxCollider.enabled = false;
            Destroy(gameObject);
        }
    }

    private void BecomePlatform()
    {
        capsuleCollider.enabled = false;
        boxCollider.usedByEffector = true;
        platformEffector.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Ground");
        if(dir.x < 0)
        {
            platformEffector.rotationalOffset = 180;
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.1f);
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if(collision?.collider?.GetComponent<Tilemap>() != null)
    //    {
    //        rb.bodyType = RigidbodyType2D.Static;
    //    }
    //}
}
