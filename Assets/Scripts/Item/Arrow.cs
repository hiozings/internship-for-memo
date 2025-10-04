using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Arrow : MonoBehaviour
{
    public float normalSpeed;
    public float currentSpeed;
    public float lifeTime;
    public int gravity;

    private Attack attack;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Vector2 velo;
    private bool isLaunched;
    private bool touched;
    private float timer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        attack = GetComponent<Attack>();
        boxCollider = GetComponent<BoxCollider2D>();
        currentSpeed = normalSpeed;
    }

    private void Update()
    {
        if (isLaunched)
        {
            timer += Time.deltaTime;
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

    public void Launch(Vector2 direction)
    {
        isLaunched = true;
        velo = direction.normalized * currentSpeed;
        rb.velocity = velo;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        velo = Vector2.zero;
        touched = true;
        boxCollider.enabled = false;
        if (collision?.GetComponent<Tilemap>() != null)
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if(collision?.collider?.GetComponent<Tilemap>() != null)
    //    {
    //        rb.bodyType = RigidbodyType2D.Static;
    //    }
    //}
}
