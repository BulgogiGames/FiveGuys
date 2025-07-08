using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
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
    public void PlaySFX(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // Spawn in gameobject
        AudioSource audioSource =  Instantiate(sfxObject, spawnTransform.position, Quaternion.identity);

        // Assign audio clip
        audioSource.clip = audioClip;

        // Assign audio volume
        audioSource.volume = volume;

        // Play audio clip
        audioSource.Play();

        // Get length of audio clip
        float clipLength = audioSource.clip.length;

        // Destroy object after time elapsed
        Destroy(audioSource.gameObject, clipLength);
    }

    // Play random SFX from array
    public void PlayRandomSFX(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        int rand = Random.Range(0, audioClip.Length);

        // Spawn in gameobject
        AudioSource audioSource =  Instantiate(sfxObject, spawnTransform.position, Quaternion.identity);

        // Assign audio clip
        audioSource.clip = audioClip[rand];

        // Assign audio volume
        audioSource.volume = volume;

        // Play audio clip
        audioSource.Play();

        // Get length of audio clip
        float clipLength = audioSource.clip.length;

        // Destroy object after time elapsed
        Destroy(audioSource.gameObject, clipLength);
    }
}
