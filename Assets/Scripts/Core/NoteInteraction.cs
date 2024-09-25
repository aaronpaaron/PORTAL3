using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteInteractionWithRaycast : MonoBehaviour
{
    public float interactDistance = 3f;        // Distance to detect the note
    public LayerMask noteLayer;                // Layer for the note object
    public TextMeshProUGUI interactText;       // UI Text: "Press E to read"
    public GameObject noteUIPanel;             // UI Panel showing the note
    public Image noteImage;                    // UI Image to display the note image
    public MonoBehaviour playerMovement;       // Reference to player movement script (e.g., FPSController)
    private bool isReading = false;

    private Note currentNote;                  // Reference to the currently hit note

    void Update()
    {
        // Raycast from the camera's position
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        // If raycast hits the note within distance
        if (Physics.Raycast(ray, out hit, interactDistance, noteLayer))
        {
            // Check if the object hit has a Note component
            Note note = hit.collider.GetComponent<Note>();

            if (note != null)
            {
                interactText.gameObject.SetActive(true);  // Show "Press E to read" UI

                if (Input.GetKeyDown(KeyCode.E))  // When "E" is pressed
                {
                    if (!isReading)
                    {
                        currentNote = note;  // Store the reference to the current note
                        ShowNote();  // Show the note
                    }
                    else
                    {
                        HideNote();  // Hide the note
                    }
                }
            }
        }
        else
        {
            interactText.gameObject.SetActive(false);  // Hide the interact UI if not looking at the note
        }
    }

    void ShowNote()
    {
        if (currentNote != null)
        {
            // Display the image from the current note
            noteImage.sprite = currentNote.noteSprite;
            noteUIPanel.SetActive(true);
            interactText.gameObject.SetActive(false);
            Time.timeScale = 0;  // Freeze the game
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Disable player movement and camera control
            playerMovement.enabled = false;

            isReading = true;
            Debug.Log("Showing the note");
        }
    }

    void HideNote()
    {
        noteUIPanel.SetActive(false);
        Time.timeScale = 1;  // Unfreeze the game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Re-enable player movement and camera control
        playerMovement.enabled = true;

        isReading = false;
        Debug.Log("Hiding the note");
    }
}