using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Getback : MonoBehaviour

{
    // This will redirect me to the Main_Menu
    public string sceneToLoad = "Main_Menu";

    public void GetBack()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}

