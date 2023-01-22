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

    [Header("SWIPE")]
    [SerializeField] private float _minSwipeThreshold;
    
    [Header("DOUBLE TAP")]
    [SerializeField] private float _minDoupleTapThreshold = 0.5f;
    private int _tapTimes;
    private float _timeSinceLastTap;
    
    public static event Action DoubleTap;
    public static event Action SwipeLeft;
    public static event Action SwipeRight;
    public static event Action SwipeUp;
    public static event Action SwipeDown;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _touchStartPos = Input.mousePosition;
            CheckForDoubleTap();
        }
        else if (Input.GetMouseButton(0))
        {
            _touchCurrentPos = Input.mousePosition;
            //CheckSwipe();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _touchCurrentPos = Input.mousePosition;
            CheckSwipe();
        }
    }

    private void CheckForDoubleTap()
    {
        _tapTimes++;
        if (_tapTimes >= 2)
        {
            _tapTimes = 0;
            if (Time.time - _timeSinceLastTap < _minDoupleTapThreshold)
            {
                DoubleTap?.Invoke();
            }
        }
        _timeSinceLastTap = Time.time;
    }
    
    private void CheckSwipe()
    {

        if (GetVerticalSwipeValue() > _minSwipeThreshold && GetVerticalSwipeValue() > GetHorizontalSwipeValue())
        {
            
            if (_touchCurrentPos.y - _touchStartPos.y > 0)
            {
                //up
                SwipeUp?.Invoke();
            }
            else if (_touchCurrentPos.y - _touchStartPos.y < 0)
            {
                //down
                SwipeDown?.Invoke();
            }
            _touchStartPos = _touchCurrentPos;
            _tapTimes = 0;
        }
        else if (GetHorizontalSwipeValue() > _minSwipeThreshold && GetHorizontalSwipeValue() > GetVerticalSwipeValue())
        {
            if (_touchCurrentPos.x - _touchStartPos.x > 0)
            {
                //right
                SwipeRight?.Invoke();
            }
            else if (_touchCurrentPos.x - _touchStartPos.x < 0)
            {
                //left
                SwipeLeft?.Invoke();
            }
            _touchStartPos = _touchCurrentPos;
            _tapTimes = 0;
        }
    }

    private float GetVerticalSwipeValue()
    {
        return Mathf.Abs(_touchCurrentPos.y - _touchStartPos.y);
    }
    
    private float GetHorizontalSwipeValue()
    {
        return Mathf.Abs(_touchCurrentPos.x - _touchStartPos.x);
    }
}
