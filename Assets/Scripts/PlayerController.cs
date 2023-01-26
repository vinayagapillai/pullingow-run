using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Animations;

namespace TempleRun {

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
        private LayerMask _groundLayer;        
        [SerializeField]
        private LayerMask _turnLayer;        
        [SerializeField]
        private LayerMask _obstacleLayer;
        [SerializeField]
        private AnimationClip _slideAnimationClip;
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private float _scoreMultiplier = 10f;

        public float _playerSpeed;
        private Vector3 _velocity;
        private Vector3 _moveDirection = Vector3.forward;
        private float  _massMultiplier = .005f;

        private Rigidbody _rb;

        private int _slidingAnimationId;
        private float _score = 0;


        public bool IsDead;
        private bool _isSliding = false;

        [SerializeField]
        private UnityEvent<Vector3> _turnEvent;
        [SerializeField]
        private UnityEvent<int> _scoreUpdateEvent;

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
            _slidingAnimationId = Animator.StringToHash("Sliding");
            _rb = GetComponent<Rigidbody>();
            _playerSpeed = _intailPlayerSpeed;

        }

        private void Update()
        {
            if (!IsGrounded(20f))
            {
                GameManager.Instance.GameOver();
                return;
            }
            //float vInput = Input.GetAxis("Vertical");


            //Vector3 movVertical = transform.forward * vInput;

            //Final movement vector
            //_velocity = movVertical * _playerSpeed;
            //_rb.MovePosition(transform.position + _velocity * Time.fixedDeltaTime);

            transform.Translate(Vector3.forward * _playerSpeed * Time.deltaTime);


            if (IsGrounded() && _velocity.y < 0)
            {
                _velocity.y = 0;
            }


            //Score functionality
            _score += _scoreMultiplier * Time.deltaTime;
            _scoreUpdateEvent.Invoke((int)_score);

            if(_playerSpeed < _maxSpeed)
            {
                _playerSpeed += Time.deltaTime * _increaseRate;
                //float speed = _rb.velocity.magnitude;
                float nextNearestWholeNumber = Mathf.CeilToInt(_playerSpeed);
           
                if (AreEqual(_playerSpeed, nextNearestWholeNumber))
                {
                    _rb.mass += _massMultiplier;
                }
               
                if(_animator.speed < 1.25f)
                {
                    _animator.speed += (1 / _playerSpeed) * Time.deltaTime;
                }
            }
        }

        bool AreEqual(float a, float b, float epsilon = 0.001f)
        {
            return Math.Abs(a - b) < epsilon;
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

            //Check if palyer touching the ground
            if ((Physics.Raycast(raycastOriginFirst, Vector3.down, out RaycastHit hit1, length, _groundLayer)) || (Physics.Raycast(raycastOriginSecond, Vector3.down, out RaycastHit hit2, length, _groundLayer)))
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
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, .1f, _turnLayer);
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
            Debug.Log("Do I have a value:" + turnPosition.HasValue);
            if (!turnPosition.HasValue)
            {
                //Game over is called is there is no turn when player tries to turn
                GameManager.Instance.GameOver();
                return;
            }
            print("right");
            //Assigning the local variable turnPosition to another variable since turnPosition can be null
            Vector3 origTurnPosition = turnPosition.Value;
            //Finding the target direction and multiplied by move Direction 
            Vector3 targetDirection = Quaternion.AngleAxis(90, Vector3.up) * _moveDirection;
            //Invokes the AddDiretion in TileSpawnerScript by passing the found target direction 
            _turnEvent.Invoke(targetDirection);
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
            Debug.Log("Do I have a value:" + turnPosition.HasValue);
            if (!turnPosition.HasValue)
            {
                //Game over is called is there is no turn when player tries to turn
                GameManager.Instance.GameOver();
                return;
            }
            print("Left");
            //Assigning the local variable turnPosition to another variable since turnPosition can be null
            Vector3 origTurnPosition = turnPosition.Value;
            //Finding the target direction and multiplied by move Direction 
            Vector3 targetDirection = Quaternion.AngleAxis(-90, Vector3.up) * _moveDirection;
            //Invokes the AddDiretion in TileSpawnerScript by passing the found target direction 
            _turnEvent.Invoke(targetDirection);
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
            if(!_isSliding && IsGrounded())
            {
                print("Crouch");
                StartCoroutine(Slide());
            }
        }

        private IEnumerator Slide()
        {
            _isSliding = true;

            //Play Sliding Animation
            _animator.Play(_slidingAnimationId);
            yield return new WaitForSeconds(_slideAnimationClip.length / _animator.speed);
            _isSliding = false;
        }

        private void UsePowerUp()
        {
            print("Power up");
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (((1 << collision.gameObject.layer) & _obstacleLayer) != 0)
            {
                Debug.Log("I am hitting:" + collision.gameObject.name);
                GameManager.Instance.GameOver();
            }
        }
    }
}

