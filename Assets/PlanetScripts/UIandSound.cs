using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIandSound : MonoBehaviour
{

    public TMP_Text infoBox;

    public AudioClip Marsclip1;
    public AudioClip Marsclip2;
    public AudioClip Marsclip3;

    AudioSource audio;

    string UIState = "void"; 
    int infoID = 1; 


    string marsText1 = "Mars, often called the \"Red Planet,\"";
    string marsText2 = "It is Earth's neighbor and has a cold, \n" 
                        +"dry surface covered in iron oxide, giving it a reddish appearance. ";
    string marsText3 = "Though not hospitable like Earth.";
     string marsText4 = "";



    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();     
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))//left mouse click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {                
                if (hit.transform.tag == "mars")
                {                   
                    UIState = "Mars";
                    infoID = 1;
                    infoBox.text = marsText1;
                    audio.PlayOneShot(Marsclip1, 1f);
                }

                if (hit.transform.tag == "earth")
                {
                    infoBox.text = "Earth \n  Mostly Harmless!";
                    UIState = "Earth";
                }


                if (hit.transform.tag == "marsInfo")
                {
                    Destroy(hit.transform.gameObject);
                    infoBox.text = "Select a Planet";
                    UIState = "void";

                }


                if (hit.transform.tag == "earthInfo")
                {
                    Destroy(hit.transform.gameObject);
                    infoBox.text = "Select a Planet";
                    UIState = "void";

                }

            }
        }
    }

    public void nextButton()
    {
        if(audio.isPlaying)
        {
            audio.Stop();
        }
        
        if (UIState == "Mars")
        {
            switch(infoID)
            { 
                case(1):
                    infoID = 2;
                    infoBox.text = marsText2;
                    audio.PlayOneShot(Marsclip2, 1f);
                    break;
                case (2):        
                    infoID = 3;
                    infoBox.text = marsText3;
                    //audio.PlayOneShot(Marsclip3, 1f); //To Add
                    break;
                default:
                    break;
            }
        }

        if (UIState == "Earth")
        {

        }
    }
}