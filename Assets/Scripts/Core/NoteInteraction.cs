using UnityEngine;
using UnityEngine.UI;

public class NoteInteraction : MonoBehaviour
{
    public float interactDistance = 3f;       // Distance to detect the note
    public LayerMask noteLayer;               // Layer for the note object
    public Text interactText;                 // UI Text: "Press E to read"
    public GameObject noteUIPanel;            // UI Panel showing the note
    private bool isReading = false;           // Whether the player is reading the note

    void Update()
    {
        // Raycast to detect if the player is looking at the note
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactDistance, noteLayer))
        {
            // Show "Press E to read" text when looking at the note
            interactText.gameObject.SetActive(true);

            // If the player presses "E" to interact with the note
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isReading)
                {
                    // Start reading the note
                    ShowNote();
                }
                else
                {
                    // Stop reading the note
                    HideNote();
                }
            }
        }
        else
        {
            // Hide the "Press E to read" text if not looking at the note
            interactText.gameObject.SetActive(false);
        }
    }

    void ShowNote()
    {
        // Show the note on the screen
        noteUIPanel.SetActive(true);
        interactText.gameObject.SetActive(false);
        
        // Freeze the game by setting timeScale to 0
        Time.timeScale = 0;
        
        // Make sure the cursor is unlocked so the player can interact with the UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        isReading = true;
    }

    void HideNote()
    {
        // Hide the note from the screen
        noteUIPanel.SetActive(false);
        
        // Resume the game by setting timeScale back to 1
        Time.timeScale = 1;
        
        // Lock the cursor again for FPS movement
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        isReading = false;
    }
}