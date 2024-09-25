using UnityEngine;

public class ButtonController2 : MonoBehaviour
{
    public DoorController2 door; // Viittaus DoorControlleriin (ovi)

    public AudioClip pressSound; // Ääni nappia painettaessa
    public AudioClip releaseSound; // Ääni nappia vapautettaessa
    private AudioSource audioSource; // AudioSource-komponentti äänen toistamiseen

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
        // Kun jokin menee napin päälle, kerro ovesta
        if (other.CompareTag("Cube"))
        {
            door.PressButton(); // Kerro ovesta

            // Toistetaan ääni kun nappi painetaan
            PlaySound(pressSound);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Kun objekti poistuu napin päältä, ilmoita siitä ovesta
        if (other.CompareTag("Cube"))
        {
            door.ReleaseButton(); // Ilmoita ovesta

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
