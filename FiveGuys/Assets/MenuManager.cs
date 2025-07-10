using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject logo;

    public void ToSettings()
    {
        canvas.GetComponent<Animator>().SetBool("inSettings", true);
        canvas.GetComponent<Animator>().SetBool("inMain", false);

        logo.GetComponent<Animator>().SetBool("inSettings", true);
        logo.GetComponent<Animator>().SetBool("inMain", false);
    }

    public void ToMain()
    {
        canvas.GetComponent<Animator>().SetBool("inSettings", false);
        canvas.GetComponent<Animator>().SetBool("inMain", true);

        logo.GetComponent<Animator>().SetBool("inSettings", false);
        logo.GetComponent<Animator>().SetBool("inMain", true);
    }

}
