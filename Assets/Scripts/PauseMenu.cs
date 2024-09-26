using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    public static bool isPaused;

    public CharacterController playerMovement;

    void Start()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenu.SetActive(true);
        //Time.timeScale = 0f;
        isPaused = true;
        playerMovement.enabled = false;
    } 


    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        //Time.timeScale = 1f;
        isPaused = false;
        playerMovement.enabled = true;
    }

    public void GoToMainMenu()
    {
        //Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

}