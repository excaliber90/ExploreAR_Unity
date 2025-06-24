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

    private AudioSource audio;
    private PlanetInfo currentPlanet;
    private Transform scaledPlanet;
    private Vector3 originalScale;

    private int infoPointer = 0;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        canvas.enabled = false;
        /*if (GetbackButton != null) {
            GetbackButton.gameObject.SetActive(false); //Hide Go back button 
        }*/
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

        // Hide Go Back button by default
        if (infoPointer == 0 && GetbackButton != null)
        {
            GetbackButton.gameObject.SetActive(false);
        }

        // Show text
        if (infoPointer < currentPlanet.descriptions.Count)
            infoBox.text = currentPlanet.descriptions[infoPointer];
        else
            infoBox.text = "";

        // Play audio
        if (infoPointer < currentPlanet.audioClips.Count)
        {
            audio.Stop();
            audio.clip = currentPlanet.audioClips[infoPointer];
            audio.Play();
        }
     
            
        // Show image
       /* if (infoPointer < currentPlanet.images.Count)
            rawImage.texture = currentPlanet.images[infoPointer];
        else
            rawImage.texture = null;*/
    }
    




public void nextInfo()
{
    if (currentPlanet == null) return;
        if (infoPointer + 1 < currentPlanet.descriptions.Count)
        {
            infoPointer++;
            displayAndPlayInfo();

            if (infoPointer >= 1 && GetbackButton != null) {
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
