using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlaySequentialAudios : MonoBehaviour
{
    public AudioClip[] audioClips; // Array of audio clips to play
    private AudioSource audioSource; // Reference to the AudioSource component
    public GameObject star; // Reference to the star GameObject with the animation
    public Button continueButton; // Reference to the continue button
    public Button replayButton; // Reference to the replay button
    public Button pauseButton; // Reference to the pause button
    public Button resumeButton; // Reference to the resume button
    public Button skipButton; // Reference to the skip button
    public Button ButtonCNVS; // Reference to the canvas toggle button

    public GameObject SolarSystem;
    public GameObject Mercury;
    public GameObject Venus;
    public GameObject Earth;
    public GameObject Moon;
    public GameObject Mars;
    public GameObject Jupiter;
    public GameObject Saturn;
    public GameObject Uranus;
    public GameObject Neptune;

    public GameObject canvas0;
    public GameObject canvas1;
    public GameObject canvas2;
    public GameObject canvas3;
    public GameObject canvas4;
    public GameObject canvas5;
    public GameObject canvas6;
    public GameObject canvas7;
    public GameObject canvas8;
    public GameObject canvas9;
    public GameObject canvas10;

    private int currentIndex = 0; // Index to keep track of the current audio clip being played
    private bool isPaused = false; // Flag to track if the audio is paused

    public Animator starAnimator; // Reference to the star's Animator component

    private bool moving1 = false;
    private bool moving2 = false;
    private bool middle = false;
    private bool left = false;
    private bool right = false;
    private bool disappear = false;

    private void Start()
    {
        // Get the AudioSource component attached to the GameObject
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            // Add an AudioSource component if it doesn't exist
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Get the Animator component from the star GameObject
        starAnimator = star.GetComponent<Animator>();

        // Disable the buttons initially
        continueButton.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);

        // Add listeners to the buttons
        continueButton.onClick.AddListener(OnContinueButtonClicked);
        replayButton.onClick.AddListener(OnReplayButtonClicked);
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
        resumeButton.onClick.AddListener(OnResumeButtonClicked);
        skipButton.onClick.AddListener(OnSkipButtonClicked);
        ButtonCNVS.onClick.AddListener(OnCanvasButtonClicked);

        // Initialize all canvases to inactive
        InitializeCanvases();

        // Start playing the first audio clip
        StartCoroutine(PlayAudioClip(currentIndex));
    }

    // private void InitializeCanvases()
    // {
    //     // Find all objects with the tag "Canvas" and set them inactive
    //     GameObject[] canvases = GameObject.FindGameObjectsWithTag("Canvas");
    //     foreach (GameObject canvas in canvases)
    //     {
    //         canvas.SetActive(false);
    //     }
    // }
    private void InitializeCanvases()
    {
        // Set all canvases to inactive
        canvas0.SetActive(false);
        canvas1.SetActive(false);
        canvas2.SetActive(false);
        canvas3.SetActive(false);
        canvas4.SetActive(false);
        canvas5.SetActive(false);
        canvas6.SetActive(false);
        canvas7.SetActive(false);
        canvas8.SetActive(false);
        canvas9.SetActive(false);
        canvas10.SetActive(false);
    }


    private void OnCanvasButtonClicked()
    {
        // Determine the canvas to toggle based on the current index
        GameObject currentCanvas = null;
        switch (currentIndex)
        {
            case 0:
                currentCanvas = canvas0;
                break;
            case 1:
                currentCanvas = canvas1;
                break;
            case 2:
                currentCanvas = canvas2;
                break;
            case 3:
                currentCanvas = canvas3;
                break;
            case 4:
                currentCanvas = canvas4;
                break;
            case 5:
                currentCanvas = canvas5;
                break;
            case 6:
                currentCanvas = canvas6;
                break;
            case 7:
                currentCanvas = canvas7;
                break;
            case 8:
                currentCanvas = canvas8;
                break;
            case 9:
                currentCanvas = canvas9;
                break;
            case 10:
                currentCanvas = canvas10;
                break;
        }

        // Toggle the active state of the current canvas
        if (currentCanvas != null)
        {
            bool isActive = currentCanvas.activeSelf;
            currentCanvas.SetActive(!isActive);
        }
    }

    private void OnContinueButtonClicked()
    {
        // Hide the buttons
        continueButton.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(false);

        // Increment the current index for the next audio clip
        currentIndex++;

        // If there are more audio clips to play, start the coroutine to play the next one
        if (currentIndex < audioClips.Length)
        {
            StartCoroutine(PlayAudioClip(currentIndex));
        }
    }

    private void OnReplayButtonClicked()
    {
        // Hide the buttons
        continueButton.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(false);

        // Replay the current audio clip
        StartCoroutine(PlayAudioClip(currentIndex));
    }

    private void OnPauseButtonClicked()
    {
        // Pause the audio
        audioSource.Pause();
        isPaused = true;
        SetStarMoving(false); // Set Moving to false in the Animator

        // Hide the pause button and show the resume button
        pauseButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(true);
    }

    private void OnResumeButtonClicked()
    {
        // Resume the audio
        audioSource.UnPause();
        isPaused = false;
        SetStarMoving(true); // Set Moving to true in the Animator

        // Hide the resume button and show the pause button
        resumeButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }

    private void OnSkipButtonClicked()
    {
        // Stop the current audio clip
        audioSource.Stop();
        SetStarMoving(false); // Set Moving to false in the Animator

        // Skip to the end and show the continue and replay buttons
        continueButton.gameObject.SetActive(true);
        replayButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
    }

    private void SetStarMoving(bool isMoving)
    {
        // Set the appropriate Animator parameter based on the isMoving value
        if (isMoving)
        {
            // Example: If your Animator parameter is named "Moving"
            starAnimator.SetBool("Moving", true);
        }
        else
        {
            starAnimator.SetBool("Moving", false);
        }
    }

    private void SetActivePlanets(int index)
    {
        // Deactivate all planets first
        SolarSystem.SetActive(false);
        Mercury.SetActive(false);
        Venus.SetActive(false);
        Earth.SetActive(false);
        Moon.SetActive(false);
        Mars.SetActive(false);
        Jupiter.SetActive(false);
        Saturn.SetActive(false);
        Uranus.SetActive(false);
        Neptune.SetActive(false);

        // Activate specific game objects based on the index
        switch (index)
        {
            case 0:
            case 1:
            case 9:
            case 10:
                SolarSystem.SetActive(true);
                break;
            case 2:
                Mercury.SetActive(true);
                Venus.SetActive(true);
                break;
            case 3:
                Earth.SetActive(true);
                Moon.SetActive(true);
                break;
            case 4:
                Mars.SetActive(true);
                break;
            case 5:
                Jupiter.SetActive(true);
                break;
            case 6:
                Saturn.SetActive(true);
                break;
            case 7:
                Uranus.SetActive(true);
                break;
            case 8:
                Neptune.SetActive(true);
                break;
        }
    }

    private IEnumerator PlayAudioClip(int index)
    {
        // Check if the index is within bounds
        if (index < audioClips.Length)
        {
            // Assign the current audio clip to the AudioSource
            audioSource.clip = audioClips[index];

            // Play the audio clip
            audioSource.Play();

            // Set the active game objects based on the index
            SetActivePlanets(index);

            // Randomly choose one of Moving, Moving1, Moving2 animations
            int moveChoice = Random.Range(0, 3);
            if (moveChoice == 0)
            {
                moving1 = true;
                starAnimator.SetBool("Moving1", true);
            }
            else if (moveChoice == 1)
            {
                moving2 = true;
                starAnimator.SetBool("Moving2", true);
            }
            else
            {
                moving1 = false;
                moving2 = false;
                starAnimator.SetBool("Moving1", false);
                starAnimator.SetBool("Moving2", false);
                starAnimator.SetBool("Moving", true); // Set Moving to true in the Animator
            }

            // Show the pause and skip buttons
            pauseButton.gameObject.SetActive(true);
            skipButton.gameObject.SetActive(true);

            // Wait until the audio clip finishes playing
            while (audioSource.isPlaying)
            {
                yield return null;
            }

            // Stop any moving animations
            moving1 = false;
            moving2 = false;
            starAnimator.SetBool("Moving1", false);
            starAnimator.SetBool("Moving2", false);
            starAnimator.SetBool("Moving", false);

            // Randomly choose one of Middle, Left, Right, Disappear animations
            int endChoice = Random.Range(0, 4);
            if (endChoice == 0)
            {
                middle = true;
                starAnimator.SetBool("Middle", true);
            }
            else if (endChoice == 1)
            {
                left = true;
                starAnimator.SetBool("Left", true);
            }
            else if (endChoice == 2)
            {
                right = true;
                starAnimator.SetBool("Right", true);
            }
            else
            {
                disappear = true;
                starAnimator.SetBool("Disappear", true);
            }

            // Hide the buttons after the audio clip finishes
            continueButton.gameObject.SetActive(true);
            replayButton.gameObject.SetActive(true);
            pauseButton.gameObject.SetActive(false);
            resumeButton.gameObject.SetActive(false);
            skipButton.gameObject.SetActive(false);
        }
    }
}
