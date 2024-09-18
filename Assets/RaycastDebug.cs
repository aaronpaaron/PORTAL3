using UnityEngine;

public class RaycastDebug : MonoBehaviour
{
    public float interactDistance = 3f;        // Distance to detect the note
    public LayerMask noteLayer;                // Layer for the note object

    void Update()
    {
        // Cast a ray from the camera's position forward
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Check if the ray hits a note object within the interactDistance
        if (Physics.Raycast(ray, out hit, interactDistance, noteLayer))
        {
            Debug.Log("Looking at the note!");  // Print to the console when the note is detected
        }
        else
        {
            Debug.Log("Not looking at the note.");
        }
    }
}