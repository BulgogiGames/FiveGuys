using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    private enum PlayerState {Working, Distracted, Controlled}
    private PlayerState playerState;


    [Header("Player Movement")]
    [SerializeField] private NavMeshAgent player;
    [SerializeField] private InputActionAsset input;
    private InputAction moveAction;
    private Vector2 moveAmount;

    void Awake()
    {
        moveAction = input.FindAction("Move");
        player = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        playerState = PlayerState.Controlled;
    }
    
    void Update()
    {
        DebugSwitchState();

        switch(playerState)
        {
            case PlayerState.Working:
                Debug.Log("im working");
                break;
            case PlayerState.Distracted:
                Debug.Log("im distracted");
                break;
            case PlayerState.Controlled:
            Debug.Log("im controlled");
                PossessedMovement();
                break;
        }
    }

    void DebugSwitchState()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            playerState = PlayerState.Working;
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            playerState = PlayerState.Distracted;
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            playerState = PlayerState.Controlled;
        }
    }


    void PossessedMovement()
    {
        moveAmount = moveAction.ReadValue<Vector2>();

        Vector3 scaledMove = player.speed * Time.deltaTime * new Vector3(moveAmount.x, 0, moveAmount.y);

        player.Move(scaledMove);
    }
}
