using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using TMPro;

public class FLASK: MonoBehaviour
{
    // Declare the onResponse event
    public OnResponseEvent onResponse;

    
    [System.Serializable]
    public class OnResponseEvent : UnityEvent<string> { }

    // Start is called before the first frame update
    void Start()
    {
        // Start the coroutine for the request
        StartCoroutine(GetRequest("http://127.0.0.1:5000/get"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Send the request and wait for a response
            yield return webRequest.SendWebRequest();

            // Check if there is any error
            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                UnityEngine.Debug.LogError("Network Error: " + webRequest.error);
            }
            else if (webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                UnityEngine.Debug.LogError("HTTP Error: " + webRequest.error);
            }
            else
            {
                // Request successful
                UnityEngine.Debug.Log(webRequest.downloadHandler.text);
                // Invoke the onResponse event with the response text
                onResponse.Invoke(webRequest.downloadHandler.text);
            }
        }
    }
}


