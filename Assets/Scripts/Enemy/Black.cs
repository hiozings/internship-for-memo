using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Black : Enemy
{
    public float jumpForce;
    public bool isJumping;
    private bool jumpInProgress = false;
    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ignoreTrap = true;
    }

    protected override void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        anim.SetBool("isGround", physicsCheck.isGround);
        if (!isDead)
        {
            if (physicsCheck.touchLeftWall)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (physicsCheck.touchRightWall)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            if (!physicsCheck.isGround && !isJumping && !jumpInProgress)
            {
                StartCoroutine(JumpDelay());
            }
        }
    }

    private IEnumerator JumpDelay()
    {
        jumpInProgress = true;
        Jump();
        yield return new WaitForSeconds(1f);
        jumpInProgress = false;
    }

    private void FixedUpdate()
    {
        Move();
    }

    public override void Move()
    {
        base.Move();

    }



    private void Jump()
    {
        isJumping = true;
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        anim.SetBool("isJump", true);
    }

}
