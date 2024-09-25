using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController3 : MonoBehaviour
{
    public DoorController doorA; // Viittaus ensimmäiseen ovelle
    public DoorController doorB; // Viittaus toiseen ovelle
    private bool isPressed = false; // Tarkistaa, onko nappia painettu

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
        // Kun jokin menee napin päälle
        if (other.CompareTag("Cube") && !isPressed)
        {
            isPressed = true; // Merkitään nappi painetuksi
            doorA.OpenDoor(); // Avaa ovi A
            doorB.OpenDoor(); // Avaa ovi B
            
            // Toistetaan ääni kun nappi painetaan
            PlaySound(pressSound);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Kun objekti poistuu napin päältä
        if (other.CompareTag("Cube") && isPressed)
        {
            isPressed = false; // Merkitään nappi vapautetuksi
            doorA.CloseDoor(); // Sulje ovi A
            doorB.CloseDoor(); // Sulje ovi B
            
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
