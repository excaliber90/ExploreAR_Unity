using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    public ARTrackedImageManager trackedImages;
    public GameObject[] ArPrefabs;

    private List<GameObject> ARObjects = new List<GameObject>();
    private bool imageCurrentlyTracked = false;
    void Awake()
    {
        if (trackedImages == null)
        {
            trackedImages = FindObjectOfType<ARTrackedImageManager>();
        }
    }

    void OnEnable()
    {
        trackedImages.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImages.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    // Event Handler
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var removedImage in eventArgs.removed)
{
    var objToRemove = ARObjects.Find(obj => obj.name == removedImage.referenceImage.name);
    if (objToRemove != null)
    {
        ARObjects.Remove(objToRemove);
        Destroy(objToRemove);
    }
}

        foreach (var trackedImage in eventArgs.added)
        {
            Debug.Log("Detected image: " + trackedImage.referenceImage.name);
            foreach (var arPrefab in ArPrefabs)
            {
                if (trackedImage.referenceImage.name == arPrefab.name)
                {
                    // Check if already spawned to avoid duplication
                    if (!ARObjects.Exists(obj => obj.name == arPrefab.name))
                    {
                        var newPrefab = Instantiate(arPrefab, trackedImage.transform);
                        newPrefab.name = arPrefab.name; // Ensure name is consistent
                        ARObjects.Add(newPrefab);
                    }
                }
            }
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            foreach (var gameObject in ARObjects)
            {
                if (gameObject.name == trackedImage.referenceImage.name)
                {
                    gameObject.SetActive(trackedImage.trackingState == TrackingState.Tracking);
                    if (trackedImage.trackingState == TrackingState.Tracking)
                    {
                        Debug.Log($"Tracking:{trackedImage.referenceImage.name}");
                    }
                    else if (trackedImage.trackingState == TrackingState.Limited)
                    {
                        Debug.Log($"Limited Tracking: {trackedImage.referenceImage.name}");
                    }
                    else if (trackedImage.trackingState == TrackingState.None)
                    {
                        Debug.Log($"Lost Tracking:{trackedImage.referenceImage.name}");
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
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                anyTracked = true;
                break;
            }
        }

        // Only log when state changes
        if (anyTracked && !imageCurrentlyTracked)
        {
            Debug.Log("At least one image is being tracked.");
            imageCurrentlyTracked = true;
        }
        else if (!anyTracked && imageCurrentlyTracked)
        {
            Debug.Log("Image Not Detected");
            imageCurrentlyTracked = false;
        }
    }
}
