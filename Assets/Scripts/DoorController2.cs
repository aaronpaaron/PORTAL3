using UnityEngine;

public class DoorController2 : MonoBehaviour
{
    public Vector3 openPosition; // Ovea siirretään tähän paikkaan, kun se avataan
    public Vector3 closedPosition; // Ovea siirretään tähän paikkaan, kun se suljetaan
    public float speed = 2f; // Kuinka nopeasti ovi liikkuu
    
    private bool isOpen = false; // Tieto siitä, onko ovi auki vai ei
    private int buttonsPressed = 0; // Kuinka monta nappia on painettu

    // Ääniklipit
    public AudioClip doorOpenSound;  // Ääni oven avautuessa
    public AudioClip doorCloseSound; // Ääni oven sulkeutuessa
    private AudioSource audioSource; // AudioSource-komponentti äänten toistamiseen

    private void Start()
    {
        // Hanki tai lisää AudioSource-komponentti
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        // Jos kaikki napit on painettu (esim. 2), avaa ovi, muuten sulje ovi
        if (isOpen)
        {
            transform.position = Vector3.Lerp(transform.position, openPosition, Time.deltaTime * speed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, closedPosition, Time.deltaTime * speed);
        }
    }

    public void PressButton()
    {
        buttonsPressed++;

        // Jos kaksi nappia on painettuna, avaa ovi ja soita ääni
        if (buttonsPressed == 2 && !isOpen) 
        {
            isOpen = true;
            PlaySound(doorOpenSound); // Soita oven avautumisääni
        }
    }

    public void ReleaseButton()
    {
        buttonsPressed--;

        // Jos yhtäkään nappia ei ole enää painettuna, sulje ovi ja soita ääni
        if (buttonsPressed < 2 && isOpen)
        {
            isOpen = false;
            PlaySound(doorCloseSound); // Soita oven sulkeutumisääni
        }
    }

    // Äänen toistaminen
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null) // Tarkista, että klippi ja audioSource ovat olemassa
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
