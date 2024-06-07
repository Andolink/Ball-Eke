using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region VARIABLES
    [Header("Important")]

    [SerializeField] private Transform targetPosition;
    [SerializeField] private CapsuleCollider ownCollider;
    [SerializeField] private ParticleSystem speedParticules;
    [SerializeField] private Animator cursorAnimator;

    [Header("VFX")]

    [SerializeField] private Animator vfxTrow;

    [Header("Input System")]

    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference dashAction;
    [SerializeField] private InputActionReference slideAction;
    [SerializeField] private InputActionReference slowmoAction;
    [SerializeField] private InputActionReference PunchAction;
    [SerializeField] private InputActionReference HookAction;
    
    [Header("Grab")]

    [SerializeField] private float grabRange = 3f;
    [SerializeField] private float trowForce = 10f;
    [SerializeField] private float ownVelocityTrowFactor = 2f;
    [SerializeField] private Transform holder;
    [SerializeField] private Transform lookAt;
    [SerializeField] private LayerMask interactLayerMask;

    private Grabable grabedObject = null;


    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashSpeed;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    [SerializeField] private float wallJumpSpeedFactor;
    [SerializeField] private float coyoteTime = 0f;
    [SerializeField] private float jumpBuffer = 0f;
   
    private bool readyToJump;
    private bool isJumping;

    [Header("Slide")]
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float crouchYScale;
    private float defaultColliderHeight;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] public bool grounded;

    [SerializeField] private Transform orientation;

    private float speedLimit = 0;

    private Vector3 moveDirection;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public bool isMovedOutside = false;
    [SerializeField] private float gravity = -20f;
    private float gravityFactor = 1f;

    private bool isSlaming = false;
    private float gravitySlam = -10f;

    private bool nearWallFound;
    private bool isMoving = false;
    private RaycastHit nearWall;

    private float dashVelocityLossTime = 0f;
    private float playerMovementControls = 1f;
    
    
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        readyToJump = true;

        defaultColliderHeight = ownCollider.height;
    }

    private void Update()
    {
        GroundedHandler();
        
        MovePlayer();
        SpeedControl();
       
        JumpHandler();
        DashHandler();
        SlamHandler();
        
        GrabHandler();
        SlowmoHandler();
    }

    public void ResetVar()
    {
        if (grabedObject != null)
        { Destroy(grabedObject.gameObject); }
        speedLimit = 0f;
        isSlaming = false;
        dashVelocityLossTime = 0;
        moveDirection = Vector3.zero;
        playerMovementControls = 1f;
        if (rb) rb.velocity = Vector3.zero;
        vfxTrow.gameObject.SetActive(false);
    }

    private void MovePlayer()
    {
        Vector2 _moveInput = moveAction.action.ReadValue<Vector2>();
        Vector3 _moveDirectionOld = moveDirection;
        Vector3 _curDir = moveDirection - Vector3.up * moveDirection.y;
        Vector3 _newDir = orientation.forward * _moveInput.y + orientation.right * _moveInput.x;
        
        float _dirChangeSpeed = 1f;
        
        if (!grounded)
        { _dirChangeSpeed = .15f; }

        isMoving = (_newDir != Vector3.zero);

        playerMovementControls = Mathf.Lerp(playerMovementControls, 1f, 6f * Time.deltaTime);

        if (!isMoving)
        { moveDirection = _moveDirectionOld; }
        else
        {
            moveDirection = Vector3.Lerp(_curDir, _newDir, 30f * _dirChangeSpeed * playerMovementControls * Time.deltaTime);
        }
    }

// THINGS
    private void GroundedHandler()
    {
        grounded = rb.velocity.y <= 0 && Physics.SphereCast(transform.position, 0.45f, Vector3.down, out RaycastHit _rayCast, playerHeight / 2 + 0.1f, whatIsGround) ;

        Vector3 _lookatFoward = orientation.transform.forward.normalized;
        Vector3 _lookatBackward = -orientation.transform.forward.normalized;
        Vector3 _lookatLeft = -orientation.transform.right.normalized;
        Vector3 _lookatRight = orientation.transform.right.normalized;

        nearWallFound = Physics.Raycast(transform.position, _lookatFoward, out nearWall, 1f, whatIsGround);
        if (!nearWallFound) nearWallFound = Physics.Raycast(transform.position, _lookatBackward, out nearWall, 1f, whatIsGround);
        if (!nearWallFound) nearWallFound = Physics.Raycast(transform.position, _lookatLeft, out nearWall, 1f, whatIsGround);
        if (!nearWallFound) nearWallFound = Physics.Raycast(transform.position, _lookatRight, out nearWall, 1f, whatIsGround);
    }
    private void SpeedControl()
    {
        gravityFactor = Mathf.Lerp(gravityFactor, 1f, 5f * Time.deltaTime);
        float _yVelo = Mathf.Lerp(rb.velocity.y, gravity, 4f * Time.deltaTime * gravityFactor);

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (grounded)
        {
            if (!isMoving)
            {
                speedLimit = Mathf.Lerp(speedLimit, 0, 10f * Time.deltaTime);
            }
            else
            {
                if (speedLimit < flatVel.magnitude)
                {
                    speedLimit = flatVel.magnitude;
                }
                else
                {
                    speedLimit = Mathf.Lerp(speedLimit, moveSpeed, 5f * Time.deltaTime);
                }
            }

        }
        else
        {
            if (isMoving)
            {
                if (speedLimit < moveSpeed)
                { speedLimit = Mathf.Lerp(speedLimit, moveSpeed, 5f * Time.deltaTime); }
                speedLimit += Time.deltaTime * 2f; 
            }
            else
            {
                speedLimit = flatVel.magnitude;
            }
        }

        Vector3 _newAimedVelocity = moveDirection * speedLimit + Vector3.up * _yVelo;
        rb.velocity = Vector3.Lerp(rb.velocity, _newAimedVelocity, 15f * Time.deltaTime);

        dashVelocityLossTime -= Time.deltaTime;
        if (dashVelocityLossTime > 0f)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, 4f * Time.deltaTime);
            speedLimit = rb.velocity.magnitude;
        }

        float _mult = 30f;
        float _val = Mathf.Clamp01(rb.velocity.magnitude - 4f);
        if ((dashVelocityLossTime > 0 || isSlaming) && !grounded)
        {
            _mult = 50f;
        }

        speedParticules.emissionRate = _val * _val * _mult;
    }
    public void ResetMovement(Vector3 _velocity)
    {
        rb.velocity = _velocity;
        speedLimit = _velocity.magnitude;
        moveDirection = _velocity.normalized;
    }
    public void StopMovedInside()
    {
        isMovedOutside = false;
        rb.isKinematic = false;
    }

// ACTIONS
    private void JumpHandler()
    {
        coyoteTime -= Time.deltaTime;
        if (grounded)
        {
            coyoteTime = 1;
        }

        jumpBuffer -= Time.deltaTime;
        if (jumpAction.action.ReadValue<float>() == 1)
        {
            jumpBuffer = 0.05f;
        }

        if (jumpAction.action.ReadValue<float>() == 0 && isJumping)
        {
             if (isJumping && rb.velocity.y > 0)
             {
                rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(rb.velocity.x, -1, rb.velocity.z), 4f * Time.deltaTime);
                if (rb.velocity.y <= 0)
                {
                    isJumping = false;
                }
             }
        }

        if (jumpBuffer > 0)
        {
            if (coyoteTime > 0)
            {
                StopMovedInside();
                isJumping = true;

                float _jumpVelocity = jumpForce + (rb.velocity.magnitude * 0.1f);

                rb.velocity = new Vector3(rb.velocity.x, _jumpVelocity, rb.velocity.z);
                moveDirection = rb.velocity.normalized;
                gravityFactor = 0f;


                StopMovedInside();

                jumpBuffer = 0;
                coyoteTime = 0;
                isSlaming = false;
                grounded = false;
               
                playerMovementControls = 1f;
                if (dashVelocityLossTime > 0)
                {
                    Meter.Instance.AddNewMeterText("Wave Dash", (int)rb.velocity.magnitude);
                    SFXManager.Instance.SfxPlay(SFXManager.Instance.sfxChadMove);
                    dashVelocityLossTime = 0f;
                }

                SFXManager.Instance.SfxPlay(SFXManager.Instance.sfxBass);
            }
            else if (nearWallFound)
            {
                isJumping = true;
                wallJumpSpeedFactor = 0;

                ResetMovement(nearWall.normal * rb.velocity.magnitude);
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                moveDirection = rb.velocity.normalized;

                jumpBuffer = 0;
                coyoteTime = 0;
                isSlaming = false;
                dashVelocityLossTime = 0f;
                playerMovementControls = 0f;
                gravityFactor = 0f;

                SFXManager.Instance.SfxPlay(SFXManager.Instance.sfxBass);
            }
        }
    }
    private void SlowmoHandler()
    {
        if (slowmoAction.action.ReadValue<float>() == 1)
        {
            TimeManager.Instance.Slowmo(-1, 0.25f, -1);
        }
        else
        {
            TimeManager.Instance.Slowmo(-1, 1, -1);
        }
    }
    private void DashHandler()
    {
        if (dashAction.action.WasPressedThisFrame())
        {
            SFXManager.Instance.SfxPlay(SFXManager.Instance.sfxDash);
            LevelManager.Instance.CameraShake();
            TimeManager.Instance.TimeStop(0.05f);
            float _velocityMagnitude = rb.velocity.magnitude + 2f;
            if (_velocityMagnitude < dashSpeed)
            { 
                _velocityMagnitude = dashSpeed;
                speedLimit = dashSpeed; 
            }

            StopMovedInside();

            Vector3 _dir = lookAt.forward;
            rb.velocity = _dir * _velocityMagnitude;
            moveDirection = rb.velocity.normalized;
            dashVelocityLossTime = 1f;
            gravityFactor = 0f;
            playerMovementControls = 0f;
        }
    }
    private void SlamHandler()
    {
        if (!grounded)
        {
            if (slideAction.action.ReadValue<float>() == 1 && isSlaming)
            {
                Debug.Log(gravitySlam);
                gravitySlam -= 100f * Time.deltaTime;
                rb.velocity = new(rb.velocity.x, gravitySlam, rb.velocity.z);
            }

            if (slideAction.action.WasPressedThisFrame())
            {
                TimeManager.Instance.TimeStop(0.075f);
                ResetMovement(Vector3.zero);
                gravitySlam = gravity;
                isSlaming = true;
            }
            else if(slideAction.action.WasReleasedThisFrame() && isSlaming)
            {
                TimeManager.Instance.TimeStop(0.075f);
                rb.velocity = new(rb.velocity.x, 0f, rb.velocity.z); ;
            }
        }
    }
    
// GRABING SYSTEM
    private void GrabHandler()
    {
        RaycastHit _rayCast;
        bool doCastBall = (Physics.Raycast(lookAt.position, lookAt.forward, out _rayCast, grabRange, interactLayerMask));

        cursorAnimator.SetBool("Interact",doCastBall);

        if (grabedObject != null)
        {
            if (PunchAction.action.WasPressedThisFrame())
            {
                TrowCurrentObject();
                TimeManager.Instance.TimeStop(0.25f);
            }
        }
        else
        {
            if (HookAction.action.WasPressedThisFrame())
            {
                if (doCastBall)
                {
                    GrabObject(_rayCast);
                }
            }
        }
    }
    private void GrabObject(RaycastHit _rayCast)
    {
        if (_rayCast.transform.TryGetComponent(out Grabable _grabable))
        {
            _grabable.Take(holder);
            grabedObject = _grabable;
        }
    }
    private void TrowCurrentObject()
    {
        float _velocity = trowForce;
        /*
        Vector3 _currentPosition = transform.position;
        Vector3 _targetPosition = targetPosition.position;//lookAt.position + lookAt.forward * _velocity;


        float _deltaY   = _targetPosition.y - _currentPosition.y;
        float _maxY     = _deltaY + 2f;
        float _deltaZ   = _targetPosition.z - _currentPosition.z;
        float _deltaX   = _targetPosition.x - _currentPosition.x;
        float _deltaF   = Mathf.Sqrt(_deltaX * _deltaX + _deltaZ * _deltaZ);
        float _grav     = 9.8f;

        float _y        = Mathf.Sqrt(2 * _grav * _maxY);
        float _delta    = 2 * _grav * (_maxY - _deltaY);
        float _f        = (Mathf.Sqrt(_delta)+_y)/ _deltaF;
        
        Vector3 _finalDir = lookAt.forward * _f + Vector3.up * _y;

        Debug.Log(string.Format("Foward {0} | Y {1}", _f, _y));
        */
        Vector3 _finalDir = lookAt.forward * trowForce + Vector3.up * trowForce * 0.85f;

        grabedObject.transform.position = lookAt.position + lookAt.forward * 1f;
        grabedObject.Trow(_finalDir);
        grabedObject = null;

        vfxTrow.gameObject.SetActive(true);
    }
}