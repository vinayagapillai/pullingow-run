using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private Vector3 _touchStartPos;
    private Vector3 _touchCurrentPos;

    [SerializeField] private float _minSwipeThreshold;

    public Action SwipeLeft;
    public Action SwipeRight;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _touchStartPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            _touchCurrentPos = Input.mousePosition;
            CheckSwipe();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _touchCurrentPos = Input.mousePosition;
            CheckSwipe();
        }
    }
    
    private void CheckSwipe()
    {
        Vector3 swipeDistance = _touchCurrentPos - _touchStartPos;
        
        if (Mathf.Abs(swipeDistance.x) > _minSwipeThreshold)
        {
            if (_touchStartPos.x < _touchCurrentPos.x)
            {
                //right
                SwipeRight?.Invoke();
            }
            else
            {
                //left
                SwipeLeft?.Invoke();
            }
        }
    }
}
