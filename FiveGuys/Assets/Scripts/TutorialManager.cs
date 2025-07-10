using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    public static TutorialManager TutorialMan { get; private set; }

    public bool tutorialDone;

    [TextArea(2, 4)]
    public string[] tutorialSteps;

    [TextArea(2, 4)]
    public string[] learningSteps;

    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialText;

    public GameObject learningPanel;
    public TextMeshProUGUI learningTexts;

    [SerializeField] private int currentStep = 0;
    [SerializeField] private int currLearningStep = 0;
    private bool tutorialActive = false;

    void Awake()
    {
        if (TutorialMan == null)
        {
            TutorialMan = this;
        }
        else if (TutorialMan != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (tutorialSteps.Length > 0)
        {
            ShowTutorial();
        }
    }

    void ShowTutorial()
    {
        tutorialActive = true;
        Time.timeScale = 0f;
        tutorialPanel.SetActive(true);
        learningPanel.SetActive(false);
        tutorialText.text = tutorialSteps[currentStep];

        if (currentStep == 2)
        {
            camera.transform.position = new Vector3(13.25f, 7, 10.30f);
        }
    }

    public void ContinueButtonClicked()
    {
        if (!tutorialActive) return;
        AdvanceTutorial();
    }

    void AdvanceTutorial()
    {
        currentStep++;

        if (currentStep == 2)
        {
            ShowLearning();
        } else if (currentStep < tutorialSteps.Length)
        {
            tutorialText.text = tutorialSteps[currentStep];
        }
        else
        {
            EndTutorial();
        }
    }

    void ShowLearning()
    {
        learningPanel.SetActive(true);
        tutorialPanel.SetActive(false);
        Time.timeScale = 1f;
        learningTexts.text = learningSteps[currLearningStep];
    }

    public void AdvanceLearning()
    {
        ShowTutorial();
    }

    void EndTutorial()
    {
        tutorialActive = false;
        Time.timeScale = 1f;
        tutorialPanel.SetActive(false);
    }
}
