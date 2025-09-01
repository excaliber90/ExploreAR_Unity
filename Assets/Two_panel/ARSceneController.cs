using UnityEngine;
using System.Collections;

public class ARSceneController : MonoBehaviour
{
    public GameObject optionPanel;       // The panel containing the two buttons
    public GameObject ARCamera;          // Your AR camera / ARSessionOrigin object
    public GalleryHandler galleryHandler; // Assign your existing GalleryHandler

    void Start()
    {
        optionPanel.SetActive(true);   // Show options at start
        ARCamera.SetActive(false);     // Disable camera initially
    }

    // Called when user clicks "Open Gallery"
    public void OnOpenGalleryClicked()
    {
        optionPanel.SetActive(false);
        if (galleryHandler != null)
            galleryHandler.OpenGallery();
    }

    // Called when user clicks "Camera"
    public void OnCameraClicked()
    {
        optionPanel.SetActive(false);
        StartCoroutine(StartARCamera());
    }

    private IEnumerator StartARCamera()
    {
        // Request camera permission
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            ARCamera.SetActive(true);   // Enable AR camera
            // Optionally enable AR tracking scripts here (ARTrackedImageManager, etc.)
            Debug.Log("Camera permission granted, AR started");
        }
        else
        {
            Debug.Log("Camera permission denied");
        }
    }
}
