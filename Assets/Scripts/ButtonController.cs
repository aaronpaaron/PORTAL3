using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public DoorController door; // Viittaus DoorControlleriin (ovi)

    private void OnTriggerEnter(Collider other)
    {
        // Kun jokin menee napin päälle, avaa ovi
        if (other.CompareTag("Cube")) // Tarkistetaan, että törmäävä objekti on pelaaja tai muu relevantti objekti
        {
            door.OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Kun objekti poistuu napin päältä, sulje ovi
        if (other.CompareTag("Cube"))
        {
            door.CloseDoor();
        }
    }
}