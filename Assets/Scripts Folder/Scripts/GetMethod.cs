using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using static FLASK;

public class GetMethod : MonoBehaviour
{
    public OnResponseEvent onResponse;
    InputField outputArea;
 
    void Start()
    {
        // outputArea = GameObject.Find("OutputArea").GetComponent<InputField>();
        // GameObject.Find("GetButton").GetComponent<Button>().onClick.AddListener(GetData);
        StartCoroutine(GetRequest("http://127.0.0.1:5000/status"));
    }

    // void GetData() => StartCoroutine(GetData_Coroutine());
 
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