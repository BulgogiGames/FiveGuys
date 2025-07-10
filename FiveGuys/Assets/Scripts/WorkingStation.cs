using Unity.VisualScripting;
using UnityEngine;

public class WorkingStation : MonoBehaviour
{
    [Header("Station Variables")]
    [SerializeField] private LayerMask playerLayer;

    [Header("Working Variables")]
    [SerializeField] private PlayerScript owner;

    [Header("Bathroom Variables")]
    [SerializeField] private Transform hidingZone;
    [SerializeField] private bool isBathroom;
    [SerializeField] private float bathroomStayTime;

    void OnTriggerEnter(Collider other)
    {
        GameObject collided = other.gameObject;

        if(owner != null)
        {
            if(playerLayer.Contains(collided) && collided == owner.gameObject && !isBathroom)
            {
                if (owner.PlayersState == PlayerState.Moving)
                {
                    owner.GetBackToWork();
                }
            }
        }
        
        if(isBathroom && collided.transform.GetComponent<PlayerScript>().HasToShit())
        {
            collided.transform.position = hidingZone.position;

            collided.transform.GetComponent<PlayerScript>().GoneBathroom();
            collided.transform.GetComponent<PlayerScript>().EnteredBathroom(transform, bathroomStayTime);
        }
    }
}
