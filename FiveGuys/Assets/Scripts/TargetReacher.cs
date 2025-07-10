using UnityEngine;

public class TargetReacher : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;

    void OnTriggerEnter(Collider other)
    {
        GameObject collided = other.gameObject;

        if (playerLayer.Contains(collided))
        {
            collided.transform.GetComponent<PlayerScript>().ReachedLocations();
        }
    }
}
