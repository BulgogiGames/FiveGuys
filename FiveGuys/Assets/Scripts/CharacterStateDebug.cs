using UnityEngine;
using TMPro;

public class CharacterStateDebug : MonoBehaviour
{
    [SerializeField] private Transform playerLocation;
    [SerializeField] private Vector3 offset = new Vector3();
    [SerializeField] private TextMeshProUGUI stateText;

    void Update()
    {
        Vector3 localOffset = transform.right * offset.x + transform.up * offset.y + transform.forward * offset.z;
        transform.position = playerLocation.position + localOffset;

        Vector3 toCamera = CameraController.CC.MainCamera.transform.position - transform.position;
        Quaternion baseRotation = Quaternion.LookRotation(Vector3.up, toCamera);
        transform.rotation = baseRotation * Quaternion.Euler(new Vector3(90f, 0f, 0f));
    }

    public void UpdateText(string text)
    {
        stateText.text = text;
    }
}
