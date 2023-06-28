/* 
@Author: Christian Matos
@Date: 2023-06-18 10:41:42
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-18 10:41:42

* Functionality:
* Approach:
* To Use:
* Dependencies:
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // For input
using DG.Tweening; // For ghost trail

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float speed = 10f; // Movement speed
    [SerializeField] private float jumpForce = 50f;

    [Header("Player State")]
    public bool grounded = true;
    public bool canMove = false;
    public bool stunned = false;

    // Player Component
    private Rigidbody2D _rigidbody2d;
    private PlayerInput _playerInput;
    private Animator _animator;
    private BoxCollider2D _boxCollider2d;
    private SpriteRenderer _spriteRenderer;
    public PlayerInputActions playerInputActions;

    private void Awake()
    {
        // Get components
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
        _boxCollider2d = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    
        // Enable player input
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        // Subscribe functions to input actions
        playerInputActions.Player.Jump.performed += Jump;
        playerInputActions.Player.Light.performed += LightAttack;
        playerInputActions.Player.Heavy.performed += HeavyAttack;
        playerInputActions.Player.Pause.performed += Pause;
        playerInputActions.UI.Unpause.performed += Unpause;
        playerInputActions.Player.Grab.performed += Grab;

        playerInputActions.Player.Jump.canceled += Fall;
        playerInputActions.Player.Grab.canceled += Release;

        // Initialize variables
        canMove = true;
    }

    // 
    private void FixedUpdate()
    {
        // Functions that continually look for input
        Move();





        //CheckDirectionalInput();
        //IsGroundedGizmo();
        //AnimationUpdate();
        //Crouch();
    }

    #region Public Functions

    // Deactivates the player's ability to move for a set amount of time
    public void Stun(float stunTime)
    {
        StartCoroutine(StunCoroutine(stunTime));

        IEnumerator StunCoroutine(float stunTime)
        {
            canMove = false;
            stunned = true;
            yield return new WaitForSeconds(stunTime);
            canMove = true;
            stunned = false;
        }
    }
    #endregion

    // Check the player's state
    private void UpdatePlayerState()
    {
        if (IsGrounded())
        {
            grounded = true;
            _animator.SetBool("grounded", true);
        }
        else
        {
            grounded = false;
            _animator.SetBool("grounded", false);
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider2d.bounds.center, _boxCollider2d.bounds.size, 0f, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        return raycastHit.collider != null;
    }

    #region Private Functions (Movement)

    // Checks for horizontal input, moves the player and flips the sprite
    private void Move()
    {
        // Checks if the player can move
        if (!canMove)
            return;

        // Flips the sprite renderer if the player is moving right
        if (playerInputActions.Player.Move.ReadValue<Vector2>().x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (playerInputActions.Player.Move.ReadValue<Vector2>().x < 0)
            transform.localScale = new Vector3(1, 1, 1);
        
        // Assigns the horizontal input multiplied by the speed, but keeps the vertical velocity
        _rigidbody2d.velocity = new Vector2(playerInputActions.Player.Move.ReadValue<Vector2>().x * speed, _rigidbody2d.velocity.y);
        
        // Sets the horizontal float in the animator to the absolute value of the horizontal input
        _animator.SetFloat("horizontalSpeed", Mathf.Abs(_rigidbody2d.velocity.x));
    }

    private void Crouch()
    {
        // Checks if the player can move and is on the ground
        if (!canMove) { return; }


        if (playerInputActions.Player.Move.ReadValue<Vector2>().y < 0)
            _animator.SetBool("isCrouching", true);
        else
            _animator.SetBool("isCrouching", false);
    }




    #endregion
    //-------------------------------------------------------------------------------------------------------------------\

















    [SerializeField] private float fallMultiplier = 5f;
    [SerializeField] private float lowJumpMultiplier = 2f; 
    [SerializeField] private float wallJumpLerp = 10f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float slideSpeed = 5f;

    [Header("Collision Settings")]
    [SerializeField] private float collisionRadius = 0.25f;
    [SerializeField] private Vector2 groundOffset, wallLeftOffset, wallRightOffset;
    [SerializeField] private LayerMask groundLayerMask;

    [Header("Ghost Trail")]
    [SerializeField] private Color trailColor = Color.white;
    [SerializeField] private Color fadeColor = Color.clear;
    [SerializeField] private float ghostInterval = 0.5f;
    [SerializeField] private float fadeTime = 0.5f;

    [Header("Particles")]
    [SerializeField] private ParticleSystem dashParticle;
    [SerializeField] private ParticleSystem jumpParticle;
    [SerializeField] private ParticleSystem landParticle;
    [SerializeField] private ParticleSystem slideParticle;
    [SerializeField] private ParticleSystem wallJumpParticle;

    // Public variables
    [HideInInspector] public bool onWall;
    [HideInInspector] public bool onRightWall;
    [HideInInspector] public bool onLeftWall;

    [HideInInspector] public bool wallGrab;
    [HideInInspector] public bool wallJumped;
    [HideInInspector] public bool wallSlideBool;
    [HideInInspector] public bool isDashing;

    [HideInInspector] public int wallSlide;

    private bool groundTouch;
    private bool hasDashed;
    private bool betterJumpingEnabled;

    public int side = 1;
    

    // Private variables
    private bool isPaused = false;
    private Vector2 movementInput;
    float x = 0;
    float y = 0;
    float xRaw = 0;
    float yRaw = 0;
    Vector2 direction = Vector2.zero;






    public PlayerState GetPlayerState()
    {
        if (IsGrounded())
            return PlayerState.Grounded;
        else
            return PlayerState.Airborne;
    }



    void Update()
    {
        //MovementUpdate();
        //CollisionVariableAssign();

        // if (grounded && !isDashing)
        // {
        //     wallJumped = false;
        //     betterJumpingEnabled = true;
        // }

        // if (wallGrab && !isDashing)
        //     GrabWall(x, y);
        // else
        //     _rigidbody2d.gravityScale = 3;

        // BetterJumping();
        // CheckSlideWall();
        // CheckGroundTouch();
        // WallParticle(y);

        // if (wallGrab || wallSlideBool || !canMove)
        //     return;
        
        // CheckSide();
    }

    private void CheckSide()
    {
        if (x > 0)
        {
            side = 1;
            AnimationFlip(side);
        }
        else if (x < 0)
        {
            side = -1;
            AnimationFlip(side);
        }
    }

    private void WallParticle(float vertical)
    {
        var main = slideParticle.main;

        if (wallSlideBool || (wallGrab && vertical < 0))
        {
            slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
            main.startColor = Color.white;
        }
        else 
            main.startColor = Color.clear;
    }

    private void CheckGroundTouch()
    {
        if (grounded && !groundTouch)
        {
            groundTouch = true;
            GroundTouch();
        }
        if (!grounded && groundTouch)
        {
            groundTouch = false;
        }
    }

    private void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;

        side = _spriteRenderer.flipX ? -1 : 1;

        jumpParticle.Play();
    }

    private void CheckSlideWall()
    {
        if (onWall && !grounded)
        {
            if (x != 0 && !wallGrab)
            {
                wallSlideBool = true;
                SlideWall();
            }
        }
        if (!onWall || grounded)
            wallSlideBool = false;
    }

    private void SlideWall()
    {
        if (wallSlide != side)
            AnimationFlip(side * -1);

        if (!canMove)
            return;

        bool pushingWall = false;
        if(((_rigidbody2d.velocity.x > 0 && onRightWall) || (_rigidbody2d.velocity.x < 0 && onLeftWall)))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : _rigidbody2d.velocity.x;

        _rigidbody2d.velocity = new Vector2(push, -slideSpeed);
    }

    private void GrabWall(float x, float y)
    {
        _rigidbody2d.gravityScale = 0;
        if (x > 0.2f || x < -0.2f)
            _rigidbody2d.velocity = new Vector2(_rigidbody2d.velocity.x, 0);

        float speedModifier = y > 0 ? .5f : 1; 

        _rigidbody2d.velocity = new Vector2(_rigidbody2d.velocity.x, y * (speed * speedModifier));
    }

    private void MovementUpdate()
    {
        x = playerInputActions.Player.Move.ReadValue<Vector2>().x;
        y = playerInputActions.Player.Move.ReadValue<Vector2>().y;
        xRaw = x;
        yRaw = y;
        direction = new Vector2(x, y);
    }

    private void ShowGhostTrail()
    {
        Sequence s = DOTween.Sequence();

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform currentGhost = transform.GetChild(i);
            s.AppendCallback(() => currentGhost.position = transform.position); // move.transform.position
            s.AppendCallback(() => currentGhost.GetComponent<SpriteRenderer>().flipX = _spriteRenderer.flipX);
            s.AppendCallback(() => currentGhost.GetComponent<SpriteRenderer>().sprite = _spriteRenderer.sprite);
            s.Append(currentGhost.GetComponent<SpriteRenderer>().material.DOColor(trailColor, 0f));
            s.AppendCallback(() => FadeSprite(currentGhost));
            s.AppendInterval(ghostInterval);
        }
    }

    private void FadeSprite(Transform currentGhost)
    {
        currentGhost.GetComponent<SpriteRenderer>().material.DOKill();
        currentGhost.GetComponent<SpriteRenderer>().material.DOColor(fadeColor, fadeTime);
    }

    private void CollisionVariableAssign()
    {
        grounded = Physics2D.OverlapCircle((Vector2)transform.position + groundOffset, collisionRadius, groundLayerMask);
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + wallRightOffset, collisionRadius, groundLayerMask) || Physics2D.OverlapCircle((Vector2)transform.position + wallRightOffset, collisionRadius, groundLayerMask);

        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + wallRightOffset, collisionRadius, groundLayerMask);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + wallLeftOffset, collisionRadius, groundLayerMask);

        wallSlide = onRightWall ? -1 : 1;
    }

    private void BetterJumping()
    {
        if (betterJumpingEnabled)
        {
            if (_rigidbody2d.velocity.y < 0)
            {
                _rigidbody2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (_rigidbody2d.velocity.y > 0 && !playerInputActions.Player.Jump.ReadValue<float>().Equals(1))
            {
                _rigidbody2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
    }

    private void AnimationUpdate()
    {
        _animator.SetBool("grounded", IsGrounded());
        _animator.SetBool("onWall", onWall); 
        _animator.SetBool("onRightWall", onRightWall); 
        _animator.SetBool("wallGrab", false); // move.wallGrab
        _animator.SetBool("wallSlide", false); // move.wallSlide
        _animator.SetBool("canMove", false); // move.canMove
        _animator.SetBool("isDashing", false); // move.isDashing
    }

    private void AnimatonSetHorizontalMovement(float x, float y, float yVelocity)
    {
        _animator.SetFloat("horizontalAxis", x);
        _animator.SetFloat("verticalAxis", y);
        _animator.SetFloat("verticalVelocity", yVelocity);
        _animator.SetFloat("speed", Mathf.Abs(x) + Mathf.Abs(y));
    }

    private void AnimationFlip(int side)
    {
        if (false || false) //move.wallGrab || move.wallSlide
        {
            if (side == -1 && _spriteRenderer.flipX)
                return;
            if (side == 1  && !_spriteRenderer.flipX)
                return;
        }

        bool state = (side == 1) ? false : true;
        _spriteRenderer.flipX = state;
    }

    private void IsGroundedGizmo()
    {
        Color rayColor;
        if (IsGrounded())
            rayColor = Color.green;
        else
            rayColor = Color.red;
        
        Debug.DrawRay(_boxCollider2d.bounds.center, Vector2.down * (_boxCollider2d.bounds.extents.y), rayColor);
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.blue;

        var positions = new Vector2[]
        {
            groundOffset,
            wallLeftOffset,
            wallRightOffset
        };
        
        Gizmos.DrawWireSphere((Vector2)transform.position + groundOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + wallLeftOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + wallRightOffset, collisionRadius);
    }

    private void CheckDirectionalInput()
    {
        movementInput = playerInputActions.Player.Move.ReadValue<Vector2>();
    }

    private void Move_()
    {
        if (!canMove)
            return;

        if (wallGrab)
            return;

        if (!wallJumped)
            _rigidbody2d.velocity = new Vector2(direction.x * speed, _rigidbody2d.velocity.y);
        else
            _rigidbody2d.velocity = Vector2.Lerp(_rigidbody2d.velocity, (new Vector2(direction.x * speed, _rigidbody2d.velocity.y)), wallJumpLerp * Time.deltaTime);

        _animator.SetFloat("horizontal", Mathf.Abs(movementInput.x));

        // if (movementInput.x == 1)
        //     this.transform.localScale = new Vector3(-1, 1, 1);
        // else if (movementInput.x == -1)
        //     this.transform.localScale = new Vector3(1, 1, 1);
        // _rigidbody2d.AddForce(new Vector2(movementInput.x, 0.0f) * speed, ForceMode2D.Force);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        _animator.SetTrigger("jump");

        if(grounded)
            NormalJump(Vector2.up, false);
            return;
        if(onWall && !grounded)
            WallJump();
            return;
        Debug.Log("Not grounded or on wall.");
        
    }



    private void NormalJump(Vector2 direction, bool enable) 
    {
        slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
        ParticleSystem particle = wallGrab ? wallJumpParticle : jumpParticle;

        _rigidbody2d.velocity = new Vector2(_rigidbody2d.velocity.x, 0);
        _rigidbody2d.velocity += direction * jumpForce;

        particle.Play();
    }

    private int ParticleSide()
    {
        return 0;
    }

    private void WallJump()
    {
        if ((side == 1 && onRightWall) || side == -1 && !onRightWall)
        {
            side *= -1;
            AnimationFlip(side);
        }

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = onRightWall ? Vector2.left : Vector2.right;

        NormalJump((Vector2.up / 1.5f + wallDir / 1.5f), true);
        wallJumped = true;
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    private void Fall(InputAction.CallbackContext context)
    {
        Debug.Log("Fall");
        _rigidbody2d.velocity = Vector2.zero;

    }

    private void LightAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Light Attack/Dash");
        if (!hasDashed)
        {
            if(xRaw != 0 || yRaw != 0)
                Dash(xRaw, yRaw);
        }

    }

    private void Dash(float x, float y)
    {
        Camera.main.transform.DOComplete();
        Camera.main.transform.DOShakePosition(.2f, .5f, 12, 90, false, true);
        //FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));

        hasDashed = true;

        _animator.SetTrigger("dash");

        _rigidbody2d.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x, y);

        _rigidbody2d.velocity += dir.normalized * dashSpeed;
        StartCoroutine(DashWait());
    }

    IEnumerator DashWait()
    {
        ShowGhostTrail();
        StartCoroutine(GroundDash());
        DOVirtual.Float(14, 0, .8f, RigidbodyDrag);

        dashParticle.Play();
        _rigidbody2d.gravityScale = 0;
        betterJumpingEnabled = false;
        wallJumped = true;
        isDashing = true;

        yield return new WaitForSeconds(.3f);

        dashParticle.Stop();
        _rigidbody2d.gravityScale = 3;
        betterJumpingEnabled = true;
        wallJumped = false;
        isDashing = false;
    }

    private void RigidbodyDrag(float x)
    {
        _rigidbody2d.drag = x;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (grounded)
            hasDashed = false;
    }

    private void HeavyAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Heavy Attack");
        _animator.SetTrigger("heavyAttack");
    }

    private void Pause(InputAction.CallbackContext context)
    {
        Debug.Log("Pause");
        isPaused = true;
        _playerInput.SwitchCurrentActionMap("UI");
        playerInputActions.UI.Enable();
        playerInputActions.Player.Disable();
    }

    private void Unpause(InputAction.CallbackContext context)
    {
        Debug.Log("Unpause");
        isPaused = false;
        _playerInput.SwitchCurrentActionMap("Player");
        playerInputActions.Player.Enable();
        playerInputActions.UI.Disable();
    }

    private void Grab(InputAction.CallbackContext context)
    {
        Debug.Log("Grab");
        if(onWall && canMove)
        {
            if (side != wallSlide)
                AnimationFlip(side * -1);
            wallGrab = true;
            wallSlideBool = false;
        }
    }

    private void Release(InputAction.CallbackContext context)
    {
        Debug.Log("Release");
        wallGrab = false;
        wallSlideBool = false;
    }
}
