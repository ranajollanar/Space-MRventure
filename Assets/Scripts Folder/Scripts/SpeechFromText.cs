using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using System;
using TMPro;
using UnityEngine.UI;

public class SpeechFromText : MonoBehaviour
{
    public TMP_Text text;
    private void Start()
    {
        StartTextRecognition(text.text);
    }

    public void StartTextRecognition(string tmptext)
    {
        
        TextToSpeech.Instance.StartSpeak(tmptext);
    }
}
