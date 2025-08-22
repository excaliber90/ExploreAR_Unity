using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class BackButtonHandler : MonoBehaviour
{
    public void GoToMainMenu()
    {

        ARSession arSession = FindObjectOfType<ARSession>();
        if (arSession != null)
            arSession.Reset(); // stops tracking and camera feed

        // Load Main Menu
        SceneManager.LoadScene("MainMenu");
    }
}
