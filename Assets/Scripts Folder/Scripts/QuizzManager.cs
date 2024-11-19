using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizzManager : MonoBehaviour
{
    public TMP_Text questionText;
    public TMP_Text Scoretext;
    public TMP_Text FinalScore;
    public TMP_Text ScoreMessage; // TMP_Text for displaying the message based on the score
    public Button[] replies;
    public GameObject[] buttonBackgrounds; // Add GameObject references for button backgrounds

    public GameModel QstData;
    public AudioSource Correct;
    public AudioSource Incorrect;
    public GameObject GameFinished;
    public GameObject luma; // Reference to the "luma" GameObject

    public RectTransform progressIndicator; // Reference to the progress indicator RectTransform
    public TMP_Text questionGameObject; // TMP_Text for the "Question" GameObject
    public TMP_Text scoreTextObject; // TMP_Text for displaying the final score
    public TMP_Text scoreMessageObject; // TMP_Text for displaying the score message

    private Vector3 startPosition = new Vector3(-150.000031f, 0, 0); // Start position
    private Vector3 endPosition = new Vector3(-0.500030518f, 0, 0); // End position

    private int currentQuestion = 0;
    private static int score = 0;

    void Start()
    {
        SetQuestion(currentQuestion);
        GameFinished.gameObject.SetActive(false);
        luma.SetActive(false); // Initially deactivate the "luma" GameObject
        scoreTextObject.gameObject.SetActive(false); // Initially deactivate the scoreTextObject TMP_Text
        scoreMessageObject.gameObject.SetActive(false); // Initially deactivate the scoreMessageObject TMP_Text
        UpdateProgressIndicator(); // Initialize the progress indicator's position
    }

    void SetQuestion(int questionIndex)
    {
        questionText.text = QstData.questions[questionIndex].question;

        // Change the text of the "Question" TMP_Text to display the question number
        questionGameObject.text = "Question " + (questionIndex + 1);

        foreach (Button r in replies)
        {
            r.onClick.RemoveAllListeners();
        }
        for (int i = 0; i < replies.Length; i++)
        {
            replies[i].GetComponentInChildren<TMP_Text>().text = QstData.questions[questionIndex].replies[i];
            int replyIndex = i;
            replies[i].onClick.AddListener(() =>
            {
                CheckReply(replyIndex);
            });
        }
    }

    void CheckReply(int replyIndex)
    {
        if (replyIndex == QstData.questions[currentQuestion].CorrectAnswerIndex)
        {
            score++;
            Scoretext.text = score.ToString();
            ChangeButtonBackgroundColor(buttonBackgrounds[replyIndex], Color.green);
            Correct.Play();
        }
        else
        {
            ChangeButtonBackgroundColor(buttonBackgrounds[replyIndex], Color.red);
            Incorrect.Play();
        }

        foreach (Button r in replies)
        {
            r.interactable = false;
        }

        StartCoroutine(Next());
    }

    void ChangeButtonBackgroundColor(GameObject buttonBackground, Color color)
    {
        Image bgImage = buttonBackground.GetComponent<Image>();
        if (bgImage != null)
        {
            bgImage.color = color;
        }
    }

    IEnumerator Next()
    {
        yield return new WaitForSeconds(2);
        currentQuestion++;
        if (currentQuestion < QstData.questions.Length)
        {
            Reset();
        }
        else
        {
            GameFinished.SetActive(true);
            float scorePercentage = (float)score / QstData.questions.Length * 100;

            // Set the final score text
            FinalScore.text = scorePercentage.ToString("F0") + "%";

            // Set the score message based on the percentage
            if (scorePercentage < 30)
            {
                ScoreMessage.text = "Better luck next time!";
            }
            else if (scorePercentage < 60)
            {
                ScoreMessage.text = "Good effort!";
            }
            else if (scorePercentage < 90)
            {
                ScoreMessage.text = "Great job!";
            }
            else
            {
                ScoreMessage.text = "Excellent!";
            }

            // Display the score text and message
            scoreTextObject.text = "You scored: " + FinalScore.text;
            luma.SetActive(true); // Activate the "luma" GameObject
            scoreTextObject.gameObject.SetActive(true); // Activate the scoreTextObject TMP_Text
            scoreMessageObject.gameObject.SetActive(true); // Activate the scoreMessageObject TMP_Text
        }

        UpdateProgressIndicator(); // Update the progress indicator position
    }

    public void Reset()
    {
        foreach (Button r in replies)
        {
            r.interactable = true;
        }

        foreach (GameObject bg in buttonBackgrounds)
        {
            ChangeButtonBackgroundColor(bg, Color.white);
        }

        SetQuestion(currentQuestion);
    }

    void UpdateProgressIndicator()
    {
        int totalQuestions = QstData.questions.Length;
        if (totalQuestions > 0)
        {
            float t = (float)currentQuestion / totalQuestions;
            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, t);
            progressIndicator.anchoredPosition = new Vector2(newPosition.x, newPosition.y);
        }
    }
}
