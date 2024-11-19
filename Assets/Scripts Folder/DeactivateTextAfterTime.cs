using System.Collections;
using UnityEngine;

public class DeactivateTextAfterTime : MonoBehaviour
{
    public GameObject textObject; // Reference to the 3D text object

    void Start()
    {
        // Start the coroutine to deactivate the text object after 2 seconds
        StartCoroutine(DeactivateAfterTime(2f));
    }

    private IEnumerator DeactivateAfterTime(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Set the text object to inactive
        textObject.SetActive(false);
    }
}
