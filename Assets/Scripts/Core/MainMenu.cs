using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }
 public void Settings(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
    

    public void QuitGame()
    {
        Application.Quit();
    }

}
