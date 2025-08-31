using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Aimal_Quiz : MonoBehaviour
{
    [Header("Panels")]
    public GameObject startPanel;   // Planet Quiz start screen
    public GameObject quizPanel;    // Quiz question panel
    public GameObject resultPanel;  // Final results panel

    [Header("UI Elements")]
    public TMP_Text questionText;
    public Button[] optionButtons;
    public TMP_Text feedbackText;   // For final results

    [Header("Quiz Data")]
    public AnimalQuizData[] quizQuestions;     
    private AnimalQuizData currentQuestion;
    private int currentQuestionIndex = 0;
    private int score = 0;
    private AnimalQuizData[] questionsPool;    // To keep track of remaining questions
    
    void Start()
    {
        startPanel.SetActive(true);
        quizPanel.SetActive(false);
        resultPanel.SetActive(false);

        // Clone quizQuestions to work as a pool
        questionsPool = new AnimalQuizData[quizQuestions.Length];
        quizQuestions.CopyTo(questionsPool, 0);
    }

    public void StartQuiz()
    {
        startPanel.SetActive(false);
        currentQuestionIndex = 0;
        score = 0;

        // Shuffle questionsPool
        ShuffleQuestions();

        quizPanel.SetActive(true);
        resultPanel.SetActive(false);

        LoadNextQuestion();
    }

    void LoadNextQuestion()
    {
        if (currentQuestionIndex >= questionsPool.Length)
        {
            ShowResultPanel();
            return;
        }

        currentQuestion = questionsPool[currentQuestionIndex];

        // Set question text
        questionText.text = currentQuestion.question;

        // Set answer buttons
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i; // Capture local index for listener
            if (i < currentQuestion.options.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<TMP_Text>().text = currentQuestion.options[i];
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }
    void OnOptionSelected(int chosenIndex)
    {
        if (chosenIndex == currentQuestion.correctAnswerIndex)
            score++;

        currentQuestionIndex++;
        LoadNextQuestion();
    }

    void ShowResultPanel()
    {
        quizPanel.SetActive(false);
        resultPanel.SetActive(true);

        feedbackText.text = $"You answered {score}/{questionsPool.Length} correctly!\n";

        if (score == questionsPool.Length)
            feedbackText.text += "Perfect! 🌟";
        else if (score >= questionsPool.Length * 0.7f)
            feedbackText.text += "Great job! 👍";
        else
            feedbackText.text += "Keep trying! 💪";
    }

    void ShuffleQuestions()
    {
        for (int i = 0; i < questionsPool.Length; i++)
        {
            AnimalQuizData temp = questionsPool[i];
            int randomIndex = Random.Range(i, questionsPool.Length);
            questionsPool[i] = questionsPool[randomIndex];
            questionsPool[randomIndex] = temp;
        }
    }
    // Called by Quit Button
    public void QuitQuiz()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main_Menu");
    }
     public void Quit()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Quiz_Scene");
    }

    // Retry Buttton
    
    public void RetryQuiz()
    {
        StartQuiz();
    }
}
