using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerControl : MonoBehaviour
{
    public PlayerInputControl inputControl;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private CapsuleCollider2D capsuleCollider;
    private PlayerAnimation playerAnimation;
    public Vector2 inputDirection;

    [Header("基本参数")]
    public float speed;
    private float originScale;
    public float jumpForce;
    public float hurtForce;

    [Header("状态参数")]
    public bool isHurt;
    public bool isDead;
    public bool isAttack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
        inputControl = new PlayerInputControl();

        originScale = transform.localScale.x;

        inputControl.Gameplay.Jump.started += Jump;
        inputControl.Gameplay.Attack.started += PlayerAttack;
    }

    

    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (!isHurt && !isDead)
        {
            Move();
        }
    }

    public void Move()
    {
        rb.velocity = new Vector2(speed * Time.deltaTime * inputDirection.x, rb.velocity.y);
        float faceDir = transform.localScale.x > 0 ? originScale: -originScale;
        if(inputDirection.x > 0)
            faceDir = -originScale;
        if(inputDirection.x < 0)
            faceDir = originScale;
        transform.localScale = new Vector3(faceDir, transform.localScale.y, transform.localScale.z);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if(physicsCheck.isGround)
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void PlayerAttack(InputAction.CallbackContext context)
    {
        playerAnimation.PlayAttack();
        isAttack = true;
    }

    #region UnityEvent
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
        //Debug.Log(hurtForce);
        //Debug.Log(dir);
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        //Debug.Log("Hurt");
    }

    public void PlayerDead()
    {
        
        isDead =true;
        inputControl.Gameplay.Disable();
        rb.AddForce(Vector2.up * (hurtForce*3), ForceMode2D.Impulse);
        //Debug.Log(Vector2.up * hurtForce);
        capsuleCollider.enabled = false;
    }
    #endregion
}
