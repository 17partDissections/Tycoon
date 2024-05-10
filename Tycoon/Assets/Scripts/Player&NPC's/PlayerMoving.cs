using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerMoving : MonoBehaviour
{
    private StandardMap _standardMap;
    private Rigidbody _rigidbody;

    void Start()
    {

        _standardMap = new StandardMap();
        _standardMap.GroundMap.Enable();
        _standardMap.GroundMap.Jump.performed += Jump;

        _rigidbody = GetComponent<Rigidbody>();

    }

    private void Jump(InputAction.CallbackContext context)
    {
        _rigidbody.AddForce(Vector3.up * 5, ForceMode.Impulse);
    }

    private void Update()
    {
        if (_standardMap.GroundMap.Moving.IsPressed())
        {
            //Debug.Log(_standardMap.GroundMap.Moving.ReadValueAsObject());
            Vector2 movingDirection = _standardMap.GroundMap.Moving.ReadValue<Vector2>();

            _rigidbody.velocity = new Vector3(movingDirection.x * 10, _rigidbody.velocity.y, movingDirection.y * 10);
            transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
        }
    }
}
