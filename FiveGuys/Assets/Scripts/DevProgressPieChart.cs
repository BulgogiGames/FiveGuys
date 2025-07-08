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
    [SerializeField] private List<PieSlice> slices = new List<PieSlice>();

    [Header("Development")]
    [SerializeField] private float ProgrammerProgress;
    [SerializeField] private float AnimatorProgress;
    [SerializeField] private float Art3DProgress;
    [SerializeField] private float Art2DProgress;
    [SerializeField] private float ComposerProgress;

    void Awake()
    {
        if (PC == null)
        {
            PC = this;
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
        float total = slices.Sum(s => s.progress);
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
                    slice.progress = ProgrammerProgress;
                    break;

                case "Animator":
                    slice.progress = AnimatorProgress;
                    break;

                case "3DArtist":
                    slice.progress = Art3DProgress;
                    break;

                case "2DArtist":
                    slice.progress = Art2DProgress;
                    break;

                case "Composer":
                    slice.progress = ComposerProgress;
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
                ProgrammerProgress += amount;
                break;

            case 2: //Animator ID
                AnimatorProgress += amount;
                break;
            
            case 3: //3D Art ID
                Art3DProgress += amount;
                break;
                
            case 4: //2D Art ID
                Art2DProgress += amount;
                break;

            case 5: //Composer ID
                ComposerProgress += amount;
                break;
        }
    }
}
