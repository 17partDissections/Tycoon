using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tycoon.PlayerSystems
{
    public class PlayerHotkeys : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private StandardMap _standardMap;
        private Rigidbody _rigidbody;
        [SerializeField] private Transform _rotation;
        private int _isMoving = Animator.StringToHash("isMoving");

        [SerializeField] GameObject _hotkeysInfoDisabled;
        [SerializeField] GameObject _hotkeysInfoEnabled;

        public Action TkeyPressed;
        private bool _isPlayerHotkeysWindowEnabled;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            

            _standardMap = new StandardMap();
            _standardMap.GroundMap.Enable();
            _standardMap.GroundMap.Jump.performed += Jump;
            _standardMap.GroundMap.Moving.started += context => _animator.SetBool(_isMoving, true);
            _standardMap.GroundMap.Moving.canceled += context => _animator.SetBool(_isMoving, false);


        }

        private void Jump(InputAction.CallbackContext context)
        {
            _rigidbody.AddForce(Vector3.up * 5, ForceMode.Impulse);
        }

        private void Update()
        {
            if (_standardMap.GroundMap.Moving.IsPressed())
            {
               
                Vector2 movingDirection = _standardMap.GroundMap.Moving.ReadValue<Vector2>();

                _rigidbody.velocity = new Vector3(movingDirection.x * 10, _rigidbody.velocity.y, movingDirection.y * 10);
                _rotation.rotation = Quaternion.LookRotation(_rigidbody.velocity);

            }
            
            if (Input.GetKeyDown(KeyCode.T))
                TkeyPressed.Invoke();
            if (Input.GetKeyDown(KeyCode.H))
            {
                if (_isPlayerHotkeysWindowEnabled)
                {
                    _hotkeysInfoDisabled.SetActive(true);
                    _hotkeysInfoEnabled.SetActive(false);
                    _isPlayerHotkeysWindowEnabled = false;
                }
                else
                {
                    _hotkeysInfoDisabled.SetActive(false);
                    _hotkeysInfoEnabled.SetActive(true);
                    _isPlayerHotkeysWindowEnabled = true;
                }
            }
        }
    }
}