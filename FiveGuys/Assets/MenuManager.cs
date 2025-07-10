using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject logo;
    [SerializeField] private AudioClip settingsClip;
    [SerializeField] private AudioClip selectClip;
    [SerializeField] private AudioClip hoverClip;

    public void ToSettings()
    {
        canvas.GetComponent<Animator>().SetBool("inSettings", true);
        canvas.GetComponent<Animator>().SetBool("inMain", false);

        logo.GetComponent<Animator>().SetBool("inSettings", true);
        logo.GetComponent<Animator>().SetBool("inMain", false);

        SFXManager.instance.PlayGlobalSFX(settingsClip, this.transform, 1f);
    }

    public void ToMain()
    {
        canvas.GetComponent<Animator>().SetBool("inSettings", false);
        canvas.GetComponent<Animator>().SetBool("inMain", true);

        logo.GetComponent<Animator>().SetBool("inSettings", false);
        logo.GetComponent<Animator>().SetBool("inMain", true);

        SFXManager.instance.PlayGlobalSFX(selectClip, this.transform, 1f);
    }

    public void loadScene()
    {
        SFXManager.instance.PlayGlobalSFX(selectClip, this.transform, 1f);
        SceneManager.LoadScene("Scene");
    }

    public void hoverSFX()
    {
        SFXManager.instance.PlayGlobalSFX(hoverClip, this.transform, 0.6f);
    }    

}
