
using UnityEngine;
using UnityEngine.SceneManagement;
public class Gobackbutton : MonoBehaviour
{

    public string sceneToLoad = "Main_Menu"; 

    public void GoBack()
    {
        SceneManager.LoadScene(sceneToLoad);
    }


}
