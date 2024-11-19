using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using static FLASK;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Debug = UnityEngine.Debug;

public class PostMethod : MonoBehaviour
{
    [SerializeField] TMP_Text outputArea;
    [SerializeField] TMP_InputField input;      
    [SerializeField] private SpeechController SpeechController;
    public OnResponseEvent onResponse;
    string myurl = "http://192.168.1.194:5000/chat";
    public Animator astroAnimator;
    public Button startButton;
    private bool isStreaming=true;

    void Start()
    {
        SpeechController.StartTextRecognition(outputArea.text);
        //outputArea = GameObject.Find("OutputArea").GetComponent<InputField>();
        startButton.onClick.AddListener(PostData);
    }
 
    void PostData() {
        StartCoroutine(PostRequest3(myurl));
        // StartCoroutine(GetStream());
    } 

    IEnumerator PostRequest3(string url)
    {
        outputArea.text = "Hmmm..";
        astroAnimator.SetBool("isThinking", true);
        astroAnimator.SetBool("isTalking", false);
        astroAnimator.SetBool("isWalking", false);
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        var jsonData = new Dictionary<string, string>
        {
            { "question", input.text }
        };
        string jsonData2 = "{\"question\": \"" + input.text + "\"}";
        string dataJson = JsonConvert.SerializeObject(jsonData);
        print (jsonData.ToString());
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData2);
    
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
    
        request.SendWebRequest();

        while(request.downloadHandler == null){
            Debug.Log("null");
            yield return null;

        }
        int count = 0;
        bool messageSent = false;
        while(!request.isDone){
            if(request.downloadHandler.text != ""){
               // Debug.Log(request.downloadHandler.text);
                 string responseText = request.downloadHandler.text;
                //  print(responseText);
                MatchCollection matches = Regex.Matches(responseText, @"\{(?:[^{}]|(?<o>\{)|(?<-o>\}))+(?(o)(?!))\}");
                if (!messageSent){
                    messageSent = true;
                    outputArea.text = "";
                }
                if(matches.Count > count){
                    // print("equal");
                    //  print(matches.Count);
                     count++;
                    var json = JsonConvert.DeserializeObject<Result3>(matches[matches.Count-1].Value);

                    // Extract the word from the JSON object
                    // if (json.TryGetValue("content", out string word))
                    // {
                    //     // Update the UI with the word
                    //     outputArea.text += word + " ";
                    //     yield return null; // Ensure smooth UI updates
                    // }
                    outputArea.text+=json.content;
                    yield return null;
                }
               
                
                foreach (Match match in matches)
                {
                    // Deserialize the JSON object
                    print(match.Value);
                }
            }
            yield return null;
        }


        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            UnityEngine.Debug.LogError("Error: " + request.error);
        }
        else
        {
            // outputArea.text= "";
            string responseText = request.downloadHandler.text;
            // Result2 result = JsonConvert.DeserializeObject<Result2>(responseText);
            // Debug.Log(result.done);
            MatchCollection matches = Regex.Matches(responseText, @"\{(?:[^{}]|(?<o>\{)|(?<-o>\}))+(?(o)(?!))\}");
            SpeechController.StartTextRecognition(outputArea.text);
            foreach (Match match in matches)
            {
                // Deserialize the JSON object
                // var json = JsonConvert.DeserializeObject<Dictionary<string, string>>(match.Value);
                

                // // Extract the word from the JSON object
                // if (json.TryGetValue("content", out string word))
                // {
                //     // Update the UI with the word
                //     outputArea.text += word + " ";
                //     yield return null; // Ensure smooth UI updates
                // }

                // var json2 = JsonConvert.DeserializeObject<Result2>(match.Value);
                // print(json2.done);
                // if(json2.done){
                //     outputArea.text = json2.full_content;
                // }
                // if(json.TryGetValue("done",out string done)){
                //     print(done),

                // }
            }

            // Finalize UI update
            astroAnimator.SetBool("isThinking", false);
            astroAnimator.SetBool("isTalking", true);
            astroAnimator.SetBool("isWalking", false);

            SpeechController.StartTextRecognition(outputArea.text);
    
            astroAnimator.SetBool("isThinking", false);
            astroAnimator.SetBool("isTalking", true);
            astroAnimator.SetBool("isWalking", false);
        }
    
        request.Dispose();
        SpeechController.StartTextRecognition(outputArea.text);
        stopwatch.Stop();
        UnityEngine.Debug.Log("Time taken: " + stopwatch.ElapsedMilliseconds + " ms");
    }

  
    //it2 ===
    //     IEnumerator PostRequest3(string url)
    // {
    //     outputArea.text = "Let me think..";
    //     astroAnimator.SetBool("isThinking", true);
    //     astroAnimator.SetBool("isTalking", false);
    //     astroAnimator.SetBool("isWalking", false);
    //     System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
    //     stopwatch.Start();
    
    //     var jsonData = new Dictionary<string, string>
    //     {
    //         { "question", input.text }
    //     };
    
    //     string dataJson = JsonConvert.SerializeObject(jsonData);
    //     var request = new UnityWebRequest(url, "POST");
    //     byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(dataJson);
    
    //     request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    //     request.downloadHandler = new DownloadHandlerBuffer();
    //     request.SetRequestHeader("Content-Type", "application/json");
    
    //     yield return request.SendWebRequest();
    
    //     if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
    //     {
    //         UnityEngine.Debug.LogError("Error: " + request.error);
    //     }
    //     else
    //     {
    //         string jsonString = "";
    //         using (var stream = new MemoryStream(request.downloadHandler.data))
    //         {
    //             using (var reader = new StreamReader(stream))
    //             {
    //                 while (!reader.EndOfStream)
    //                 {
    //                     jsonString = reader.ReadLine();
    
    //                     var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
    //                     print("jsonResponse  "+jsonResponse);
    //                     if (jsonResponse.TryGetValue("content", out string word))
    //                     {
    //                         print("word");
    //                         outputArea.text += word + " ";
    //                         yield return null;
    //                     }
    //                 }
    //             }
    //         }
    
    //         astroAnimator.SetBool("isThinking", false);
    //         astroAnimator.SetBool("isTalking", true);
    //         astroAnimator.SetBool("isWalking", false);
    //     }
    
    //     request.Dispose();
    //     SpeechController.StartTextRecognition(outputArea.text);
    //     stopwatch.Stop();
    //     UnityEngine.Debug.Log("Time taken: " + stopwatch.ElapsedMilliseconds + " ms");
    // }


    // IEnumerator PostRequest3(string url)
    // {
    //     outputArea.text = "Let me think..";
    //     // Thinking animation
    //     astroAnimator.SetBool("isThinking", true);
    //     astroAnimator.SetBool("isTalking", false);
    //     astroAnimator.SetBool("isWalking", false);
    //     System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
    //     stopwatch.Start();

    //     // Create JSON data
    //     //string jsonData = "{\"question\": \"" + input.text + "\"}";

    //     var jsonData = new Dictionary<string, string>
    //     {
    //         { "question", input.text }
    //     };
 
    //     string dataJson = JsonConvert.SerializeObject(jsonData);
    //     var request = new UnityWebRequest(url, "POST");
    //     byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(dataJson);
 
    //     request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
    //     request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    //     request.SetRequestHeader("Content-Type", "application/json");

    //     // Start the request
    //     yield return request.SendWebRequest();

    //     if (request.result == UnityWebRequest.Result.ConnectionError ||
    //         request.result == UnityWebRequest.Result.ProtocolError)
    //     {
    //         UnityEngine.Debug.LogError("Error: " + request.error);
    //     }
    //     else
    //     {
    //         // Read the response stream incrementally
    //         using (var stream = new MemoryStream(request.downloadHandler.data))
    //         {
    //             using (var reader = new StreamReader(stream))
    //             {
    //                 while (!reader.EndOfStream)
    //                 {
    //                     string responseLine = reader.ReadLine();
    //                     // Process response line as needed
    //                     UnityEngine.Debug.Log("Received: " + responseLine);
    //                     // Update UI with partial response
    //                     outputArea.text += responseLine;
    //                     // yield return null; // Ensure smooth UI updates
    //                 }
    //             }
    //         }

    //         // Finalize UI update
    //         //outputArea.text = "Stream complete";

    //         // Animation and further processing
    //         astroAnimator.SetBool("isThinking", false);
    //         astroAnimator.SetBool("isTalking", true);
    //         astroAnimator.SetBool("isWalking", false);
    //         }
        
    //     request.Dispose();
    //     SpeechController.StartTextRecognition(outputArea.text);
    //     stopwatch.Stop();
    //     UnityEngine.Debug.Log("Time taken: " + stopwatch.ElapsedMilliseconds + " ms");
    // }


//  IEnumerator PostRequest3(string url)
//     {
//         outputArea.text = "Let me think..";
//         // thinking animation
//         astroAnimator.SetBool("isThinking", true);
//         astroAnimator.SetBool("isTalking", false);
//         astroAnimator.SetBool("isWalking", false);
//         Stopwatch stopwatch = new Stopwatch();
//         stopwatch.Start();
//         var jsonData = new Dictionary<string, string>
//         {
//             { "question", input.text }
//         };
 
//         string dataJson = JsonConvert.SerializeObject(jsonData);
//         //print(dataJson);
//         var request = new UnityWebRequest(url, "POST");
//         byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(dataJson);
 
//         request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
//         request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
//         request.SetRequestHeader("Content-Type", "application/json");
//         //request.SetRequestHeader("Authorization", "Bearer " + apiKey);
 
//         yield return request.SendWebRequest();
 
//         if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
//         {
//             print("Error: " + request.error);
//         }
//         else
//         {
        
//             print("Received: " + request.downloadHandler.text);
//           //  Result result = JsonUtility.FromJson<APIResponse>(request.downloadHandler.text).result;
 
//             string result = JsonUtility.FromJson<ApiResult>(request.downloadHandler.text).response;
//             outputArea.text = result;
//             // astroAnimator.Play("");
//             astroAnimator.SetBool("isThinking", false);
//             astroAnimator.SetBool("isTalking", true);
//             astroAnimator.SetBool("isWalking", false);
            
 
//         }
//         SpeechController.StartTextRecognition(outputArea.text);
//        // btn.interactable = true;
//         stopwatch.Stop();
//         UnityEngine.Debug.Log("Time taken: " + stopwatch.ElapsedMilliseconds + " ms");
//         //text.text = stopwatch.ElapsedMilliseconds / 1000 + " s";
//         request.Dispose();
//     }


    // IEnumerator PostData_Coroutine()
    // {
    //     outputArea.text = "Let me think..";
    //     string uri = "http://10.72.0.153:5000/llm/question";
    //     WWWForm form = new WWWForm();
    //     form.AddField("question", "hello");
    //     // thinking animation 




    //     UnityEngine.Debug.Log(form);
    //     using(UnityWebRequest request = UnityWebRequest.Post(uri, form))
    //     {
    //         request.SetRequestHeader("Content-Type","application/json");

    //         yield return request.SendWebRequest();
    //         if (request.result == UnityWebRequest.Result.ConnectionError)
    //         {
    //             UnityEngine.Debug.LogError("Network Error: " + request.error);
    //         }
    //         else if (request.result == UnityWebRequest.Result.ProtocolError)
    //         {
    //             UnityEngine.Debug.LogError("HTTP Error: " + request.error);
    //         }
    //         else
    //         {
    //             // Request successful
    //             UnityEngine.Debug.Log(request.downloadHandler.text);
    //             string result = JsonUtility.FromJson<ApiResult>(request.downloadHandler.text).response;
    //             outputArea.text = result;
    //             // Invoke the onResponse event with the response text
    //             onResponse.Invoke(request.downloadHandler.text);
    //         }
    //     }
    //     SpeechController.StartTextRecognition(outputArea.text);
    // }
}


public class ApiResult
{
    //public Result result;
    public string response;
}

public class Result
{
    public string response;
}

public class Result2
{
    public string model;
    public string full_content;
    public bool done;
}

public class Result3
{
    public string model;
    public string content;
    public bool done;

}