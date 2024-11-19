using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Public fields to assign GameObjects in the Inspector
    public GameObject home;
    public GameObject category;
    public GameObject star;
    public GameObject profile;
    
    public GameObject homeActive;
    public GameObject categoryActive;
    public GameObject starActive;
    public GameObject profileActive;

    void Start()
    {
        // Initialize all active states to inactive
        homeActive.SetActive(false);
        categoryActive.SetActive(false);
        starActive.SetActive(false);
        profileActive.SetActive(false);

        // Determine the active menu based on the current scene
        string currentScene = SceneManager.GetActiveScene().name;
        
        switch (currentScene)
        {
            case "HomeScene":
                homeActive.SetActive(true);
                break;
            case "All":
                categoryActive.SetActive(true);
                break;
            case "Storyline":
                starActive.SetActive(true);
                break;
            case "Filter":
                profileActive.SetActive(true);
                break;
            default:
                // If the scene doesn't match any known case, keep all active states inactive
                break;
        }
    }
}
