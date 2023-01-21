using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed;

    private Rigidbody _rb;

    public bool IsDead;
    private bool _isDead;


    private void OnEnable()
    {
        InputManager.SwipeRight += TurnRight;
        InputManager.SwipeLeft += TurnLeft;
        InputManager.SwipeUp += Jump;
        InputManager.SwipeDown += Crouch;
        InputManager.DoubleTap += UsePowerUp;
    }
    
    private void OnDisable()
    {
        InputManager.SwipeRight -= TurnRight;
        InputManager.SwipeLeft -= TurnLeft;
        InputManager.SwipeUp -= Jump;
        InputManager.SwipeDown -= Crouch;
        InputManager.DoubleTap -= UsePowerUp;
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
