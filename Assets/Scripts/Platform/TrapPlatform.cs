using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapPlatform : MonoBehaviour
{
    public float disappearTime;
    public float reappearTime;
    private bool startVisible = true;

    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    //private Collider2D coll;

    private bool isActive = true;
    private bool isDisappearing = false;
    private bool isGround;
    public bool isSmash;
    private bool ignoreTrap;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>(); 
        anim = GetComponent<Animator>();
        if (!startVisible)
        {
            SetPlatformState(false);
        }
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //private void Update()
    //{
    //    isGround = coll?.GetComponent<PhysicsCheck>()?.isGround ?? false;
    //}

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("Tirrger");
        isGround = collision?.GetComponent<PhysicsCheck>()?.isGround ?? false;
        ignoreTrap = collision?.GetComponent<Enemy>()?.ignoreTrap ?? false;
        if (isGround && isActive && !isDisappearing && !isSmash && !ignoreTrap)
        {
            StartCoroutine(Disappear());
        }
    }

    private IEnumerator Disappear()
    {
        isDisappearing = true;
        yield return new WaitForSeconds(disappearTime);
        // 消失
        SetPlatformState(false);
        isActive = false;

        yield return new WaitForSeconds(reappearTime);
        // 重新出现
        SetPlatformState(true);
        isActive = true;
        isDisappearing = false;
    }

    private void SetPlatformState(bool state)
    {
        boxCollider.enabled = state;
        isSmash = !state;
        anim.SetBool("isSmash", isSmash);
        if (state)
            spriteRenderer.enabled = true;
    }
}
