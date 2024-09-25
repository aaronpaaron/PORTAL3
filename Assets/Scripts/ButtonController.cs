using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public DoorController door; // Viittaus DoorControlleriin (ovi)
    public AudioClip pressSound; // Ääni kun nappi painetaan
    public AudioClip releaseSound; // Ääni kun nappi vapautetaan
    private AudioSource audioSource;

    void Start()
    {
        // Hanki tai lisää AudioSource-komponentti tähän GameObjectiin
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kun jokin menee napin päälle, avaa ovi
        if (other.CompareTag("Cube")) // Tarkistetaan, että törmäävä objekti on relevantti
        {
            door.OpenDoor();

            // Toistetaan ääni kun nappi painetaan
            PlaySound(pressSound);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Kun objekti poistuu napin päältä, sulje ovi
        if (other.CompareTag("Cube"))
        {
            door.CloseDoor();

            // Toistetaan ääni kun nappi vapautetaan
            PlaySound(releaseSound);
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip); // Toistetaan ääni kerran
        }
    }
}
