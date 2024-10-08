using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Endscreenswitch : MonoBehaviour
{
    public float changeTime;
    public string sceneName;
    void Start()
    {
        sceneName = "Main Menu";
    }
    private void Update()
    {
        changeTime -= Time.deltaTime;
        if(changeTime <= 0)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}