using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();

            Ray myRay = mainCamera.ScreenPointToRay(mousePos);

            RaycastHit hit;

            bool weHitSomething = Physics.Raycast(myRay, out hit);

            if (weHitSomething)
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    hit.transform.GetComponent<PlayerScript>().GotClicked();
                    Debug.Log("im hitting");
                }
            }
            else
            {
                Debug.Log("We dont hit anything");
            }
        }
    }
}
