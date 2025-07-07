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

    [SerializeField] private bool isControlled;
        public bool IsControlled { get { return isControlled; }  set { isControlled = value; } }

    [Header("Animation")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private SpriteRenderer[] playerSprites;
        public SpriteRenderer[] PlayerSprites => playerSprites;

    void Awake()
    {
        moveAction = input.FindAction("Move");
        player = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        playerState = PlayerState.Working;
    }
    
    void Update()
    {
        DebugSwitchState();
        SwitchState();

        switch(playerState)
        {
            case PlayerState.Working:
                Debug.Log("im working");
                isControlled = false;
                break;
            case PlayerState.Distracted:
                Debug.Log("im distracted");
                isControlled = false;
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

    void SwitchState()
    {
        if (playerState == PlayerState.Distracted && isControlled)
        {
            playerState = PlayerState.Controlled;
            isControlled = false;
        }
    }


    void PossessedMovement()
    {
        moveAmount = moveAction.ReadValue<Vector2>();

        Vector3 scaledMove = player.speed * Time.deltaTime * new Vector3(moveAmount.x, 0, moveAmount.y);

        player.Move(scaledMove);
    }
}
