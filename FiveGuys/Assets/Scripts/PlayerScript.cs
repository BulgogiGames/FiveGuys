using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour
{
    private enum PlayerState {Working, Distracted, Controlled}
    [SerializeField] private PlayerState playerState;

    [Header("Player Movement")]
    [SerializeField] private NavMeshAgent player;
    [SerializeField] private InputActionAsset input;
    private InputAction moveAction;
    private Vector2 moveAmount;

    [SerializeField] private bool isControlled;
        public bool IsControlled { get { return isControlled; }  set { isControlled = value; } }

    [Header("Animation")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private List<bool> animTrigger;

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
                SetAnimation(0);
                isControlled = false;
                break;
            case PlayerState.Distracted:
                SetAnimation(1);
                isControlled = false;
                break;
            case PlayerState.Controlled:
                
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

    void SetAnimation(int index)
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
                    //
                    break;
            }
            
            animTrigger[index] = true;
        }
        
        playerAnimator.SetBool("Working", animTrigger[0]);
        playerAnimator.SetBool("Walking", animTrigger[1]);
    }


    void PossessedMovement()
    {
        moveAmount = moveAction.ReadValue<Vector2>();

        Vector3 scaledMove = player.speed * Time.deltaTime * new Vector3(moveAmount.x, 0, moveAmount.y);

        player.Move(scaledMove);
    }
}
