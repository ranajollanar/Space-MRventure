using UnityEngine;

public class MoonOrbit : MonoBehaviour
{
    public float orbitSpeed = 1f; // Personalized orbit speed for the moon
    private float orbitRadius; // Distance from the earth

    // Reference to the earth GameObject
    public GameObject earthObject;

    // Start is called before the first frame update
    void Start()
    {
        // Calculate the initial orbit radius based on the current position relative to the earth
        if (earthObject != null)
        {
            Transform centerObject = earthObject.transform;
            orbitRadius = Vector3.Distance(transform.position, centerObject.position);
        }
        else
        {
            Debug.LogError("Earth GameObject is not assigned!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        OrbitAroundCenter();
    }

    void OrbitAroundCenter()
    {
        // Check if the earth GameObject is assigned
        if (earthObject != null)
        {
            // Get the position of the earth
            Transform centerObject = earthObject.transform;

            // Rotate the moon around the earth
            transform.RotateAround(centerObject.position, Vector3.up, orbitSpeed * Time.deltaTime);

            // Update the moon's position relative to the earth's position
            Vector3 direction = transform.position - centerObject.position;
            Vector3 orbitPosition = centerObject.position + direction.normalized * orbitRadius;
            transform.position = orbitPosition;
        }
        else
        {
            Debug.LogError("Earth GameObject is not assigned!");
        }
    }
}
