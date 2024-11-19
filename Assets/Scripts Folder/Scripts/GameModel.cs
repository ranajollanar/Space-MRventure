using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New QuestionData", menuName = "QuestionData")]
public class GameModel : ScriptableObject
{
    [System.Serializable]
    public struct Question
    {
        public string question;
        public string[] replies;
        
        public int CorrectAnswerIndex;
    }

   /* [System.Serializable]
    public struct CategoryData
    {
        public string CategoryName;
        public Question[] Questions;
    }*/

    public Question[] questions;
}
