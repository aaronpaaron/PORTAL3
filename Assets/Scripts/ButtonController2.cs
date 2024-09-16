using UnityEngine;

public class ButtonController2 : MonoBehaviour
{
    public DoorController2 door; // Viittaus DoorControlleriin (ovi)

    private void OnTriggerEnter(Collider other)
    {
        // Kun jokin menee napin päälle, kerro ovesta
        if (other.CompareTag("Cube"))
        {
            door.PressButton();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Kun objekti poistuu napin päältä, ilmoita siitä ovesta
        if (other.CompareTag("Cube"))
        {
            door.ReleaseButton();
        }
    }
}
