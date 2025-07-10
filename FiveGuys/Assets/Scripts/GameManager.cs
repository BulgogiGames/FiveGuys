using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager GM { get; private set; }

    [SerializeField] private GameTimer timer;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private bool setValues;
    [SerializeField] private Transform newChartLoc;


    [SerializeField] private TMP_Text programming;
    [SerializeField] private TMP_Text threeDArt;
    [SerializeField] private TMP_Text twoDArt;
    [SerializeField] private TMP_Text music;
    [SerializeField] private TMP_Text animating;


    void Awake()
    {
        if (GM == null)
        {
            GM = this;
            DontDestroyOnLoad(this);
        }
        else if (GM != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        endPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.HasGameFinished)
        {
            
            if (!setValues)
            {
                SetText();
                endPanel.SetActive(true);
                DevProgressPieChart.PC.SetNewLoc(newChartLoc);
                setValues = true;
            }
            
        }
    }

    void SetText()
    {
        DevProgressPieChart pieC = DevProgressPieChart.PC;
        float pieTotal = pieC.ProgrammerProgress + pieC.AnimatorProgress + pieC.Art2DProgress + pieC.Art3DProgress + pieC.ComposerProgress;
        
        programming.text = "Programming Contribution: " + (pieC.ProgrammerProgress / pieTotal * 100).ToString() + "%";
        threeDArt.text = "3D Art Contribution: " + (pieC.Art3DProgress / pieTotal * 100).ToString() + "%";
        twoDArt.text = "2D Art Contribution: " + (pieC.Art2DProgress / pieTotal * 100).ToString() + "%";
        music.text = "Music Contribution: " + (pieC.ComposerProgress / pieTotal * 100).ToString() + "%";
        animating.text = "Animation Contribution: " + (pieC.AnimatorProgress / pieTotal * 100).ToString() + "%";
    }

    public void Reboot()
    {
        SceneManager.LoadScene("Menu");
    }
}
