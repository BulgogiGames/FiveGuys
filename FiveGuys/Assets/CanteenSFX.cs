using System.Collections;
using UnityEngine;

public class CanteenSFX : MonoBehaviour
{
    [SerializeField] private float minWait;
    [SerializeField] private float maxWait;
    [SerializeField] private AudioClip[] audioClips;
    
    void Start()
    {
        StartCoroutine(TriggerSFXRandomly());
    }

    IEnumerator TriggerSFXRandomly()
    {
        // Set random time
        float waitTime = Random.Range(minWait, maxWait);

        // Wait for random time
        yield return new WaitForSeconds(waitTime);

        // Play SFX
        SFXManager.instance.PlayRandomSFX(audioClips, this.transform, 1f);

        //Re-trigger coroutine
        StartCoroutine(TriggerSFXRandomly());
    }
}
