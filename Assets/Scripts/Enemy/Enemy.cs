using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator anim;
    protected PhysicsCheck physicsCheck;
    protected CapsuleCollider2D capsuleCollider2D;
    protected BoxCollider2D boxCollider2D;
    public GameObject[] lootPrefab;

    public float normalSpeed;
    public float currentSpeed;
    public Vector3 faceDir;

    public float moveTime;
    public float moveDelta;
    private float moveCounter;
    public bool canMove = true;
    public bool isDead;
    public bool ignoreTrap;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        currentSpeed = normalSpeed;
        moveCounter = moveTime;
    }

    protected virtual void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        if(!isDead)
        {
            if(physicsCheck.touchLeftWall)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (physicsCheck.touchRightWall)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            if (!physicsCheck.isGround)
            {
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }
    }

    //private void FixedUpdate()
    //{   
    //    //if(!isDead)
    //    //{
    //    //    if (moveCounter > 0)
    //    //    {
    //    //        moveCounter -= moveDelta;
    //    //        anim.SetBool("isMoving", false);
    //    //    }
    //    //    else
    //    //    {
    //    //        Move();
    //    //        anim.SetBool("isMoving", true);
    //    //        moveCounter = moveTime;
    //    //    }
    //    //}
    //    //Move();
    //}

    public virtual void Move()
    {
        rb.position = new Vector2(rb.position.x + faceDir.x * 0.01f, rb.position.y);
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
    }

    public virtual void EnemyDead()
    {
        if(isDead) return;
        isDead = true;
        anim.SetBool("dead", true);
        capsuleCollider2D.enabled = false;
        boxCollider2D.enabled = false;
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        Attack attack = GetComponent<Attack>();
        attack.damage = 0;
        SpawnLoot();
        rb.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
        StartCoroutine(Destroy());
    }

    public virtual void SpawnLoot()
    {
        if (lootPrefab.Length > 0)
        {
            int index = UnityEngine.Random.Range(0, lootPrefab.Length);
            Instantiate(lootPrefab[index], transform.position, Quaternion.identity);
        }
    }

    protected IEnumerator Destroy()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
