/*using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    [Header("AR Prefabs")]
    public GameObject[] ArPrefabs;
    public float rotationSpeed = 20f;
    private ARTrackedImageManager trackedImages;
    private List<GameObject> ARObjects = new List<GameObject>();
    private bool imageCurrentlyTracked = false;

    void Awake()
    {
        // Automatically find the ARTrackedImageManager in the scene
        trackedImages = FindObjectOfType<ARTrackedImageManager>();

        if (ArPrefabs != null)
        {
            foreach (var prefab in ArPrefabs)
            {
                if (prefab != null)
                    Debug.Log("Prefab Name:" + prefab.name);
                else
                    Debug.LogWarning("One of the ARPrefabs is missing in the Inspector!");
            }
        }
        else
        {
            Debug.LogWarning("AR Prefabs array is not assigned!");
        }
        if (trackedImages != null && trackedImages.referenceLibrary != null)
        {
            var referenceLibrary = trackedImages.referenceLibrary;
            for (int i = 0; i < referenceLibrary.count; i++)
            {
                var image = referenceLibrary[i];
                Debug.Log("Reference Image Name: " + image.name);
             }
            
         }
    }

    void OnEnable()
    {
        if (trackedImages != null)
            trackedImages.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        if (trackedImages != null)
            trackedImages.trackedImagesChanged -= OnTrackedImagesChanged;
    }
   
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Handle removed images
        foreach (var removedImage in eventArgs.removed)
        {
            var objToRemove = ARObjects.Find(obj => obj.name == removedImage.referenceImage.name);
            if (objToRemove != null)
            {
                ARObjects.Remove(objToRemove);
                Destroy(objToRemove);
                Debug.Log("Removed AR object: " + removedImage.referenceImage.name);
            }
        }

        // Handle added images
        foreach (var trackedImage in eventArgs.added)
        {
            Debug.Log("Detected image: " + trackedImage.referenceImage.name);
            foreach (var arPrefab in ArPrefabs)
            {
                if (trackedImage.referenceImage.name == arPrefab.name)
                {
                    if (!ARObjects.Exists(obj => obj.name == arPrefab.name))
                    {
                        var newPrefab = Instantiate(arPrefab, trackedImage.transform);
                        newPrefab.name = arPrefab.name;
                        ARObjects.Add(newPrefab);
                        Debug.Log("Instantiated AR object: " + arPrefab.name);
                    }
                }
            }
        }

        // Handle updated images
        foreach (var trackedImage in eventArgs.updated)
        {
            foreach (var gameObject in ARObjects)
            {
                if (gameObject.name == trackedImage.referenceImage.name)
                {
                    // Keep object active if Tracking or Limited
                    bool isVisible = trackedImage.trackingState != TrackingState.None;
                    gameObject.SetActive(isVisible);

                    switch (trackedImage.trackingState)
                    {
                        case TrackingState.Tracking:
                            Debug.Log($"Tracking: {trackedImage.referenceImage.name}");
                            break;
                        case TrackingState.Limited:
                            Debug.Log($"Limited Tracking: {trackedImage.referenceImage.name}");
                            break;
                        case TrackingState.None:
                            Debug.Log($"Lost Tracking: {trackedImage.referenceImage.name}");
                            break;
                    }
                }
            }
        }

        CheckIfAnyImageIsTracked();
    }

    private void CheckIfAnyImageIsTracked()
    {
        bool anyTracked = false;

        foreach (var trackedImage in trackedImages.trackables)
        {
            if (trackedImage.trackingState != TrackingState.None) // Treat Limited as 
            {
                anyTracked = true;
                break;
            }
        }

        if (anyTracked && !imageCurrentlyTracked)
        {
            Debug.Log("At least one image is being tracked.");
            imageCurrentlyTracked = true;
        }
        else if (!anyTracked && imageCurrentlyTracked)
        {
            Debug.Log("No images are currently detected.");
            imageCurrentlyTracked = false;
        }
    }
}
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    [Header("AR Prefabs")]
    public GameObject[] ArPrefabs;

    [Header("Rotation Settings")]
    public float rotationSpeed = 20f; // degrees per second (same as QuizManager default)

    private ARTrackedImageManager trackedImages;
    private List<GameObject> ARObjects = new List<GameObject>();
    private bool imageCurrentlyTracked = false;

    void Awake()
    {
        // Automatically find the ARTrackedImageManager in the scene
        trackedImages = FindObjectOfType<ARTrackedImageManager>();

        if (ArPrefabs != null)
        {
            foreach (var prefab in ArPrefabs)
            {
                if (prefab != null)
                    Debug.Log("Prefab Name:" + prefab.name);
                else
                    Debug.LogWarning("One of the ARPrefabs is missing in the Inspector!");
            }
        }
        else
        {
            Debug.LogWarning("AR Prefabs array is not assigned!");
        }
        
        if (trackedImages != null && trackedImages.referenceLibrary != null)
        {
            var referenceLibrary = trackedImages.referenceLibrary;
            for (int i = 0; i < referenceLibrary.count; i++)
            {
                var image = referenceLibrary[i];
                Debug.Log("Reference Image Name: " + image.name);
            }
        }
    }

    void OnEnable()
    {
        if (trackedImages != null)
            trackedImages.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        if (trackedImages != null)
            trackedImages.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void Update()
    {
        // Simple rotation for all active AR objects (same as QuizManager approach)
        foreach (var arObject in ARObjects)
        {
            if (arObject != null && arObject.activeInHierarchy)
            {
                // Simple rotation like in QuizManager
                arObject.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Handle removed images
        foreach (var removedImage in eventArgs.removed)
        {
            var objToRemove = ARObjects.Find(obj => obj.name == removedImage.referenceImage.name);
            if (objToRemove != null)
            {
                ARObjects.Remove(objToRemove);
                Destroy(objToRemove);
                Debug.Log("Removed AR object: " + removedImage.referenceImage.name);
            }
        }

        // Handle added images
        foreach (var trackedImage in eventArgs.added)
        {
            Debug.Log("Detected image: " + trackedImage.referenceImage.name);
            foreach (var arPrefab in ArPrefabs)
            {
                if (trackedImage.referenceImage.name == arPrefab.name)
                {
                    if (!ARObjects.Exists(obj => obj.name == arPrefab.name))
                    {
                        var newPrefab = Instantiate(arPrefab, trackedImage.transform);
                        newPrefab.name = arPrefab.name;
                        ARObjects.Add(newPrefab);
                        
                        Debug.Log("Instantiated AR object: " + arPrefab.name);
                    }
                }
            }
        }

        // Handle updated images
        foreach (var trackedImage in eventArgs.updated)
        {
            foreach (var gameObject in ARObjects)
            {
                if (gameObject.name == trackedImage.referenceImage.name)
                {
                    // Keep object active if Tracking or Limited
                    bool isVisible = trackedImage.trackingState != TrackingState.None;
                    gameObject.SetActive(isVisible);

                    switch (trackedImage.trackingState)
                    {
                        case TrackingState.Tracking:
                            Debug.Log($"Tracking: {trackedImage.referenceImage.name}");
                            break;
                        case TrackingState.Limited:
                            Debug.Log($"Limited Tracking: {trackedImage.referenceImage.name}");
                            break;
                        case TrackingState.None:
                            Debug.Log($"Lost Tracking: {trackedImage.referenceImage.name}");
                            break;
                    }
                }
            }
        }

        CheckIfAnyImageIsTracked();
    }

    private void CheckIfAnyImageIsTracked()
    {
        bool anyTracked = false;

        foreach (var trackedImage in trackedImages.trackables)
        {
            if (trackedImage.trackingState != TrackingState.None)
            {
                anyTracked = true;
                break;
            }
        }

        if (anyTracked && !imageCurrentlyTracked)
        {
            Debug.Log("At least one image is being tracked.");
            imageCurrentlyTracked = true;
        }
        else if (!anyTracked && imageCurrentlyTracked)
        {
            Debug.Log("No images are currently detected.");
            imageCurrentlyTracked = false;
        }
    }
}