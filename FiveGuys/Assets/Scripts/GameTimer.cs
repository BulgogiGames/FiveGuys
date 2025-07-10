using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float SetGameTime;
    private float gameTime;
    [SerializeField] private GameObject panel;
    [SerializeField] private bool hasGameFinished;
        public bool HasGameFinished => hasGameFinished;

    void Start()
    {
        gameTime = SetGameTime + 1;
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
                SceneManager.LoadScene("END SCENE");
                panel.SetActive(false);
                hasGameFinished = true;
            }
            
        }

        SetTimerText();
    }

    void SetTimerText()
    {
        float minutes = Mathf.FloorToInt(gameTime / 60);
        float seconds = Mathf.FloorToInt(gameTime % 60);
        panel.GetComponentInChildren<TMP_Text>().text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
