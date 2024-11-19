using UnityEngine;
using UnityEngine.UI;
using Lean.Touch;
using System.Collections;
using Unity.VisualScripting;
using System;
using TMPro;

public class PlanetInteraction : MonoBehaviour
{
    private PlanetList planetList;
    public TMP_Text planetNameText;
    public TMP_Text distanceText;
    public TMP_Text descriptionText;
    public Camera mainCamera;
    public GameObject planetInfoCanvas;
    // public GameObject mask;
    public CanvasGroup maskCanvasGroup;
    public Button backButton;
    public GameObject card; 
    public GameObject swipeArrow;

    private Vector3 initialCameraPosition;
    private Quaternion initialCameraRotation;
    private bool allowTouchInput = true;
    bool triggered = false;
    GameObject selectedPlanet = null;
    void Start()
    {
        backButton.onClick.AddListener(BackToInitialState);
        planetInfoCanvas.SetActive(false);
    }

    void OnDestroy()
    {
        backButton.onClick.RemoveListener(BackToInitialState);
    }

    void OnEnable()
    {
        LeanTouch.OnFingerTap += OnFingerTap;
    }

    void OnDisable()
    {
        LeanTouch.OnFingerTap -= OnFingerTap;
    }

    void OnFingerTap(LeanFinger finger)
    {
        if (!allowTouchInput) return;

        Ray ray = mainCamera.ScreenPointToRay(finger.ScreenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            Debug.Log("Hit Object Tag: " + hitObject.tag); // Log the tag of the hit object
            if (hitObject.CompareTag("Celestial") || hitObject.CompareTag("Earth") || hitObject.CompareTag("Moon")) 
            {
                SelectPlanet(hitObject);
            }
        }
    }
    

    private void Awake()
    {
        planetList = PlanetDataLoader.LoadPlanetData();
    }

    public string GetPlanetDescription(string planetName)
    {
        foreach (var planet in planetList.planets)
        {
            if (planet.name == planetName)
            {
                return planet.description;
            }
        }
        return "Description not found";
    }

    public float GetDistanceFromSun(string planetName)
    {
        foreach (var planet in planetList.planets)
        {
            if (planet.name == planetName)
            {
                return planet.distance_from_sun;
            }
        }
        return -1; // Default value if planet not found
    }
    public void SelectPlanet(GameObject planet)
    {
        string planetName = GetPlanetName(planet);
        float distance = GetDistanceFromSun(planet.name);
        string description = GetPlanetDescription(planet.name);

        planetNameText.text = planetName;
        distanceText.text = distance.ToString() + " AU"; // AU: Astronomical Unit
        descriptionText.text = description;

        selectedPlanet = planet;
        initialCameraPosition = mainCamera.transform.parent.position;
        initialCameraRotation = mainCamera.transform.parent.rotation;
        planet.GetComponent<TrailRenderer>().emitting = false;
        // Calculate the position and rotation to look at (center of the selected planet)
        Vector3 targetPosition = planet.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - mainCamera.transform.parent.position);
        mainCamera.transform.LookAt(targetPosition);
        // Start the camera movement coroutine
        StartCoroutine(MoveCamera(targetPosition, targetRotation));

        // Activate planet info canvas
        planetInfoCanvas.SetActive(true);
        // start animating card and swipe arrow translate them on y axis (from not apparent to appearing on the buttom of the screen) 
        StartCoroutine(AnimateCardAndSwipeArrow());
        StartCoroutine(FadeOutMask());
        // Disable touch input while zooming in
        allowTouchInput = false;
    }

    
    IEnumerator AnimateCardAndSwipeArrow()
    {
        Vector3 initialCardPosition = new Vector3(5.58502197f, -1100f, 0f);
        Vector3 targetCardPosition = new Vector3(5.58502197f, -635f, 0f);

        Vector3 initialArrowPosition = new Vector3(0f, -1100f, 0f);
        Vector3 targetArrowPosition = new Vector3(0f, -380f, 0f);

        float duration = 3.0f; // Adjust the duration of the animation
        float elapsedTime = 2f;

        while (elapsedTime < duration)
        {
            // Interpolate card position smoothly
            card.transform.localPosition = Vector3.Lerp(initialCardPosition, targetCardPosition, elapsedTime / duration);

            // Interpolate swipe arrow position smoothly
            swipeArrow.transform.localPosition = Vector3.Lerp(initialArrowPosition, targetArrowPosition, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the card and swipe arrow end at the exact target positions
        card.transform.localPosition = targetCardPosition;
        swipeArrow.transform.localPosition = targetArrowPosition;

        // Call the fade-out animation
    
    }


    IEnumerator FadeOutMask()
    {
        float duration = 1.0f; // Adjust the duration of the fade-out animation
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Fade out the mask canvas group
            maskCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Increment the elapsed time
        elapsedTime += Time.deltaTime;
 
        // Calculate the interpolation factor based on the elapsed time and duration
        float t = Mathf.Clamp01(elapsedTime / duration);
 
        // Interpolate the position of the object between initial and final positions
         maskCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
 
        // If the interpolation is complete, reset the elapsed time
        if (t >= 1.0f)
        {
            elapsedTime = 0f;
        }


        // Ensure the mask ends with an alpha value of 0
        maskCanvasGroup.alpha = 0f;

        // Deactivate interaction and blocking of raycasts
        maskCanvasGroup.blocksRaycasts = false;
        maskCanvasGroup.interactable = false;
    }



    private string GetPlanetName(GameObject planet)
    {
        return planet.name;
    }

    IEnumerator MoveCamera(Vector3 targetPosition, Quaternion targetRotation,bool t= true)
    {   
        triggered = t;
        float duration = 1.0f; // Adjust the duration of the camera movement
        float elapsedTime = 0f;
        
        Vector3 initialPosition = mainCamera.transform.parent.position;
        Quaternion initialRotation = mainCamera.transform.parent.rotation;
        if(!t){
            initialPosition = mainCamera.transform.parent.localPosition;
        }
        while (elapsedTime < duration)
        {
            // Interpolate camera position and rotation smoothly
            if(t){
            mainCamera.transform.parent.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            mainCamera.transform.parent.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / duration);
            }else{

            mainCamera.transform.parent.localPosition = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            mainCamera.transform.parent.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / duration);


            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the camera ends at the exact target position and rotation
       // mainCamera.transform.parent.position = targetPosition;
       // mainCamera.transform.parent.rotation = targetRotation;
        
    }
    void Update(){
        if(!triggered) return;
        SelectPlanet(selectedPlanet);
    }
    public void BackToInitialState()
    {
        // Start the camera movement coroutine to return to the initial state
        StopAllCoroutines();
        StartCoroutine(MoveCamera(Vector3.zero,  Quaternion.identity,false));
        selectedPlanet.GetComponent<TrailRenderer>().emitting = true;
        triggered =false;
        selectedPlanet = null;
        allowTouchInput =true;
        
        // mainCamera.transform.parent.localPosition = Vector3.zero;
        // mainCamera.transform.parent.rotation = Quaternion.identity;
        // mainCamera.transform.position = new Vector3(-827f, 116f, -17098f);
        // Deactivate planet info canvas
        planetInfoCanvas.SetActive(false);
    }

}

