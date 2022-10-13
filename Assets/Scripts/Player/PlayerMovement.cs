using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]

public class PlayerMovement : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float playerSpeed;

    [Header("Teleport")]
    [SerializeField] private int teleportCooldownTime = 10;
    [SerializeField] private bool teleportReady = true;
    [SerializeField] private GameObject teleportGuide;
    public CooldownManager cooldownManager;

    [Header("Weapons")]


    private float gravityValue = -9.8f;
    private float controllerDeadzone = 0.1f;
    private float rotateSmoothing = 1000f;
    private Vector3 playerVelocity;

    [Space]
    [SerializeField] private bool isGamepad;

    private CharacterController controller;
    private WaitForSeconds secondIncrement = new WaitForSeconds(.05f);

    //Input
    private Vector2 movement;
    private Vector2 aim;
    private bool teleportPressed;

    private PlayerControls playerControls;
    private PlayerInput playerInput;

    private void Awake() {
        controller = GetComponent<CharacterController>();
        playerControls = new PlayerControls();
        playerInput = GetComponent<PlayerInput>();

        teleportGuide.SetActive(false);

        cooldownManager.teleportCooldown.maxValue = teleportCooldownTime;
        cooldownManager.SetTeleport(teleportCooldownTime);
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    void Update() {
        HandleInput();
        HandleMovement();
        HandleRotation();
    }

    //Get input from device
    void HandleInput() {
        movement = playerControls.Controls.Movement.ReadValue<Vector2>();
        aim = playerControls.Controls.Aim.ReadValue<Vector2>();
        teleportPressed = playerControls.Controls.Teleport.WasReleasedThisFrame();
    }

    void HandleMovement() {
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        controller.Move(move * Time.deltaTime * playerSpeed);

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (teleportPressed) {
            Teleport();
        }
    }

    void HandleRotation() {
        if (isGamepad) {
            if (MathF.Abs(aim.x) > controllerDeadzone || MathF.Abs(aim.y) > controllerDeadzone) {
                Vector3 playerDirection = Vector3.right * aim.x + Vector3.forward * aim.y;
                if (playerDirection.sqrMagnitude > 0.0f) {
                    Quaternion newRotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotateSmoothing * Time.deltaTime);
                }
            }
        }
        else {
            Ray ray = Camera.main.ScreenPointToRay(aim);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayDistance;

            if (groundPlane.Raycast(ray, out rayDistance)) {
                Vector3 point = ray.GetPoint(rayDistance);
                LookAt(point);
            }
        }
    }

    void Teleport() {
        if (teleportReady == false) {
            return;
        }

        teleportGuide.SetActive(true);
        controller.transform.position = teleportGuide.transform.position;
        teleportGuide.SetActive(false);
 
        StartCoroutine(TeleportCooldown());
    }

    private IEnumerator TeleportCooldown() {
        teleportReady = false;
        float currentTeleport = 0f;

        cooldownManager.SetTeleport(currentTeleport);

        while(currentTeleport < teleportCooldownTime) {
            currentTeleport += .05f;
            cooldownManager.SetTeleport(currentTeleport);
            yield return secondIncrement;
        }

        teleportReady = true;
    }

    private void LookAt(Vector3 lookPoint) {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }

    public void onDeviceChange (PlayerInput pi) {
        isGamepad = pi.currentControlScheme.Equals("Gamepad") ? true : false;
    }
}
