using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Mathf;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
   
    private InputMaster _input;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private BulletController _bulletController;
    [SerializeField] private GameObject bulletSpawnPoint;
    public Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        _input = new InputMaster();
        _input.Player.Enable();

        _input.Player.Shoot.performed += HandleShootPerformed;
        _input.Player.Jump.performed += HandleJumpPerformed;
    }

    private void HandleJumpPerformed(InputAction.CallbackContext obj)
    {
        rigidbody.AddForce(Vector3.up*4,ForceMode.Impulse);
    }

    private void HandleShootPerformed(InputAction.CallbackContext obj)
    {
        var bulletRotation = Quaternion.LookRotation(playerCamera.transform.forward);
        Instantiate(_bulletController, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
    }

    private void Update()
    {
        ApplyLookRotation();
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        var movement = _input.Player.Movement.ReadValue<Vector2>();
        Transform transform2;
        (transform2 = transform).Translate(new Vector3(movement.x, 0, movement.y) * Time.deltaTime);
        Vector3 fowardMovement = movement.y * transform2.forward;
        Vector3 horizontalMovement = movement.x * transform2.right;
        Vector3 movementVector = (fowardMovement + horizontalMovement) * Time.deltaTime;
        if (!(_characterController is null)) _characterController?.Move(movementVector);
    }

    private void ApplyLookRotation()
    {
        var mouseDelta = _input.Player.Look.ReadValue<Vector2>();
        transform.Rotate(0, mouseDelta.x, 0);
        Transform transform1 = playerCamera.transform;
        Vector3 cameraRotation = transform1.localEulerAngles;
        cameraRotation.x -= mouseDelta.x;
        cameraRotation.x = Clamp(WrapAngle(cameraRotation.x), -90, 90);
        transform1.localEulerAngles = cameraRotation;
    }

    static float WrapAngle(float value) => Repeat(value + 180f, 360f) - 180f;
}
