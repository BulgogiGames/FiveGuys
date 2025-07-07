using UnityEngine;

public class WorkingStation : MonoBehaviour
{
    [SerializeField] private PlayerScript owner;
    [SerializeField] private LayerMask playerLayer;

    void OnTriggerEnter(Collider other)
    {
        GameObject collided = other.gameObject;

        if (playerLayer.Contains(collided) && collided == owner.gameObject)
        {
            owner.GetBackToWork();
        }
    }
}
