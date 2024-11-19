using UnityEngine;

public class PlanetOrbit : MonoBehaviour
{
    public float orbitSpeed = 1f; // Personalized orbit speed for each planet
    private float orbitRadius; // Distance from the sun
    public GameObject sun; // Public reference to the sun GameObject

    // Start is called before the first frame update
    void Start()
    {
        if (sun != null)
        {
            // Calculate the initial orbit radius based on the current position relative to the sun
            orbitRadius = Vector3.Distance(transform.position, sun.transform.position);
        }
        else
        {
            Debug.LogError("Sun GameObject is not assigned!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sun != null)
        {
            OrbitAroundCenter();
        }
    }

    void OrbitAroundCenter()
    {
        // Rotate the planet around the sun
        transform.RotateAround(sun.transform.position, Vector3.up, orbitSpeed * Time.deltaTime);
    }
}
