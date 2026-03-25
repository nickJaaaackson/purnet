using PurrNet;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementSample : NetworkIdentity
{
    [SerializeField] private float moveForce = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private NetworkRigidbody rb;
    [SerializeField] private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction jumpAction;

    protected override void OnSpawned()
    {
        base.OnSpawned();
        enabled = isOwner;

        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }
        playerInput.enabled = false;
        playerInput.enabled = enabled;
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];

        if (isOwner)
        {
            Renderer renderer = GetComponentInChildren<Renderer>();
            renderer.material.color = Color.green;
        }
    }

    private void FixedUpdate()
    {
        if (!isOwner || moveAction == null)
        {
            return;
        }

        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        rb.AddForce(direction * (moveForce * rb.mass));
    }

    private void Update()
    {
        if (!isOwner || jumpAction == null)
        {
            return;
        }

        if (jumpAction.WasPressedThisFrame())
        {
            rb.AddForce(Vector3.up * (rb.mass * jumpForce), ForceMode.Impulse);
        }
    }
}
