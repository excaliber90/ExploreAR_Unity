using UnityEngine;
using TMPro;  // Only if you use TMP for info panel text

[System.Serializable]
public class ImagePrefabMapping
{
    public string imageName;    
    public GameObject prefab;   
    public string title;        
    public string description;  
}

public class GalleryHandler : MonoBehaviour
{
    [Header("Prefabs")]
    public ImagePrefabMapping[] mappings;

    [Header("UI Elements")]
    public GameObject optionPanel;       // Panel with Gallery/Camera buttons
    public GameObject infoPanel;         // Panel to show prefab info
    public TMP_Text titleText;
    public TMP_Text descriptionText;

    [Header("Spawn Settings")]
    public Camera sceneCamera;           // Assign your manually added camera here
    public float spawnDistance = 2f;     // Distance in front of the camera
    public Vector3 prefabScale = Vector3.one;

    // ================== Open Gallery ==================
    public void OpenGallery()
    {
        NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                string fileName = System.IO.Path.GetFileNameWithoutExtension(path);

                // Spawn prefab in front of assigned camera
                SpawnPrefab(fileName);

                // Hide the option panel
                if (optionPanel != null)
                    optionPanel.SetActive(false);
            }
        }, "Select an image", "image/*");
    }
/*
    public void OpenCamera()
    {
        
        Debug.Log("Camera button clicked. Implement later if needed.");
    }
*/  
    public void OpenCamera()
{
    NativeCamera.TakePicture((path) =>
    {
        if (path != null)
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(path);

            // Spawn prefab in front of assigned camera
            SpawnPrefab(fileName);

            // Hide the option panel
            if (optionPanel != null)
                optionPanel.SetActive(false);
        }
    }, maxSize: 1024);
}

    private void SpawnPrefab(string imageName)
    {
        foreach (var map in mappings)
        {
            if (map.imageName == imageName)
            {
                if (sceneCamera == null)
     {
        Debug.LogWarning("Scene camera not assigned!");
         return;
    }

                // Spawn in front of the assigned camera
                Vector3 spawnPos = sceneCamera.transform.position + sceneCamera.transform.forward * spawnDistance;
                GameObject obj = Instantiate(map.prefab, spawnPos, Quaternion.identity);

                // Scale and rotate to face camera
                obj.transform.localScale = prefabScale;
                obj.transform.LookAt(sceneCamera.transform);
                obj.transform.Rotate(0, 180, 0);

                // Show info panel
                if (infoPanel != null)
                {
                    infoPanel.SetActive(true);
                    if (titleText != null) titleText.text = map.title;
                    if (descriptionText != null) descriptionText.text = map.description;
                }

                Debug.Log("Prefab spawned for: " + imageName);
                return;
            }
        }

        Debug.LogWarning("No prefab found for: " + imageName);
    }

    // ================== Editor Testing ==================
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SpawnPrefab("Earth");
        if (Input.GetKeyDown(KeyCode.Alpha2)) SpawnPrefab("Mars");
    }
}
