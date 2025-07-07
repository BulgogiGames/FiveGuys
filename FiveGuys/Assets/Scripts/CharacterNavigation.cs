using UnityEngine;
using UnityEngine.AI;

public class CharacterNavigation : MonoBehaviour
{
    [SerializeField] private Transform target;

    private NavMeshAgent agent;

    void Start() 
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Update() 
    {
        agent.SetDestination(target.position);    
    }
}
