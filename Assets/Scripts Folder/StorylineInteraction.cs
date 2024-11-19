using UnityEngine;
using UnityEngine.UI;
using Lean.Touch;
using System.Collections;

public class StorylineInteraction : MonoBehaviour
{
    private PlanetList planetList;
    public Camera mainCamera;
    public Button backButton;
    private bool allowTouchInput = true;
    private bool triggered = false;
    private GameObject selectedPlanet = null;
    private Vector3 initialCameraPosition;
    private Quaternion initialCameraRotation;

    void Start()
    {
        backButton.onClick.AddListener(BackToInitialState);
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

    public void SelectPlanet(GameObject planet)
    {
        selectedPlanet = planet;
        initialCameraPosition = mainCamera.transform.parent.position;
        initialCameraRotation = mainCamera.transform.parent.rotation;
        planet.GetComponent<TrailRenderer>().emitting = false;
        
        Vector3 targetPosition = planet.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - mainCamera.transform.parent.position);
        
        StartCoroutine(MoveCamera(targetPosition, targetRotation));

        backButton.gameObject.SetActive(true);
        allowTouchInput = false;
    }

    IEnumerator MoveCamera(Vector3 targetPosition, Quaternion targetRotation)
    {   
        float duration = 1.0f;
        float elapsedTime = 0f;
        Vector3 initialPosition = mainCamera.transform.parent.position;
        Quaternion initialRotation = mainCamera.transform.parent.rotation;
        
        while (elapsedTime < duration)
        {
            mainCamera.transform.parent.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            mainCamera.transform.parent.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    void Update()
    {
        if (!triggered) return;
        SelectPlanet(selectedPlanet);
    }

    public void BackToInitialState()
    {
        StopAllCoroutines();
        StartCoroutine(MoveCamera(Vector3.zero, Quaternion.identity));
        selectedPlanet.GetComponent<TrailRenderer>().emitting = true;
        triggered = false;
        selectedPlanet = null;
        allowTouchInput = true;
        backButton.gameObject.SetActive(false);
    }
}
