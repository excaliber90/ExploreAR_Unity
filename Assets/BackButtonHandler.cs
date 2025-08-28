using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class BackButtonHandler : MonoBehaviour
{
    // This button will redirect me to the Main Menu
    public void GoToMainMenu()
    {

        ARSession arSession = FindObjectOfType<ARSession>();
        if (arSession != null)
            arSession.Reset(); // stops tracking and camera feed

        SceneManager.LoadScene("MainMenu");
    }
}
