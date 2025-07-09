using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour
{
    private enum PlayerState { Working, Distracted, Moving, Shitting, ClockingIn }
    [SerializeField] private PlayerState playerState;
    [SerializeField] private bool isControlled;
        public bool IsControlled { get { return isControlled; }  set { isControlled = value; } }

    [Header("Character Navigation")]
    [SerializeField] private Transform target;
    [SerializeField] private Transform workingStation;
    [SerializeField] private LayerMask stationLayer;

    [Header("Player Movement")]
    [SerializeField] private NavMeshAgent player;
    [SerializeField] private InputActionAsset input;
    private InputAction moveAction;
    private Vector2 moveAmount;

    
    //Debug Stuff
    private PlayerState lastState;
    [SerializeField] private CharacterStateDebug DebugText;

    [Header("Working Stuff")]
    [SerializeField] private int roleID;
    [SerializeField] private float getPointEveryXSecs;
    [SerializeField] private float workPointCountDown;

    [Header("Distraction Stuff")]
    [SerializeField] private List<Transform> possibleActivities;
    [SerializeField] private float distractionCountDown;

    [SerializeField] private float minWaitForDistraction;
    [SerializeField] private float maxWaitForDistraction;

    
    [Header("Animation")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private bool backHead;
    [SerializeField] private Transform cameraFocus;
    [SerializeField] private List<GameObject> playerHeads;
    [SerializeField] private List<bool> animTrigger;

    [SerializeField] private bool hasChosenDistraction;

    [SerializeField] private bool hasToShit;
    [SerializeField] private float bathroomCountDown;
    [SerializeField] private Transform bathroomEntrance;

    void Awake()
    {
        moveAction = input.FindAction("Move");
        player = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        distractionCountDown = Random.Range(minWaitForDistraction, maxWaitForDistraction);
        playerState = PlayerState.Working;

        if (backHead)
        {
            //just for xaviers scarf 
            playerHeads[1].SetActive(false);
        }
        else if (!backHead)
        {
            playerHeads[0].SetActive(false);
        }
    }

    void Update()
    {
        //DebugSwitchState();
        FaceCamera();

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
                isControlled = false;
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
                isControlled = false;
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

    void FaceCamera()
    {
        Vector3 toCamera = CameraController.CC.MainCamera.transform.position - cameraFocus.position;

        Vector3 right = Vector3.Cross(Vector3.up, toCamera.normalized * -1f).normalized;
        Vector3 correctedY = Vector3.Cross(right, Vector3.up).normalized;

        Quaternion baseRotation = Quaternion.LookRotation(Vector3.up, correctedY);
        cameraFocus.rotation = baseRotation * Quaternion.Euler(new Vector3(90f, 0f, 0f));
    }
}
