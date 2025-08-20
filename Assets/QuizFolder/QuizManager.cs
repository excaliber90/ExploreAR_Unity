using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject startPanel;   // Planet Quiz start screen
    public GameObject quizPanel;    // Quiz question panel
    public GameObject resultPanel;  // Feedback / result panel

    [Header("UI Elements")]
    public Text questionText;
    public Button[] optionButtons;
    public Text feedbackText;

    [Header("Quiz Data")]
    public QuizData[] quizQuestions;     // Assign QuizData assets in Inspector
    public Transform planet3DHolder;     // Empty object above question to spawn planet

    [Header("Planet Prefabs")]
    public GameObject[] planetPrefabs;   // Assign 3D planet prefabs (name must match planetName)

    private QuizData currentQuestion;
    private GameObject currentPlanetModel;

    void Start()
    {
        // Show Start Panel at beginning, hide others
        startPanel.SetActive(true);
        quizPanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    // Called by Start Button
    public void StartQuiz()
    {
        startPanel.SetActive(false);
        quizPanel.SetActive(true);

        LoadRandomQuestion();
    }

    void LoadRandomQuestion()
    {
        // Pick a random question
        int randomIndex = Random.Range(0, quizQuestions.Length);
        currentQuestion = quizQuestions[randomIndex];

        // Set question text
        questionText.text = currentQuestion.question;

        // Set answer buttons
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i; // Capture local index for listener
            if (i < currentQuestion.options.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<Text>().text = currentQuestion.options[i];
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => CheckAnswer(index));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false); // hide unused buttons
            }
        }

        // Spawn 3D planet model
        if (currentPlanetModel != null)
            Destroy(currentPlanetModel);

        foreach (var prefab in planetPrefabs)
        {
            if (prefab.name == currentQuestion.planetName)
            {
                currentPlanetModel = Instantiate(prefab, planet3DHolder.position, Quaternion.identity, planet3DHolder);
                break;
            }
        }
    }

    void CheckAnswer(int chosenIndex)
    {
        quizPanel.SetActive(false);
        resultPanel.SetActive(true);

        if (chosenIndex == currentQuestion.correctAnswerIndex)
            feedbackText.text = "Amazing! 🎉 Correct Answer!";
        else
            feedbackText.text = "You can do better! ❌";
    }

    void Update()
    {
        // Slowly rotate the planet for visual effect
        if (currentPlanetModel != null)
        {
            currentPlanetModel.transform.Rotate(Vector3.up * 20f * Time.deltaTime);
        }
    }

    // Called by Quit Button
    public void QuitQuiz()
    {
        // Return to Main Menu (replace "MainMenu" with your scene name)
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
