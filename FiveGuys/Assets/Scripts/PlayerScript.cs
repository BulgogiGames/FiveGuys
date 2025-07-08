using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour
{
    private enum PlayerState { Working, Distracted, Moving, Shitting, ClockingIn }
    [SerializeField] private PlayerState playerState;

    [Header("Character Navigation")]
    [SerializeField] private Transform target;
    [SerializeField] private Transform workingStation;
    [SerializeField] private LayerMask stationLayer;

    [Header("Player Movement")]
    [SerializeField] private NavMeshAgent player;
    [SerializeField] private InputActionAsset input;
    private InputAction moveAction;
    private Vector2 moveAmount;

    [SerializeField] private bool isControlled;
    public bool IsControlled { get { return isControlled; } set { isControlled = value; } }

    [SerializeField] private CharacterStateDebug DebugText;

    [Header("Distraction Stuff")]
    [SerializeField] private List<Transform> possibleActivities;
    [SerializeField] private float distractionCountDown;

    [SerializeField] private float minWaitForDistraction;
    [SerializeField] private float maxWaitForDistraction;

    [SerializeField] private bool hasChosenDistraction;

    [SerializeField] private bool hasToShit;
    [SerializeField] private float bathroomCountDown;
    [SerializeField] private Transform bathroomEntrance;

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
            if (DebugText != null)
            {
                UpdateDebugText(playerState.ToString());
            }

            lastState = playerState;
        }

        switch (playerState)
        {
            case PlayerState.Working:
                player.isStopped = true;

                if (distractionCountDown > 0)
                {
                    distractionCountDown -= Time.deltaTime;
                }

                if (distractionCountDown <= 0)
                {
                    playerState = PlayerState.Distracted;
                }

                break;

            case PlayerState.Distracted:
                //Choose distraction
                if(!hasChosenDistraction)
                {
                    int chosenDistraction = Random.Range(0, possibleActivities.Count);

                    if(chosenDistraction == 1)
                    {
                        hasToShit = true;
                    }

                    target = possibleActivities[chosenDistraction];
                    hasChosenDistraction = true;
                }
                
                player.SetDestination(target.position);
                player.isStopped = false;
                break;

            case PlayerState.Moving:
                PossessedMovement();
                break;

            case PlayerState.Shitting:
                player.isStopped = true;
                
                if(bathroomCountDown > 0)
                {
                    bathroomCountDown -= Time.deltaTime;
                }

                if(bathroomCountDown <= 0)
                {
                    transform.position = new Vector3(bathroomEntrance.position.x, 1, bathroomEntrance.position.z);

                    playerState = PlayerState.ClockingIn;
                }
                break;

            case PlayerState.ClockingIn:
                player.isStopped = false;
                player.SetDestination(workingStation.position);
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

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void GotClicked()
    {
        player.isStopped = true;
        player.ResetPath();


        if (DebugText != null)
        {
            UpdateDebugText("Working");
        }

        

        playerState = PlayerState.Moving;
    }

    public void UpdateDebugText(string update)
    {
        DebugText.UpdateText(update);
    }

    public void GetBackToWork()
    {
        distractionCountDown = Random.Range(minWaitForDistraction, maxWaitForDistraction);

        //Get the mouse back
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //Place the player at the working station
        transform.position = workingStation.position;
        player.isStopped = true;

        hasChosenDistraction = false;

        //Start Working again
        playerState = PlayerState.Working;
    }

    public void GoneBathroom()
    {
        playerState = PlayerState.Shitting;
    }

    public bool HasToShit()
    {
        return hasToShit;
    }

    public void EnteredBathroom(Transform bathroomDoor, float bathroomTime)
    {
        bathroomEntrance = bathroomDoor;

        bathroomCountDown = bathroomTime;

        hasToShit = false;
    }
}
