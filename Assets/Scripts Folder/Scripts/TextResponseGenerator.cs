using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextResponseGenerator : MonoBehaviour
{
    // Reference to the input field in the Unity UI
    public TMP_InputField inputField;

    // Reference to the text component where the response will be displayed
    public TMP_Text responseText;
    [SerializeField] private SpeechController SpeechController;
    // Responses based on different inputs
    private Dictionary<string, string> responses = new Dictionary<string, string>()
    {
        { "hello", "Hi there!" },
        { "how are you?", "I'm just a computer program, but I'm doing well. How about you?" },
        { "bye", "Goodbye! Take care." },
        // Add more responses as needed
    };

    // Called when the user submits the input
    public void SubmitInput()
    {
        string input = inputField.text.ToLower(); // Convert input to lowercase for case-insensitive comparison
        string response = GenerateResponse(input);
        SpeechController.StartTextRecognition(response.ToString());
        DisplayResponse(response);

    }

    // Generates a response based on the input
    private string GenerateResponse(string input)
    {
        if (responses.ContainsKey(input))
        {
            return responses[input];
        }
        else
        {
            return "I'm sorry, I didn't understand that.";
        }
    }

    // Displays the response in the UI
    private void DisplayResponse(string response)
    {
        responseText.text = response;
    }
}
