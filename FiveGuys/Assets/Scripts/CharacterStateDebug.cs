using UnityEngine;
using TMPro;

public class CharacterStateDebug : MonoBehaviour
{
    [SerializeField] private Transform playerLocation;
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, 0);
    [SerializeField] private TextMeshProUGUI stateText;

    void Update()
    {
        transform.position = playerLocation.position + offset;

    }

    public void UpdateText(string text)
    {
        stateText.text = text;
    }
}
