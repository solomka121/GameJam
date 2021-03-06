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
    [SerializeField] private float _airCurrentVelocity;

    [Header("HP")]
    [SerializeField] private int _countHp;
    private int _hp; 

    [Header("WallRun")]
    [SerializeField] private LayerMask _whatIsWallRun;
    [SerializeField] private Transform _orientation;
    [SerializeField] private float wallrunForce, maxWallrunTime, maxWallSpeed;
    [SerializeField] private float maxWallRunCameraTilt, wallRunCameraTilt;
    private bool isWallRight, isWallLeft, isWallRunning;

    [Header("Camera")]
    [SerializeField] private Transform _camera;

    private Vector3 _nowCheckPoint;
    private bool _ground;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
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
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
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
    }

    #endregion

    #region Move
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

            if (Input.GetButtonDown("Jump"))
            {
                _rb.AddForce(Vector3.up * _forceJump);
            }

            _airCurrentVelocity = _rb.velocity.magnitude;
        }
        else
        { //add velocity limit
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;

            Vector3 movementDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            if (_airCurrentVelocity > _maxVelocityInAir)
            {
                _rb.velocity = _rb.velocity.normalized * _airCurrentVelocity;
            }
            _rb.AddForce(movementDirection * _speedInJump, ForceMode.Force);
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
        Destroy(gameObject);
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
        if (Input.GetKey(KeyCode.D) && isWallRight) StartWallrun();
        if (Input.GetKey(KeyCode.A) && isWallLeft) StartWallrun();
    }
    private void StartWallrun()
    {
        _rb.useGravity = false;
        isWallRunning = true;

        if (_rb.velocity.magnitude <= maxWallSpeed)
        {
            _rb.AddForce(_orientation.forward * wallrunForce * Time.deltaTime);

            //Make sure char sticks to wall
            if (isWallRight)
                _rb.AddForce(_orientation.right * wallrunForce / 5 * Time.deltaTime);
            else
                _rb.AddForce(-_orientation.right * wallrunForce / 5 * Time.deltaTime);
        }
    }
    private void StopWallRun()
    {
        isWallRunning = false;
        _rb.useGravity = true;
    }
    private void CheckForWall() //make sure to call in void Update
    {
        isWallRight = Physics.Raycast(transform.position, _orientation.right, 1f, _whatIsWallRun);
        isWallLeft = Physics.Raycast(transform.position, -_orientation.right, 1f, _whatIsWallRun);

        //leave wall run
        if (!isWallLeft && !isWallRight) StopWallRun();
    }

    private void WallJump()
    {
        if (isWallRunning)
        {

            //normal jump
            if (isWallLeft && !Input.GetKey(KeyCode.D) || isWallRight && !Input.GetKey(KeyCode.A))
            {
                _rb.AddForce(Vector2.up * _forceJump * 1.5f);
                _rb.AddForce(Vector3.up * _forceJump * 0.5f);
            }

            //sidwards wallhop
            if (isWallRight || isWallLeft && Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) _rb.AddForce(-_orientation.up * _forceJump * 1f);
            if (isWallRight || isWallLeft && Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) _rb.AddForce(-_orientation.up * _forceJump * 1f);
            if (isWallRight && Input.GetKey(KeyCode.A)) _rb.AddForce(-_orientation.right * _forceJump * 3.2f);
            if (isWallLeft && Input.GetKey(KeyCode.D)) _rb.AddForce(_orientation.right * _forceJump * 3.2f);

            //Always add forward force
            _rb.AddForce(_orientation.forward * _forceJump * 1f);
        }
    }
    #endregion

    void Update()
    {
        if(_force)
        {
            Move();
        }

        if(!_force)
        {
            MoveTurque();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Atack();
        }

        WallRunInput();
        CheckForWall();
    }
}
