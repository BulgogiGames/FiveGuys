using System;
using TMPro;
using UnityEngine;

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
        float pieTotal = pieC.SliceAmounts[0].fillAmount + pieC.SliceAmounts[1].fillAmount + pieC.SliceAmounts[2].fillAmount + pieC.SliceAmounts[3].fillAmount + pieC.SliceAmounts[4].fillAmount;
        
        programming.text = "Programming Contribution: " + (pieC.SliceAmounts[0].fillAmount / pieTotal * 100).ToString() + "%";
        threeDArt.text = "3D Art Contribution: " + (pieC.SliceAmounts[1].fillAmount / pieTotal * 100).ToString() + "%";
        twoDArt.text = "2D Art Contribution: " + (pieC.SliceAmounts[2].fillAmount / pieTotal * 100).ToString() + "%";
        music.text = "Music Contribution: " + (pieC.SliceAmounts[3].fillAmount / pieTotal * 100).ToString() + "%";
        animating.text = "Animation Contribution: " + (pieC.SliceAmounts[4].fillAmount / pieTotal * 100).ToString() + "%";
    }
}
