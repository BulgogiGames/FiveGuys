using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerWalk : MonoBehaviour
{
    [SerializeField] private NavMeshAgent player;
    [SerializeField] private InputActionAsset input;
    [SerializeField] private InputAction moveAction;
    [SerializeField] private Vector2 moveAmount;

    void Awake()
    {
        moveAction = input.FindAction("Move");
    }
    void Update()
    {
        moveAmount = moveAction.ReadValue<Vector2>();

        Vector3 scaledMove = player.speed * Time.deltaTime * new Vector3(moveAmount.x, 0, moveAmount.y);

        player.Move(scaledMove);
    }
}
