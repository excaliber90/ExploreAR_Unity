using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ARUI : MonoBehaviour
{
    public Canvas canvas;
    public TMP_Text infoBox;
    public RawImage rawImage;
    public Button GetbackButton;

    private AudioSource audio;
    private PlanetInfo currentPlanet;
    private Transform scaledPlanet;
    private Vector3 originalScale;
    public GameObject quizPanel;
    public TMP_Text quizQuestion;
    public Button[] answerButtons;
    public TMP_Text feedbackText;

    [System.Serializable]
    public class QuizData
    {
    public string question;
        public string[] options;
    public int correctAnswerIndex;
    }

private Dictionary<string, QuizData> quizDictionary = new Dictionary<string, QuizData>();

    private int infoPointer = 0;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        canvas.enabled = false;
        /*if (GetbackButton != null) {
            GetbackButton.gameObject.SetActive(false); //Hide Go back button initially
        }*/
         quizDictionary.Add("Earth", new QuizData {
        question = "Which planet supports life?",
        options = new string[] { "Mars", "Venus", "Earth" },
        correctAnswerIndex = 2
    });

    quizDictionary.Add("Mars", new QuizData {
        question = "Which planet is called the Red Planet?",
        options = new string[] { "Earth", "Jupiter", "Mars" },
        correctAnswerIndex = 2
    });
    }

    void ShowQuiz(string planetName)
    {
        if (!quizDictionary.ContainsKey(planetName)) return;

        QuizData quiz = quizDictionary[planetName];
        quizPanel.SetActive(true);
        quizQuestion.text = quiz.question;
        feedbackText.text = "";

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i; // Needed to capture button index
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = quiz.options[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => CheckAnswer(index, quiz.correctAnswerIndex));
        }
    }

void CheckAnswer(int selectedIndex, int correctIndex)
{
    if (selectedIndex == correctIndex)
    {
        feedbackText.text = "Correct!";
        feedbackText.color = Color.green;
        StartCoroutine(HideQuizAfterDelay(2f));
    }
    else
    {
        feedbackText.text = "Try again!";
        feedbackText.color = Color.red;
    }
}

IEnumerator HideQuizAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay);
    quizPanel.SetActive(false);
}

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click or tap
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 50))
            {
                PlanetInfo planetInfo = hit.transform.GetComponent<PlanetInfo>();
                if (planetInfo != null)
                {
                    SelectPlanet(hit.transform, planetInfo);
                }
            }
        }
    }

    void SelectPlanet(Transform planetTransform, PlanetInfo planetInfo)
    {
        // Reset previously scaled planet
        if (scaledPlanet != null && scaledPlanet != planetTransform)
        {
            scaledPlanet.localScale = originalScale;
        }

        currentPlanet = planetInfo;
        infoPointer = 0;
        displayCanvas();
        displayAndPlayInfo();

        if (scaledPlanet != planetTransform)
        {
            scaledPlanet = planetTransform;
            originalScale = scaledPlanet.localScale;
            scaledPlanet.localScale = originalScale * 1.2f;
        }
    }

    void displayAndPlayInfo()
    {
        if (currentPlanet == null) return;

        // Hide Go Back button by default
        if (infoPointer == 0 && GetbackButton != null)
        {
            GetbackButton.gameObject.SetActive(false);
        }

        // Show text
        if (infoPointer < currentPlanet.descriptions.Count)
            infoBox.text = currentPlanet.descriptions[infoPointer];
        else
        {
            infoBox.text = "";
            ShowQuiz(currentPlanet.name);
        }

        // Play audio
        if (infoPointer < currentPlanet.audioClips.Count)
        {
            audio.Stop();
            audio.clip = currentPlanet.audioClips[infoPointer];
            audio.Play();
        }
     
            
        // Show image
       /* if (infoPointer < currentPlanet.images.Count)
            rawImage.texture = currentPlanet.images[infoPointer];
        else
            rawImage.texture = null;*/
    }
    




public void nextInfo()
{
    if (currentPlanet == null) return;
        if (infoPointer + 1 <= currentPlanet.descriptions.Count)
        {
            infoPointer++;
            displayAndPlayInfo();

            if (infoPointer >= 1 && GetbackButton != null)
            {
                GetbackButton.gameObject.SetActive(true);
            }
    }
}

    public void lastInfo()
    {
        if (currentPlanet == null) return;
        if (infoPointer - 1 >= 0)
        {
            infoPointer--;
            displayAndPlayInfo();
            if (infoPointer == 0 && GetbackButton != null){
                GetbackButton.gameObject.SetActive(false);
            }
        }
    }

    public void displayCanvas()
    {
        canvas.enabled = true;
        if(infoPointer >0 && GetbackButton !=null){
            GetbackButton.gameObject.SetActive(true);
        }
    }

    public void hideCanvas()
{
    canvas.enabled = false;
    audio.Stop();

    if (GetbackButton != null) {
        GetbackButton.gameObject.SetActive(false);
            
        }
    }
}
