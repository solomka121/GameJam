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

    [Header("HP")]
    [SerializeField] private int _countHp;
    private int _hp; 

    [Header("WallRun")]
    [SerializeField] private LayerMask _whatIsWallRun;
    [SerializeField] private Transform _orientation;
    [SerializeField] private float _wallrunForce, _maxWallrunTime, _maxWallSpeed;
    [SerializeField] private float _maxWallRunCameraTilt, _wallRunCameraTilt;
    [SerializeField] private bool _isWallRight, _isWallLeft, _isWallRunning;

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

    private Vector3 _nowCheckPoint;
    private bool _ground;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _drag = _rb.drag;
        _rb.drag = _maxGroundDrag;
        _ground = false;
        _hp = _countHp;
    }

    private void Start()
    {
        _nowCheckPoint = transform.position;
        StartWallrun();
    }
    #endregion

    #region Check ground
    /*private void OnCollisionStay(Collision collision)
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
        
    }

    #endregion

    #region Move

    private void Jump()
    {
        if (_ground)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _rb.AddForce(Vector3.up * _forceJump);
            }
        }

    }
    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(horizontal, 0, vertical);

        if(_ground)
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

    public void Atack(int damage)
    {
        _hp -= damage;

        if(_hp <= 0)
        {
            Death();
        }
        else 
        {
            Respawn();
        }
    }

    public void Atack()
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

    private void Death()
    {
        //Destroy(gameObject);
    }

    public void Respawn()
    {
        transform.position = _nowCheckPoint;
        _rb.velocity = Vector3.zero;
    }

    #endregion

    #region CheckPoint

    public void CheckPoint(Vector3 point)
    {
        _nowCheckPoint = point;
    }

    #endregion

    #region Wall

    private void WallRunInput() //make sure to call in void Update
    {
        //Wallrun
        if (Input.GetKey(KeyCode.D) && _isWallRight) StartWallrun();
        if (Input.GetKey(KeyCode.A) && _isWallLeft) StartWallrun();
    }
    private void StartWallrun()
    {
        _rb.useGravity = false;
        _isWallRunning = true;

        if (_rb.velocity.magnitude <= _maxWallSpeed)
        {
            _rb.AddForce(_orientation.forward * _wallrunForce * Time.deltaTime);

            //Make sure char sticks to wall
            if (_isWallRight)
                _rb.AddForce(_orientation.right * _wallrunForce / 5 * Time.deltaTime);
            else
                _rb.AddForce(-_orientation.right * _wallrunForce / 5 * Time.deltaTime);
        }
    }
    private void StopWallRun()
    {
        _isWallRunning = false;
        _rb.useGravity = true;
    }
    private void CheckForWall() //make sure to call in void Update
    {
        _isWallRight = Physics.Raycast(transform.position, _orientation.right, 1f, _whatIsWallRun);
        _isWallLeft = Physics.Raycast(transform.position, -_orientation.right, 1f, _whatIsWallRun);

        //leave wall run
        if (!_isWallLeft && !_isWallRight) StopWallRun();
    }

    private void WallJump()
    {
        if (_isWallRunning)
        {

            //normal jump
            if (_isWallLeft && !Input.GetKey(KeyCode.D) || _isWallRight && !Input.GetKey(KeyCode.A))
            {
                _rb.AddForce(Vector2.up * _forceJump * 1.5f);
                _rb.AddForce(Vector3.up * _forceJump * 0.5f);
            }

            //sidwards wallhop
            if (_isWallRight || _isWallLeft && Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) _rb.AddForce(-_orientation.up * _forceJump * 1f);
            if (_isWallRight || _isWallLeft && Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) _rb.AddForce(-_orientation.up * _forceJump * 1f);
            if (_isWallRight && Input.GetKey(KeyCode.A)) _rb.AddForce(-_orientation.right * _forceJump * 3.2f);
            if (_isWallLeft && Input.GetKey(KeyCode.D)) _rb.AddForce(_orientation.right * _forceJump * 3.2f);

            //Always add forward force
            _rb.AddForce(_orientation.forward * _forceJump * 1f);
        }
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
        if(collision.gameObject.layer == 12)
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
	#endregion
	

	void Update()
    {
        Jump();
        if (Input.GetMouseButtonDown(1))
        {
            Atack();
        }

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
		
	}
}
