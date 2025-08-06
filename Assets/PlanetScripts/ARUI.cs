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
    public Button NextButton;

    

    private AudioSource audio;
    private PlanetInfo currentPlanet;
    private Transform scaledPlanet;
    private Vector3 originalScale;
    public GameObject quizPanel;
    public TMP_Text quizQuestion;
    public Button[] answerButtons;
    public TMP_Text feedbackText;

    public List<QuizData> quizDataAssets;  // Editable from Inspector

    private Dictionary<string, QuizData> quizDictionary = new Dictionary<string, QuizData>();

    private int infoPointer = 0;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        canvas.enabled = false;
        quizPanel.SetActive(false);

        // Populate Dictionary from List
        foreach (var quiz in quizDataAssets)
        {
            string key = quiz.planetName.Trim().ToLower();
            if (!quizDictionary.ContainsKey(key))
                quizDictionary.Add(key, quiz);
        }
    }

    void ShowQuiz(string planetName)
    {
        planetName = planetName.Trim().ToLower();

        if (!quizDictionary.ContainsKey(planetName))
        {
            Debug.LogWarning("No quiz found for planet: " + planetName);
            return;
        }

        QuizData quiz = quizDictionary[planetName];
        quizPanel.SetActive(true);
        quizQuestion.text = quiz.question;
        feedbackText.text = "";

        for (int i = 0; i < answerButtons.Length; i++)
        {
            // Needed to capture button index
            if (i < quiz.options.Length)
            {
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].GetComponentInChildren<TMP_Text>().text = quiz.options[i];
                int index = i;
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => CheckAnswer(index, quiz.correctAnswerIndex));
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
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
        Debug.Log("InfoPointer: " + infoPointer + " / Total: " + currentPlanet.descriptions.Count);

        // Hide Go Back button by default
        if (infoPointer == 0 && GetbackButton != null)
        {
            GetbackButton.gameObject.SetActive(false);
        }
        else if (GetbackButton != null)
        {
            GetbackButton.gameObject.SetActive(true);
        }

           // Check if NextButton should be shown or hidden
    if (infoPointer >= currentPlanet.descriptions.Count - 1)
    {
        if (NextButton != null)
            NextButton.gameObject.SetActive(false); // Hide Next at last info
    }
    else
    {
        if (NextButton != null)
            NextButton.gameObject.SetActive(true); // Show Next if not at last
    }


        // Show Info Text
        if (infoPointer < currentPlanet.descriptions.Count)
        {
            infoBox.text = currentPlanet.descriptions[infoPointer];
        }
        else if (infoPointer == currentPlanet.descriptions.Count)
        {
            infoBox.text = "";
            ShowQuiz(currentPlanet.name.Trim().ToLower());
            if (NextButton != null)
            NextButton.gameObject.SetActive(false); // Hide Next during quiz

            return; 
        }

        // Play audio
        if (infoPointer < currentPlanet.audioClips.Count && infoPointer<currentPlanet.descriptions.Count)
        {
            audio.Stop();
            audio.clip = currentPlanet.audioClips[infoPointer];
            audio.Play();
        }
     
            
        // Shows image
       /* if (infoPointer < currentPlanet.images.Count)
            rawImage.texture = currentPlanet.images[infoPointer];
        else
            rawImage.texture = null;*/
    }



    public void nextInfo()
    {
        if (currentPlanet == null) return;
        if (infoPointer < currentPlanet.descriptions.Count) {

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
