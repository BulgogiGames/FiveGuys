using UnityEngine;

public class SFXManager : MonoBehaviour
{
    private static SFXManager instance;
    [SerializeField] private AudioSource sfxObject;

    // Plays on component load
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Plays SFX at point
    public void playSFX(AudioClip audioClip, Transform spawnTransform)
    {
        // Spawn in gameobject
        AudioSource audioSource =  Instantiate(sfxObject, spawnTransform.position, Quaternion.identity);
        // Assign audio source component

        // Assign audio clip

        // Play audio clip

        // Get length of audio clip

        // Destroy object after time elapsed
    }
}
