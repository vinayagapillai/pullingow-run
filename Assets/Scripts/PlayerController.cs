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
        Vector3 myPosition = transform.position;
    }
}
