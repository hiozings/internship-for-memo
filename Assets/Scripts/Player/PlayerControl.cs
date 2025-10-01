using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerControl : MonoBehaviour
{
    public PlayerInputControl inputControl;
    private Rigidbody2D rb;
    public Vector2 inputDirection;
    public float speed;
    public float originScale;
    private void Awake()
    {
        inputControl = new PlayerInputControl();
        rb = GetComponent<Rigidbody2D>();
        originScale = transform.localScale.x;
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
}
