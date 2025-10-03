using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    protected Animator anim;
    PhysicsCheck physicsCheck;

    public float normalSpeed;
    public float currentSpeed;
    public Vector3 faceDir;

    public float moveTime;
    public float moveDelta;
    private float moveCounter;
    public bool canMove = true;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;
        moveCounter = moveTime;
    }

    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);

        if(physicsCheck.touchLeftWall)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (physicsCheck.touchRightWall)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if(!physicsCheck.isGround)
        {
            transform.localScale = new Vector3(faceDir.x, 1, 1);
        }
    }

    private void FixedUpdate()
    {
        if (moveCounter > 0)
        {
            moveCounter -= moveDelta;
            anim.SetBool("isMoving", false);
        }
        else
        {
            Move();
            anim.SetBool("isMoving", true);
            moveCounter = moveTime;
        }
        //Move();
    }

    public virtual void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);

    }
}
