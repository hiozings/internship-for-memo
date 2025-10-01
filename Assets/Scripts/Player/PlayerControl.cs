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
    public Vector2 inputDirection;
    [Header("基本参数")]
    public float speed;
    private float originScale;
    public float jumpForce;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        inputControl = new PlayerInputControl();
        originScale = transform.localScale.x;
        inputControl.Gameplay.Jump.started += Jump;
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
        Move();
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
}
