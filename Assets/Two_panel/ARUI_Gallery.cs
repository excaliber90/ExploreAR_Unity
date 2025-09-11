/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ARUI_Gallery : MonoBehaviour
{
    [Header("UI References")]
    public Canvas infoCanvas;           
    public TMP_Text infoBox;            
    public Button GetbackButton;
    public Button NextButton;
    public GameObject OptionPanel;      

    private AudioSource audioSource;
    private PlanetInfo currentPrefabInfo;
    private Transform selectedPrefab;
    private Vector3 originalScale;
    private int infoPointer = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        infoCanvas.enabled = false;      // Hide info panel at start
        OptionPanel.SetActive(true);     // Show option buttons
    }

    /// <summary>
    /// Call this method when a gallery prefab is clicked.
    /// </summary>
    public void OnPrefabClicked(Transform prefabTransform, PlanetInfo prefabInfo)
    {
        // Reset previous prefab scale
        if (selectedPrefab != null && selectedPrefab != prefabTransform)
        {
            selectedPrefab.localScale = originalScale;
        }

        selectedPrefab = prefabTransform;
        originalScale = selectedPrefab.localScale;
        selectedPrefab.localScale = originalScale * 1.2f; // optional scale up

        currentPrefabInfo = prefabInfo;
        infoPointer = 0;

        // Show info panel, hide option buttons
        infoCanvas.enabled = true;
        OptionPanel.SetActive(false);

        DisplayInfo();
    }

    void DisplayInfo()
    {
        if (currentPrefabInfo == null) return;

        // Show description text
        if (infoPointer < currentPrefabInfo.descriptions.Count)
            infoBox.text = currentPrefabInfo.descriptions[infoPointer];

        // Play audio
        if (infoPointer < currentPrefabInfo.audioClips.Count)
        {
            audioSource.Stop();
            audioSource.clip = currentPrefabInfo.audioClips[infoPointer];
            audioSource.Play();
        }

        // Button visibility
        GetbackButton.gameObject.SetActive(infoPointer > 0);
        NextButton.gameObject.SetActive(infoPointer < currentPrefabInfo.descriptions.Count - 1);
    }

    public void NextInfo()
    {
        if (currentPrefabInfo == null) return;

        if (infoPointer < currentPrefabInfo.descriptions.Count - 1)
        {
            infoPointer++;
            DisplayInfo();
        }
    }

    public void PreviousInfo()
    {
        if (currentPrefabInfo == null) return;

        if (infoPointer > 0)
        {
            infoPointer--;
            DisplayInfo();
        }
    }

    public void HideInfoPanel()
    {
        infoCanvas.enabled = false;
        audioSource.Stop();
        OptionPanel.SetActive(true);

        // Reset prefab scale
        if (selectedPrefab != null)
        {
            selectedPrefab.localScale = originalScale;
            selectedPrefab = null;
        }
    }
}*/
