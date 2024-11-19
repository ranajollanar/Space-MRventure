using UnityEngine;
using UnityEngine.UI;

public class MuteToggle : MonoBehaviour
{
    private bool isMuted = false; // Flag to track mute state
    private AudioSource[] audioSources; // Array to store all audio sources in the scene

    void Start()
    {
        // Find all AudioSource components in the scene
        audioSources = FindObjectsOfType<AudioSource>();
    }

    public void ToggleMute()
    {
        // Toggle the mute state
        isMuted = !isMuted;

        // Set the mute state for all audio sources
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.mute = isMuted;
        }
    }
}
