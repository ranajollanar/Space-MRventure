using UnityEngine;
using Lean.Touch;

public class AstronautTouch : MonoBehaviour
{
    public bool Touched = false; // Boolean to track if the astronaut object is touched

    // This method will be called when the object is touched
    public void OnFingerDown(LeanFinger finger)
    {
        // Check if the touch event is over this object
        if (finger.IsOverGui == false)
        {
            Ray ray = finger.GetRay();
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    Touched = true;
                }
            }
        }
    }

    // Subscribe to LeanTouch events when the object is enabled
    private void OnEnable()
    {
        LeanTouch.OnFingerDown += OnFingerDown;
    }

    // Unsubscribe from LeanTouch events when the object is disabled
    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= OnFingerDown;
    }
}
