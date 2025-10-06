using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerControl : MonoBehaviour
{
    [Header("事件监听")]
    public SceneLoadEventSO loadEvent;
    public VoidEventSO afterSceneLoadedEvent;

    public PlayerInputControl inputControl;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private CapsuleCollider2D capsuleCollider;
    private PlayerAnimation playerAnimation;
    public GameObject arrowPrefab;
    public Transform firePoint;
    public Vector2 inputDirection;

    [Header("基本参数")]
    public float normalSpeed;
    public float currentSpeed;
    private float originScale;
    public float jumpForce;
    public float hurtForce;
    public float flySpeed;

    [Header("状态参数")]
    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    public bool canFly;
    public bool isFly;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
        inputControl = new PlayerInputControl();

        originScale = transform.localScale.x;
        currentSpeed = normalSpeed;

        inputControl.Gameplay.Jump.started += Jump;
        inputControl.Gameplay.Attack.started += PlayerAttack;
    }

    

    private void OnEnable()
    {
        inputControl.Enable();
        loadEvent.LoadRequestScene += OnLoadEvent;
        afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadedEvent;
    }

    

    private void OnDisable()
    {
        inputControl.Disable();
        loadEvent.LoadRequestScene -= OnLoadEvent;
        afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoadedEvent;
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
            if (canFly && !physicsCheck.isGround && InputSystem.GetDevice<Keyboard>().wKey.isPressed)
            {
                //UnityEngine.Debug.Log("Fly");
                isFly = true;
                Fly();
            }
            else
            {
                isFly = false;
            }
        }
    }

    private void OnLoadEvent(GameSceneEventSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.Gameplay.Disable();
        UnityEngine.Debug.Log("Load Event Received");
        this.gameObject.SetActive(false);
    }

    private void OnAfterSceneLoadedEvent()
    {
       inputControl.Gameplay.Enable();
        UnityEngine.Debug.Log("After Scene Loaded Event Received");
        this.gameObject.SetActive(true);
    }

    public void Move()
    {
        rb.velocity = new Vector2(currentSpeed * Time.deltaTime * inputDirection.x, rb.velocity.y);
        float faceDir = transform.localScale.x > 0 ? originScale: -originScale;
        if(inputDirection.x > 0)
            faceDir = -originScale;
        if(inputDirection.x < 0)
            faceDir = originScale;
        transform.localScale = new Vector3(faceDir, transform.localScale.y, transform.localScale.z);
    }

    public void Fly()
    {
        //rb.AddForce(Vector2.up * jumpForce * Time.deltaTime, ForceMode2D.Force);
        rb.velocity = new Vector2(rb.velocity.x, flySpeed * Time.deltaTime);
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
        //FireArrow();
    }

    private void FireArrow()
    {
        GameObject arrowObj = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        Arrow arrow = arrowObj.GetComponent<Arrow>();
        Vector2 dir = transform.localScale.x > 0 ? Vector2.left : Vector2.right;
        arrow.Launch(dir);
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
        LimitedInBounds limitedInBounds = GetComponent<LimitedInBounds>();
        limitedInBounds.isLimited = false;
    }

   

    #endregion

    public IEnumerator ResetSpeed(PlayerControl playerControl, float buffDuration, Character character)
    {
        //Debug.Log("Start");
        //Debug.Log(buffDuration);
        yield return new WaitForSeconds(buffDuration);
        //Debug.Log(buffDuration);
        //Debug.Log("End");
        playerControl.currentSpeed = playerControl.normalSpeed;
        character.isBuff = false;
        character.buffType = BuffType.Nobuff;
        character.OnBuffChange?.Invoke(character);
    }

    public IEnumerator ResetFly(PlayerControl playerControl, float buffDuration, Character character)
    {
        yield return new WaitForSeconds(buffDuration);
        playerControl.canFly = false;
        character.isBuff = false;
        character.buffType = BuffType.Nobuff;
        character.OnBuffChange?.Invoke(character);
    }
}
