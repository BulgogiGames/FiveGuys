using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    private enum PlayerState {Working, Distracted}
    [SerializeField] private PlayerState playerState;

    [Header("Character Navigation")]
    [SerializeField] private Transform target;
    [SerializeField] private Transform workingStation;

    [Header("Player Movement")]
    [SerializeField] private NavMeshAgent player;
    [SerializeField] private InputActionAsset input;
    private InputAction moveAction;
    private Vector2 moveAmount;

    [SerializeField] private bool isControlled;
        public bool IsControlled { get { return isControlled; }  set { isControlled = value; } }

    [SerializeField] private CharacterStateDebug DebugText;

    [Header("Distraction Stuff")]
    [SerializeField] private float distractionCountDown;

    [SerializeField] private float minWaitForDistraction;
    [SerializeField] private float maxWaitForDistraction;

    //Debug Stuff
    private PlayerState lastState;
    void Awake()
    {
        moveAction = input.FindAction("Move");
        player = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        distractionCountDown = Random.Range(minWaitForDistraction, maxWaitForDistraction);
        playerState = PlayerState.Working;
    }
    
    void Update()
    {
        DebugSwitchState();

        if (playerState != lastState)
        {
            if(DebugText != null)
            {
                UpdateDebugText(playerState.ToString());
            }
            
            lastState = playerState;
        }

        switch(playerState)
        {
            case PlayerState.Working:
                player.isStopped = true;
                
                if(distractionCountDown > 0)
                {
                    distractionCountDown -= Time.deltaTime;
                }

                if(distractionCountDown <= 0)
                {
                    playerState = PlayerState.Distracted;
                }

                break;

            case PlayerState.Distracted:
                player.SetDestination(target.position);
                player.isStopped = false;
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
    }



    void PossessedMovement()
    {
        moveAmount = moveAction.ReadValue<Vector2>();

        Vector3 scaledMove = player.speed * Time.deltaTime * new Vector3(moveAmount.x, 0, moveAmount.y);

        player.Move(scaledMove);
    }

    public void GotClicked()
    {
        player.isStopped = true;

        transform.position = workingStation.position + new Vector3(0,0.94f,0);

        if(DebugText != null)
        {
            UpdateDebugText("Working");
        }

        distractionCountDown = Random.Range(minWaitForDistraction, maxWaitForDistraction);

        playerState = PlayerState.Working;
    }

    public void UpdateDebugText(string update)
    {
        DebugText.UpdateText(update);
    }
}
