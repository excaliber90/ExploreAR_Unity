using UnityEngine;
using UnityEngine.SceneManagement;

public class TwoPanelManager : MonoBehaviour
{
   private const string NEXT_AR_SCENE_KEY = "NextARScene";  


    public void OnCameraClicked()
    {
        string nextScene = PlayerPrefs.GetString(NEXT_AR_SCENE_KEY, "");
        if (!string.IsNullOrEmpty(nextScene))
        {
            Debug.Log("Loading AR scene: " + nextScene);
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            Debug.LogError("No AR scene stored in PlayerPrefs!");
        }
    }

    // Called when "Open Gallery" button is clicked
    public void OnGalleryClicked()
    {
        Debug.Log("Gallery option selected!");
        // 🔧 Here you can add your gallery-opening logic
        // If after selecting an image you want to load the AR scene, do the same as OnCameraClicked()
    }
}
