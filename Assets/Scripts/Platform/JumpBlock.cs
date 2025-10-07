using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBlock : MonoBehaviour
{
    private Animator anim;

    public float jumpForce;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetTrigger("JumpUp");
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                //rb.velocity = new Vector2(rb.velocity.x, 10f); // Adjust the jump force as needed
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }
}
