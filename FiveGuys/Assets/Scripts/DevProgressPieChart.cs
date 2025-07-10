using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class DevProgressPieChart : MonoBehaviour
{
    [System.Serializable]
    public class PieSlice
    {
        public string roleName;
        public Image sliceImage;
        public float progress;
    }

    public static DevProgressPieChart PC { get; private set; }


    [Header("PieSlices")]
    [SerializeField] private Transform pieChart;
    [SerializeField] private List<PieSlice> slices = new List<PieSlice>();

    [Header("Development")]
    [SerializeField] private float programmerProgress;
        public float ProgrammerProgress => programmerProgress;
    [SerializeField] private float animatorProgress;
        public float AnimatorProgress => animatorProgress;
    [SerializeField] private float art3DProgress;
        public float Art3DProgress => art3DProgress;
    [SerializeField] private float art2DProgress;
        public float Art2DProgress => art2DProgress;
    [SerializeField] private float composerProgress;
        public float ComposerProgress => composerProgress;
    
    [SerializeField] private float total;
    public float Total => total;

    [SerializeField] private List<Image> sliceAmounts;
        public List<Image> SliceAmounts => sliceAmounts;

    void Awake()
    {
        if (PC == null)
        {
            PC = this;
            DontDestroyOnLoad(this);
        }
        else if (PC != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        UpdateChart();
    }

    public void UpdateChart()
    {
        total = slices.Sum(s => s.progress);
        if(total <= 0)
        {
            return;
        }

        float cumulativeRotation = 0f;

        foreach(var slice in slices)
        {
            switch(slice.roleName)
            {
                case "Programmer":
                    slice.progress = programmerProgress;
                    break;

                case "Animator":
                    slice.progress = animatorProgress;
                    break;

                case "3DArtist":
                    slice.progress = art3DProgress;
                    break;

                case "2DArtist":
                    slice.progress = art2DProgress;
                    break;

                case "Composer":
                    slice.progress = composerProgress;
                    break;
            }

            float normalized = slice.progress / total;

            slice.sliceImage.fillAmount = normalized;
            slice.sliceImage.rectTransform.rotation = Quaternion.Euler(0, 0, -cumulativeRotation * 360f);

            cumulativeRotation += normalized;
        }
    }

    public void UpdateProgress(float amount, int id)
    {
        switch(id)
        {
            case 1: //Programmer ID
                programmerProgress += amount;
                break;

            case 2: //Animator ID
                animatorProgress += amount;
                break;
            
            case 3: //3D Art ID
                art3DProgress += amount;
                break;
                
            case 4: //2D Art ID
                art2DProgress += amount;
                break;

            case 5: //Composer ID
                composerProgress += amount;
                break;
        }
    }

    public void SetNewLoc(Transform newParent)
    {
        pieChart.position = newParent.position;
        pieChart.SetParent(newParent);
        pieChart.localScale *= 2;
    }
}
