using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("main stuff")]
        [SerializeField] private Camera mainCamera;

        [SerializeField] private InputActionAsset input;
        private InputAction mouseDeltaAction;
        private InputAction mouseScrollAction;

        private enum CameraStates { Overhead, POV }
        [SerializeField] private CameraStates camState;


    [Header("rotation stuff")]
        [SerializeField] private float mouseSens;
        [SerializeField] private float lookXLimit;//, lookYLimit;
        private float rotationX, rotationY;
        

    [Header("zoom stuff")]
        [SerializeField] private float zoomSpeed;
        private float zoomNum;
        [SerializeField] private float minHeight, maxHeight;

    [Header("movement stuff")]
        [SerializeField] private LayerMask borderLayer;


    void Awake()
    {
        mainCamera = GetComponent<Camera>();
        mouseDeltaAction = input.FindAction("Look");
        mouseScrollAction = input.FindAction("Zoom");
    }

    void Start()
    {
        camState = CameraStates.Overhead;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        switch(camState)
        {
            case CameraStates.Overhead:
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    CameraClick();
                }

                //CameraMovement();
                CameraRotation();
                CameraZoom();
                break;

            case CameraStates.POV:
                //
                break;
        }
        
    }

    void CameraClick()
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
                    //Debug.Log("im hitting");
                }
            }
            else
            {
                //Debug.Log("We dont hit anything");
            }
    }

    void CameraMovement()
    {

    }

    void CameraRotation()
    {
        Vector2 mouseDelta = mouseDeltaAction.ReadValue<Vector2>();

        //move up/down
        if (transform.position.y < maxHeight / 2)
        {
            rotationX -= mouseDelta.y * (mouseSens / 10);
            rotationX = Mathf.Clamp(rotationX, 0, lookXLimit);
        }
        else
        {
            rotationX = lookXLimit;
        }

        //move left/right
        rotationY += mouseDelta.x * (mouseSens / 10);
        //rotationY = Mathf.Clamp(rotationY, -lookYLimit, lookYLimit);

        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
    }

    void CameraZoom()
    {
        float scrollValue = mouseScrollAction.ReadValue<Vector2>().y;
        zoomNum = scrollValue * (zoomSpeed * 10);

        if (zoomNum != 0)
        {
            Vector3 zoomDirection = transform.forward * zoomNum * Time.deltaTime;
            Vector3 newPosition = transform.position + zoomDirection;

            transform.position = new Vector3(newPosition.x, Mathf.Clamp(newPosition.y, minHeight, maxHeight), newPosition.z);
        }
    }
}
