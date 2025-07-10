using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public enum PlayerState { Working, Distracted, Moving, Shitting, ClockingIn }

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private PlayerState playerState;
        public PlayerState PlayersState => playerState;
    private PlayerState lastState;
    [SerializeField] private PlayerState prevState;

    [Header("Character Navigation")]
    [SerializeField] private Transform target;
    [SerializeField] private Transform workingStation;
    [SerializeField] private LayerMask stationLayer;

    [Header("Player Movement")]
    [SerializeField] private NavMeshAgent player;
    [SerializeField] private InputActionAsset input;
    [SerializeField] private Transform playerPOV;
        public Transform PlayerPOV => playerPOV;
    private InputAction moveAction;
    private Vector2 moveAmount;

    
    //Debug Stuff
    [SerializeField] private CharacterStateDebug debugText;

    [Header("Working Stuff")]
    [SerializeField] private int roleID;
    [SerializeField] private float getPointEveryXSecs;
    [SerializeField] private float workPointCountDown;

    [Header("Distraction Stuff")]
    [SerializeField] private List<Transform> possibleActivities;
    [SerializeField] private float distractionCountDown;

    [SerializeField] private float minWaitForDistraction;
    [SerializeField] private float maxWaitForDistraction;

    [SerializeField] private bool hasChosenDistraction;

    [SerializeField] private bool hasToShit;
    [SerializeField] private float bathroomCountDown;
    [SerializeField] private Transform bathroomEntrance;

    
    [Header("Animation")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private bool backHead;
    [SerializeField] private Transform cameraFocus;
    [SerializeField] private List<GameObject> playerHeads;
    [SerializeField] private List<bool> animTrigger;

    void Awake()
    {
        moveAction = input.FindAction("Move");
        player = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        distractionCountDown = Random.Range(minWaitForDistraction, maxWaitForDistraction);
        prevState = playerState;
        playerState = PlayerState.Working;

        if (backHead)
        {
            //just for xaviers scarf 
            playerHeads[0].SetActive(false);
        }
        else if (!backHead)
        {
            playerHeads[1].SetActive(false);
        }
    }

    void Update()
    {
        //DebugSwitchState();
        FaceToCamera();
        if (playerState == PlayerState.Moving)
        {
            CameraIsFace();
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                playerState = prevState;
                debugText.TurnOnAndOff(true);
            }
        }        

        if (playerState != lastState)
        {
            if (debugText != null)
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
                    prevState = playerState;
                    playerState = PlayerState.Distracted;
                }

                if(workPointCountDown > 0)
                {
                    workPointCountDown -= Time.deltaTime;
                }

                if(workPointCountDown <= 0)
                {
                    //Award point here and reset the countdown
                    DevProgressPieChart.PC.UpdateProgress(1, roleID);
                    workPointCountDown = getPointEveryXSecs;
                }

                SetAnimation(0);
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

                SetAnimation(1);
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
                    transform.position = new Vector3(bathroomEntrance.position.x, 2, bathroomEntrance.position.z);

                    Debug.Log("Player has finished shitting, going back to work: " + transform.position);

                    prevState = playerState;
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
            prevState = playerState;
            playerState = PlayerState.Working;
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            prevState = playerState;
            playerState = PlayerState.Distracted;
        }
    }


    void SetAnimation(int index)
    {
        if(playerAnimator != null)
        {
            if (!animTrigger[index])
            {
                switch(index)
                {
                    case 0:
                        animTrigger[1] = false;
                        animTrigger[2] = false;
                        break;
                    case 1:
                        animTrigger[0] = false;
                        animTrigger[2] = false;
                        break;
                    case 2:
                        // animTrigger[0] = false;
                        // animTrigger[1] = false;
                        break;
                }
                
                animTrigger[index] = true;
            }
            
            playerAnimator.SetBool("Working", animTrigger[0]);
            playerAnimator.SetBool("Walking", animTrigger[1]);
            // playerAnimator.SetBool("Wanking", animTrigger[2]);
        }
    }


    void PossessedMovement()
    {
        moveAmount = moveAction.ReadValue<Vector2>();

        // Get forward/right based on current Y-rotation
        Vector3 moveDir = transform.forward * moveAmount.y + transform.right * moveAmount.x;

        Vector3 scaledMove = (player.speed * 2) * new Vector3(moveDir.x, 0f, moveDir.z) * Time.deltaTime;
        player.Move(scaledMove);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void GotClicked()
    {
        player.isStopped = true;
        player.ResetPath();


        if (debugText != null)
        {
            UpdateDebugText("Working");
        }

        
        prevState = playerState;
        playerState = PlayerState.Moving;
    }

    public void UpdateDebugText(string update)
    {
        debugText.UpdateText(update);
    }

    public void GetBackToWork()
    {
        distractionCountDown = Random.Range(minWaitForDistraction, maxWaitForDistraction);

        //Get the mouse back
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //Place the player at the working station
        transform.position = workingStation.position + new Vector3(0f, 2f, 0f);
        player.isStopped = true;

        hasChosenDistraction = false;

        //Start Working again
        prevState = playerState;
        playerState = PlayerState.Working;
    }

    public void GoneBathroom()
    {
        prevState = playerState;
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

    void FaceToCamera()
    {
        Vector3 toCamera = CameraController.CC.MainCamera.transform.position - cameraFocus.position;

        Vector3 right = Vector3.Cross(Vector3.up, toCamera.normalized * -1f).normalized;
        Vector3 correctedY = Vector3.Cross(right, Vector3.up).normalized;

        Quaternion baseRotation = Quaternion.LookRotation(Vector3.up, correctedY);
        cameraFocus.rotation = baseRotation * Quaternion.Euler(new Vector3(90f, 0f, 0f));
    }

    void CameraIsFace()
    {
        debugText.TurnOnAndOff(false);

        //rotate to face camera forward direction
        Vector3 camForward = CameraController.CC.MainCamera.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();
        transform.rotation = Quaternion.LookRotation(camForward);
    }
}
