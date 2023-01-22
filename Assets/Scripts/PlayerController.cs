using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TempleRun.Player {

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
        [SerializeField]
        private LayerMask turnLayer;

        private float _playerSpeed;
        private float _gravity;
        private Vector3 _velocity;
        private Vector3 _moveDirection = Vector3.forward;

        private Rigidbody _rb;

        public bool IsDead;
        private bool _isDead;
        private bool _isGrounded;

        [SerializeField]
        private UnityEvent<Vector3> turnEvent;


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
            _velocity = (movHorizontal + movVertical) * _playerSpeed;

            if (_velocity != Vector3.zero)
                _rb.MovePosition(transform.position + _velocity * Time.fixedDeltaTime);

            if (IsGrounded() && _velocity.y < 0)
            {
                _velocity.y = 0;
            }

            //velocity.y += _gravity * Time.deltaTime;
        }

        private void OnDisable()
        {
            InputManager.SwipeRight -= TurnRight;
            InputManager.SwipeLeft -= TurnLeft;
            InputManager.SwipeUp -= Jump;
            InputManager.SwipeDown -= Crouch;
            InputManager.DoubleTap -= UsePowerUp;
        }

        private bool IsGrounded(float length = .2f)
        {
            Vector3 raycastOriginFirst = transform.position;
            raycastOriginFirst.y -= GetComponent<CapsuleCollider>().height / 2;
            //For not to pass through the ground
            raycastOriginFirst.y += .1f;

            Vector3 raycastOriginSecond = raycastOriginFirst;
            raycastOriginFirst -= transform.forward * .2f;
            raycastOriginSecond += Vector3.forward * .2f;

            Debug.DrawLine(raycastOriginFirst, Vector3.down, Color.green);
            Debug.DrawLine(raycastOriginSecond, Vector3.down, Color.blue);

            //Check if palyer touching the ground
            if ((Physics.Raycast(raycastOriginFirst, Vector3.down, out RaycastHit hit1, length, groundLayer)) || (Physics.Raycast(raycastOriginSecond, Vector3.down, out RaycastHit hit2, length, groundLayer)))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private Vector3? CheckTurn(float _turnValue)
        {
            //Returns a array of collider
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, .1f, turnLayer);
            if (hitColliders.Length != 0)
            {
                Tile tile = hitColliders[0].transform.parent.GetComponent<Tile>();
                TileType type = tile.type;
                if((type == TileType.LEFT && _turnValue == -1f) || (type == TileType.RIGHT && _turnValue == 1f) || (type == TileType.SIDEWAYS))
                {
                    return tile.pivot.position;
                }
            }
            return null;
        }

        private void TurnRight()
        {
            //Checking the turn by pssing the turn value
            Vector3? turnPosition = CheckTurn(1f);
            if (!turnPosition.HasValue)
                return;
            print("right");
            //Assigning the local variable turnPosition to another variable since turnPosition can be null
            Vector3 origTurnPosition = turnPosition.Value;
            //Finding the target direction and multiplied by move Direction 
            Vector3 targetDirection = Quaternion.AngleAxis(90, Vector3.up) * _moveDirection;
            //Invokes the AddDiretion in TileSpawnerScript by passing the found target direction 
            turnEvent.Invoke(targetDirection);
            Vector3 tempPlayerPosition = new Vector3(origTurnPosition.x, transform.position.y, origTurnPosition.z);
            transform.position = tempPlayerPosition;
            //Rotating the player 
            Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 90, 0);
            transform.rotation = targetRotation;
            //Setting up the moveDirection
            _moveDirection = transform.forward.normalized;
        }

        private void TurnLeft()
        {
            //Checking the turn by pssing the turn value
            Vector3? turnPosition =  CheckTurn(-1f);
            print("Left");
            //Assigning the local variable turnPosition to another variable since turnPosition can be null
            Vector3 origTurnPosition = turnPosition.Value;
            //Finding the target direction and multiplied by move Direction 
            Vector3 targetDirection = Quaternion.AngleAxis(-90, Vector3.up) * _moveDirection;
            //Invokes the AddDiretion in TileSpawnerScript by passing the found target direction 
            turnEvent.Invoke(targetDirection);
            Vector3 tempPlayerPosition = new Vector3(origTurnPosition.x, transform.position.y, origTurnPosition.z);
            transform.position = tempPlayerPosition;
            //Rotating the player 
            Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, -90, 0);
            transform.rotation = targetRotation;
            //Setting up the moveDirection
            _moveDirection = transform.forward.normalized;
        }

        private void Jump()
        {
            print("Jump");
            if (IsGrounded())
            {
                _rb.AddForce(transform.up * _jumpHeight, ForceMode.Impulse);
            }
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

}

