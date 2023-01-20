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


    private void Start()
    {
        InputManager.Instance.SwipeRight += TurnRight;
    }
    
    private void OnDisable()
    {
        InputManager.Instance.SwipeRight -= TurnRight;
    }

    private void TurnRight()
    {
        print("right");
    }
    
    
}
