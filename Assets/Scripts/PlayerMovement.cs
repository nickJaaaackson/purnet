using PurrNet;
using PurrNet.Prediction;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : PredictedIdentity<PlayerMovement.Input, PlayerMovement.State>
{
    [SerializeField] private PredictedRigidbody rb;
    [SerializeField] private float moveForce = 5f;
    [SerializeField] private PlayerInput playerInput;
    private InputAction moveAction;

    public override void OnPreSetup()
    {
        base.OnPreSetup();
        enabled = isOwner;

        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }

        playerInput.enabled = false;
        playerInput.enabled = enabled;
        moveAction = playerInput.actions["Move"];

    }
    protected override void Simulate(Input input, ref State state, float delta)
    {
        Vector3 moveDirection = new Vector3(input.direction.x, 0, input.direction.y).normalized * moveForce;
        rb.AddForce(moveDirection);
    }

    protected override void GetFinalInput(ref Input input)
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        input.direction = new Vector2(moveInput.x, moveInput.y);
    }

    public struct Input : IPredictedData
    {
        public Vector2 direction;
        public void Dispose()
        {

        }
    }

    public struct State : IPredictedData<State>
    {
        public void Dispose()
        {

        }
    }
}
