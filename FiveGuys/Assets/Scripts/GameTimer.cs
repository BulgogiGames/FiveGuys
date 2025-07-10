using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float SetGameTime;
    private float gameTime;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private bool hasGameFinished;

    void Start()
    {
        gameTime = SetGameTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameTime > 0)
        {
            gameTime -= Time.deltaTime;
        }
        else
        {
            if (!hasGameFinished)
            {
                Debug.Log("game finished");
                hasGameFinished = true;
            }
            
        }

        SetTimerText();
    }

    void SetTimerText()
    {
        float minutes = Mathf.FloorToInt(gameTime / 60);
        float seconds = Mathf.FloorToInt(gameTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
