using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // тег Ground повесить на землю, для работы прыжка!!!
    #region Initialization

    [Header("Controller")]
    [SerializeField] private bool _force;
    [SerializeField] private float _forceJump;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedInJump;
    [SerializeField] private float _maxVelocityInAir;
    [SerializeField] private float _maxVelocityOnGround;
    [SerializeField] private float _rbDragGravity;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private int _maxDoubleJump = 1;
    private float _tempRbDragGravity;
    private float _tempMaxVelocityInAir;
    private int _currentDoubleJump;
    public bool _isDoubleJumpActive; 

    [Header("HP")]
    [SerializeField] private int _countHp;
    private float _deathLevel;
    private int _hp;
    public Vector3 CurrentCheckPoint;

    [Header("WallRun")]
    [SerializeField] private LayerMask _whatIsWallRun;
    [SerializeField] private Transform _orientation;
    [SerializeField] private float _wallrunForce, _maxWallSpeed;
    [SerializeField] private float _maxWallRunCameraTilt, _wallRunCameraTilt;
    [SerializeField] private bool _isWallRight, _isWallLeft, _isWallRunning;
    [SerializeField] private float _MaxWallRunTime;
    private bool _readyToJump;


    [Header("Camera")]
    [SerializeField] private Transform _camera;

    [Header("Drag controller")]
    [SerializeField] private float _maxGroundDrag;
    [SerializeField] private float _minGroundDrag;
    [SerializeField] private float _normalSpeed;
    [SerializeField] private float _iceSpeed;
    [SerializeField] private LayerMask _highFrictionSurface;
    [SerializeField] private LayerMask _lowFrictionSurface;
    private float _drag;

    
    private bool _ground;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _drag = _rb.drag;
        _rb.drag = _maxGroundDrag;
        _ground = false;
        _hp = _countHp;
        _readyToJump = true;
    }

    private void Start()
    {
        CurrentCheckPoint = transform.position;
        _tempRbDragGravity = _rbDragGravity;
        _tempMaxVelocityInAir = _maxVelocityInAir;
    }
    #endregion

    #region Check ground
    /* private void OnCollisionStay(Collision collision)
     {
         if(collision.gameObject.CompareTag("Ground"))
         {
             _ground = true;
         }
     }

     private void OnCollisionExit(Collision collision)
     {
         if (collision.gameObject.CompareTag("Ground"))
         {
             _ground = false;
         }

     }*/

    private void CheckGround()
    {

        RaycastHit _hit;
        if (Physics.Raycast(transform.position, Vector3.down, out _hit, 0.9f, _whatIsGround))
        {
            Debug.DrawRay(transform.position, Vector3.down * 0.9f, Color.green);
            _ground = true;
            _rbDragGravity = _tempRbDragGravity;
            StopCoroutine(ChangeRbGravityDrag(0f, 0f, 0f));
        }
        else
        {
            _ground = false;
        }
    }

    #endregion

    #region Move

    private void Jump()
    {
        if (_ground)
        {
            _currentDoubleJump = 0;

            if (Input.GetButtonDown("Jump"))
            {
                _rb.AddForce(Vector3.up * _forceJump);
            }
        }

        if (!_ground && _currentDoubleJump < _maxDoubleJump && _isDoubleJumpActive)
        {
            if(Input.GetButtonDown("Jump"))
            {
                _currentDoubleJump++;
                _rb.AddForce(Vector3.up * _forceJump);
            }
        }
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(horizontal, 0, vertical);

        if (_ground)
        {
            if (move.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;

                Vector3 movementDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                _rb.AddForce(movementDirection * _speed, ForceMode.Force);
            }

        }
        else
        { //add velocity limit
            if (move.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;

                Vector3 movementDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                _rb.AddForce(movementDirection * _speedInJump, ForceMode.Force);
            }

        }
    }

    private void MoveTurque()
    {
        //works only in global (use camera rotation. if we change camera angle - it still torques in other direction)
        if (Input.GetButton("Horizontal"))
        {
            _rb.AddTorque(Vector3.back * Input.GetAxis("Horizontal") * 10);
        }

        if (Input.GetButton("Vertical"))
        {
            _rb.AddTorque(Vector3.right * Input.GetAxis("Vertical") * 10);
        }

        if (Input.GetButtonDown("Jump") && _ground)
        {
            _rb.AddForce(Vector3.up * _forceJump);
        }
    }

    #endregion

    #region HP

    public void LivesCount(int damage)
    {
        _hp -= damage;

        if (_hp <= 0)
        {
            Death();
        }
        else
        {
            Respawn();
        }
    }

    public void LivesCount()
    {
        _hp -= 1;

        if (_hp <= 0)
        {
            Death();
        }
        else
        {
            Respawn();
        }
    }

    
    public void PlayerDeathLevelChecking(float currentDeathLevel)
	{
        _deathLevel = currentDeathLevel;
	}

    private void CheckDeathLevel()
	{
		if (transform.position.y <= _deathLevel)
		{
            LivesCount();
		}
	}
    private void Death()
    {
        //Destroy(gameObject);
        //MainMenu
    }

    public void Respawn()
    {
        transform.position = CurrentCheckPoint;
        _rb.velocity = Vector3.zero;
    }

    #endregion

    #region CheckPoint

    public void CheckPoint(Vector3 point)
    {
        CurrentCheckPoint = point;
    }

    #endregion

    #region Wall

    private void WallRunInput() //make sure to call in void Update
    {
        //Wallrun
        float horizontal = Input.GetAxis("Horizontal");

        if (horizontal > 0 && _isWallRight) StartWallrun();
        if (horizontal < 0 && _isWallLeft) StartWallrun();
    }
    private void StartWallrun()
    {
        _rb.useGravity = false;
        //_rbDragGravity = 0;
        StartCoroutine(ChangeRbGravityDrag(0, _rbDragGravity * 2, _MaxWallRunTime));
        _isWallRunning = true;
        _maxVelocityInAir = 20;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(horizontal, 0, vertical);

        float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
        Vector3 movementDirectionForward = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        Vector3 movementDirectionRight = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.right;

        // if (_rb.velocity.magnitude <= _maxWallSpeed)/////////////////////////////////////////////////////////////////////
        // {
        _rb.AddForce(movementDirectionForward * _wallrunForce * Time.deltaTime);

        //Make sure char sticks to wall
        if (_isWallRight)
        {
            _rb.AddForce(movementDirectionRight * _wallrunForce / 5 * Time.deltaTime);
        }
        else
        {
            _rb.AddForce(-movementDirectionRight * _wallrunForce / 5 * Time.deltaTime);
        }


        // }
    }
    private void StopWallRun()
    {
        _isWallRunning = false;
        _rb.useGravity = true;
        _rbDragGravity = _tempRbDragGravity;
        _maxVelocityInAir = _tempMaxVelocityInAir;
    }
    private void CheckForWall() //make sure to call in void Update
    {
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(0, 0, vertical);

        float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
        Vector3 movementDirectionRight = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.right;



        _isWallRight = Physics.Raycast(transform.position, movementDirectionRight, 2f, _whatIsWallRun);
        _isWallLeft = Physics.Raycast(transform.position, -movementDirectionRight, 2f, _whatIsWallRun);

        Debug.DrawRay(transform.position, movementDirectionRight * 2f, Color.red);
        Debug.DrawRay(transform.position, -movementDirectionRight * 2f, Color.red);

        //leave wall run
        if (!_isWallLeft && !_isWallRight) StopWallRun();
    }

    private void WallJump()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(0, 0, vertical);

        float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
        Vector3 movementDirectionRight = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.right;
        Vector3 movementDirectionForward = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        if (_isWallRunning)
        {
            //normal jump
            if (Input.GetButtonDown("Jump") && _readyToJump)
            {
                _readyToJump = false;
                Invoke(nameof(ResetJump), 1f);
                _rb.AddForce(Vector3.up * _forceJump);
                _rb.AddForce(movementDirectionForward * _forceJump / 7);
            }

            //sidwards wallhop
            if (Input.GetButtonDown("Jump"))
            {
                if (_isWallRight && horizontal < 0) _rb.AddForce(-movementDirectionRight * _forceJump);
                if (_isWallLeft && horizontal > 0) _rb.AddForce(movementDirectionRight * _forceJump);
            }


            //Always add forward force
            //_rb.AddForce(movementDirectionForward * _forceJump * 0.001f);
        }
    }

    private void ResetJump()
    {
        _readyToJump = true;
    }
    #endregion

    #region RbDragController
    private void OnCollisionEnter(Collision collision)
	{
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(horizontal, 0, vertical);

        #region HighFriction ground (drag: 0.2 - 2)
        if (collision.gameObject.layer == 11)//high friction layer
        {
            _speed = _normalSpeed;
            if (move.magnitude > 0.3f)
            {
                //if moves
                if (_rb.drag > _minGroundDrag)
                {
                    //smoothly change drag to _minGroundDrag
                    StartCoroutine(ChangeDrag(_rb.drag, _minGroundDrag, 1f));

                }
            }
            if (move.magnitude < 0.3f)
            {
                //if stands
                if (_rb.drag < _maxGroundDrag)
                {
                    //smoothly plus drag value to _maxGroundDrag
                    StartCoroutine(ChangeDrag(_rb.drag, _maxGroundDrag, 1f));
                }
            }

        }
        #endregion

        #region LowFriction ground (drag: 0.2)
        if (collision.gameObject.layer == 12)
        {
            _speed = _iceSpeed;
            if (_ground)
            {
                if (_rb.drag > _minGroundDrag)
                {
                    //smoothly change drag to _minGroundDrag
                    StartCoroutine(ChangeDrag(_rb.drag, _minGroundDrag, 1f));

                }
            }
        }
        #endregion

    }

    private void RbDragInAir()
    {
        if (!_ground)
        {
            _speed = _normalSpeed;
            _rb.drag = _minGroundDrag;

        }
    }
    private void VelocityInAir()
    {
        if (!_ground)
        {
            if (_rb.velocity.magnitude > _maxVelocityInAir)
            {
                _rb.AddForce(-_rb.velocity * _speedInJump / 9, ForceMode.Force);
            }
            _rb.AddForce(Vector2.down * _rbDragGravity);
        }
    }
    private void VelocityOnGround()
    {
        if (_ground)
        {

            if (_rb.velocity.magnitude > _maxVelocityOnGround)
            {
                _rb.AddForce(-_rb.velocity * _speed / 10, ForceMode.Force);
            }
        }
    }

    IEnumerator ChangeDrag(float drag_start, float drag_end, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            _rb.drag = Mathf.Lerp(drag_start, drag_end, duration);
            elapsed += Time.deltaTime;
            yield return null;

        }
        _rb.drag = drag_end;
    }

    IEnumerator ChangeRbGravityDrag(float rbGravityDrag_start, float rbGravityDrag_end, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            _rbDragGravity = Mathf.Lerp(rbGravityDrag_start, rbGravityDrag_end, duration);
            elapsed += Time.deltaTime;
            yield return null;

            if (_ground)
            {
                break;
            }
        }
        _rbDragGravity = rbGravityDrag_end;
    }
    #endregion

    #region GetSet

    public void SetBonusSpeed(float value)
    {
        _normalSpeed *= value;
        _maxVelocityOnGround *= value;
    }


    #endregion
    void Update()
    {
        CheckGround();
        Jump();
        WallJump();

        //DELETE
        if (Input.GetMouseButtonDown(1))
        {
            LivesCount();
        }
        //DELETE

        WallRunInput();
        CheckForWall();
    }
    private void FixedUpdate()
    {
        if (_force)
        {
            Move();
        }

        if (!_force)
        {
            MoveTurque();
        }

        VelocityInAir();
        RbDragInAir();
        VelocityOnGround();
        CheckDeathLevel();
    }
}
