using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Auth;

public class Main_Menu : MonoBehaviour
{
    public GameObject searchPanel;
    public GameObject profilePanel;
    public TMP_Text profileUserName_Text;
    public TMP_Text profileUserEmail_Text;

    private const string USERNAME_KEY = "username";
    private const string EMAIL_KEY = "email";
    private const string NEXT_AR_SCENE_KEY = "NextARScene";

    void Start()
    {
        if (profileUserName_Text != null)
            profileUserName_Text.text = PlayerPrefs.GetString(USERNAME_KEY, "Guest");

        if (profileUserEmail_Text != null)
            profileUserEmail_Text.text = PlayerPrefs.GetString(EMAIL_KEY, "No Email");
    }

    //If the scene is not in the build setting scene then this will send the message 

    public void OnAnimalClicked()
    {

        Debug.Log("Animal_Kingdom button clicked!");
        PlayerPrefs.SetString(NEXT_AR_SCENE_KEY, "Animal_Kingdom");
        SceneManager.LoadScene("Two_Panel");
    }
    public void OnSolarSystemClicked()
    {
        Debug.Log("Solar System button clicked!");
        PlayerPrefs.SetString(NEXT_AR_SCENE_KEY, "AR_SolarSystem");
        SceneManager.LoadScene("AR_SolarSystem");
    }
    public void OnQuizClicked()
    {
        if (Application.CanStreamedLevelBeLoaded("Quiz_Scene"))
            SceneManager.LoadScene("Quiz_Scene");
        else
            Debug.LogError("Scene 'Quiz_Scene' not found in Build Settings!");
    }

    public void OnProfileClicked()
    {
        profilePanel.SetActive(true);
        searchPanel.SetActive(false);
    }

    public void OnSearchClicked()
    {
        searchPanel.SetActive(true);
        profilePanel.SetActive(false);
    }

    public void OnClosePanels()
    {
        searchPanel.SetActive(false);
        profilePanel.SetActive(false);
    }

    public void OnLogoutClicked()
    {
        FirebaseAuth.DefaultInstance.SignOut();
        PlayerPrefs.DeleteKey(USERNAME_KEY);
        PlayerPrefs.DeleteKey(EMAIL_KEY);
        Debug.Log("User logged out.");
        SceneManager.LoadScene("Login");
    }
    public void Onback()
    {
        Debug.Log("You are redirected to the main menu.");
        SceneManager.LoadScene("Main_Menu");
    }
}
