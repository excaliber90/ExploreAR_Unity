using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using TMPro;

public class Buttontest : MonoBehaviour
{
    public TMP_Text text;

    void Start()
    {
        
    }


    void Update()
    {
    }
       public void ClickButton(){
        text.text ="You clicked me! ";
       }
}
