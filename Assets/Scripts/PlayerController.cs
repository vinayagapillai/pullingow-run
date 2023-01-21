using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _intailPlayerSpeed = 4f;
    [SerializeField]
    private float _maxSpeed = 30f;  
    [SerializeField]
    private float _increaseRate = .1f;
    [SerializeField]
    private float _jumpHeight = 1.0f; 
    [SerializeField]
    private float _intialGravityValue = -9.81f;
    [SerializeField]
    private LayerMask groundLayer;

    private float _playerSpeed;
    private float _gravity;
    private Vector3 _moveDirection = Vector3.forward;

    private Rigidbody _rb;

    public bool IsDead;
    private bool _isDead;
    private bool _isGrounded;


    private void OnEnable()
    {
        InputManager.SwipeRight += TurnRight;
        InputManager.SwipeLeft += TurnLeft;
        InputManager.SwipeUp += Jump;
        InputManager.SwipeDown += Crouch;
        InputManager.DoubleTap += UsePowerUp;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _playerSpeed = _intailPlayerSpeed;
        _gravity = _intialGravityValue;
    }

    private void FixedUpdate()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        Vector3 movHorizontal = transform.right * hInput;
        Vector3 movVertical = transform.forward * vInput;

        //Final movement vector
        Vector3 velocity = (movHorizontal + movVertical) * _playerSpeed;

        if(velocity != Vector3.zero)
            _rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
    }

    private void OnDisable()
    {
        InputManager.SwipeRight -= TurnRight;
        InputManager.SwipeLeft -= TurnLeft;
        InputManager.SwipeUp -= Jump;
        InputManager.SwipeDown -= Crouch;
        InputManager.DoubleTap -= UsePowerUp;
    }

    private bool IsGrounded(float length)
    {
        Vector3 raycastOriginFirst = transform.position;
        raycastOriginFirst.y -= GetComponent<CapsuleCollider>().height / 2;
        //For not to pass through the ground
        raycastOriginFirst.y += .1f;

        Vector3 raycastOriginSecond = raycastOriginFirst;
        raycastOriginFirst -= transform.forward * .2f;
        raycastOriginSecond += Vector3.forward * .2f;

        Debug.DrawLine(raycastOriginFirst, Vector3.down, Color.green);
        Debug.DrawLine(raycastOriginSecond, Vector3.down, Color.blue) ;

        //Check if palyer touching the ground
        if((Physics.Raycast(raycastOriginFirst, Vector3.down, out RaycastHit hit1, length, groundLayer)) || (Physics.Raycast(raycastOriginSecond, Vector3.down, out RaycastHit hit2, length, groundLayer))){
            return true;
        }
        else
        {
            return false;
        }

    }

    private void TurnRight()
    {
        print("right");
    }
    
    private void TurnLeft()
    {
        print("Left");
    }
    
    private void Jump()
    {
        print("Jump");
    }

    private void Crouch()
    {
        print("Crouch");
    }

    private void UsePowerUp()
    {
        print("Power up");
    }

    
}
