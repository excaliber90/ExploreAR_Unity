
using UnityEngine;
using UnityEngine.SceneManagement;
public class Gobackbutton : MonoBehaviour
{
    // let me do back to the main menu
    public string sceneToLoad = "Main_Menu"; 

    public void GoBack()
    {
        SceneManager.LoadScene(sceneToLoad);
    }


}
