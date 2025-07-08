using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    [Header("main stuff")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private InputActionAsset input;

        private enum CameraStates { Overhead, POV }
        [SerializeField] private CameraStates camState;


    [Header("rotation stuff")]
        [SerializeField] private float rotateSpeed;
        [SerializeField] private float lookXLimit;
        private float rotationX, rotationY;
        private InputAction mouseTurnAction;
        

    [Header("zoom stuff")]
        [SerializeField] private float zoomSpeed;
        private float zoomNum;
        [SerializeField] private float minHeight, maxHeight;
        private InputAction mouseZoomAction;

    [Header("movement stuff")]
        [SerializeField] private LayerMask borderLayer; 
        [SerializeField] private float moveSpeed;
        private InputAction cameraMoveAction;
        [SerializeField] private List<bool> forwardDir;


    void Awake()
    {
        mainCamera = GetComponent<Camera>();
        mouseTurnAction = input.FindAction("Turn");
        mouseZoomAction = input.FindAction("Zoom");
        cameraMoveAction = input.FindAction("Move");
    }

    void Start()
    {
        camState = CameraStates.Overhead;
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

                CameraMovement();
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
        Vector3 move = cameraMoveAction.ReadValue<Vector2>();
        Vector3 scaledMove = new Vector3();
        IsFlipped();

        if (forwardDir[0])
        {
            //z+
            scaledMove = moveSpeed * Time.deltaTime * new Vector3(move.x, 0, move.y);            
        }
        if (forwardDir[1])
        {
            //z-
            scaledMove = -moveSpeed * Time.deltaTime * new Vector3(move.x, 0, move.y);
        }
        if (forwardDir[2])
        {
            //x+
            scaledMove = moveSpeed * Time.deltaTime * new Vector3(move.y, 0, -move.x);
        }
        if (forwardDir[3])
        {
            //x-
            scaledMove = -moveSpeed * Time.deltaTime * new Vector3(move.y, 0, -move.x);
        }
        
        Vector3 newPosition = transform.position + scaledMove;
        transform.position = newPosition;
    }

    void IsFlipped()
    {
        Vector3 forward = transform.forward;

        if (Vector3.Dot(forward, Vector3.forward) > 0.5f)
        {
            SwitchBool(0);
            //Debug.Log("Facing World Forward (Z+)");
        }
        else if (Vector3.Dot(forward, Vector3.back) > 0.5f) 
        {
            SwitchBool(1);
            //Debug.Log("Facing World Backward (Z-)");
        } 
        else if (Vector3.Dot(forward, Vector3.right) > 0.5f) 
        {
            SwitchBool(2);
            //Debug.Log("Facing World Right (X+)");
        }
        else if (Vector3.Dot(forward, Vector3.left) > 0.5f) 
        {
            SwitchBool(3);
            //Debug.Log("Facing World Left (X-)");
        }
    }

    void SwitchBool(int index) 
    {
        switch(index)
        {
            case 0:
                forwardDir[1] = false;
                forwardDir[2] = false;
                forwardDir[3] = false;
                break;
            case 1:
                forwardDir[0] = false;
                forwardDir[2] = false;
                forwardDir[3] = false;
                break;
            case 2:
                forwardDir[0] = false;
                forwardDir[1] = false;
                forwardDir[3] = false;
                break;
            case 3:
                forwardDir[0] = false;
                forwardDir[1] = false;
                forwardDir[2] = false;
                break;
        }

        forwardDir[index] = true;
    }

    void CameraRotation()
    {
        Vector2 moveAmount = mouseTurnAction.ReadValue<Vector2>();

        //move up/down
        if (transform.position.y < maxHeight)
        {
            rotationX -= moveAmount.y * (rotateSpeed / 10);
            rotationX = Mathf.Clamp(rotationX, 0, lookXLimit);
        }
        else
        {
            rotationX = lookXLimit;
        }

        //move left/right
        rotationY += moveAmount.x * (rotateSpeed / 10);
        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
    }

    void CameraZoom()
    {
        float scrollValue = mouseZoomAction.ReadValue<Vector2>().y;
        zoomNum = scrollValue * zoomSpeed;

        if (zoomNum != 0)
        {
            Vector3 zoomDirection = transform.forward * zoomNum * Time.deltaTime;
            Vector3 newPosition = transform.position + zoomDirection;

            transform.position = new Vector3(newPosition.x, Mathf.Clamp(newPosition.y, minHeight, maxHeight), newPosition.z);
        }
    }
}
