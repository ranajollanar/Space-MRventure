using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using System;
using TMPro;
using UnityEngine.UI;

public class SpeechController : MonoBehaviour
{
    public TMP_InputField inputField;
    private string _Result ="";
    private void Start()
    {
        SpeechToText.Instance.onResultCallback = OnSpeechResult;
    }
    private void OnSpeechResult(string result)
    {
        inputField.text = result;
        Debug.Log(result + "OnSpeechResult");
        _Result=result;
    }
    public void StartSpeechRecognition()
    {
        SpeechToText.Instance.StartRecording();

        Debug.Log(_Result + "StartSpeechRecognition");


    }
    public void StartTextRecognition(string text)
    {
        TextToSpeech.Instance.StartSpeak(text);
    }
}
