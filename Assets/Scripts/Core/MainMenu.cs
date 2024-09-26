using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    void Start()
    {
        Cursor.visible = true;
    }
    
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("SampleScene");
        Cursor.visible = false;
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
