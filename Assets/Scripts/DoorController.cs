using System;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Vector3 openPosition; // Ovea siirretään tähän paikkaan, kun se avataan
    public Vector3 closedPosition; // Ovea siirretään tähän paikkaan, kun se suljetaan
    public float speed = 2f; // Kuinka nopeasti ovi liikkuu

    private bool isOpen = false; // Tieto siitä, onko ovi auki vai ei

    // Ääniklipit
    public AudioClip doorOpenSound;  // Ääni oven avautuessa
    public AudioClip doorCloseSound; // Ääni oven sulkeutuessa
    public AudioClip bigDoorAlarmSound;   // Lisä-ääni, jos kyseessä on iso ovi (BigDoor)
    public AudioClip bigDoorIntercom; // Toinen ääni, jos kyseessä on iso ovi (BigDoor)

    public AudioSource audioSource; // AudioSource-komponentti äänten toistamiseen

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
        // Jos ovi on auki, siirrä sitä openPositioniin, muuten closedPositioniin
        if (isOpen)
        {
            transform.position = Vector3.Lerp(transform.position, openPosition, Time.deltaTime * speed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, closedPosition, Time.deltaTime * speed);
        }
    }

    public void OpenDoor()
    {
        if (!isOpen) // Varmista, ettei ääntä soiteta useaan kertaan
        {
            isOpen = true; // Avaa ovi
            PlaySound(doorOpenSound); // Soita oven avautumisääni

            // Jos objektin tägi on "BigDoor", soita lisä-ääni
            if (CompareTag("BigDoor"))
            {
                //Debug.Log("ääni sanoo pöö ja aukesi");
                PlaySound(bigDoorAlarmSound); // Soita alarmin ääni
                PlaySound(bigDoorIntercom); // Soita toinen ääni
                VolumeReducer volumeReducerComponent = GameObject.Find("EndRoomMusic").GetComponent<VolumeReducer>();
                StartCoroutine(volumeReducerComponent.FadeOutAudio());
            }
        }
    }

    public void CloseDoor()
    {
        if (isOpen) // Varmista, ettei ääntä soiteta useaan kertaan
        {
            isOpen = false; // Sulje ovi
            PlaySound(doorCloseSound); // Soita oven sulkeutumisääni

            // Jos objektin tägi on "BigDoor", soita lisä-ääni
            if (CompareTag("BigDoor"))
            {
                //Debug.Log("ääni sanoo pöö ja meni kiinni");
                PlaySound(bigDoorAlarmSound); // Soita alarmin ääni
                PlaySound(bigDoorIntercom); // Soita toinen ääni
                VolumeReducer volumeReducerComponent = GameObject.Find("EndRoomMusic").GetComponent<VolumeReducer>();
                StartCoroutine(volumeReducerComponent.FadeOutAudio());
            }
        }
    }

    // Äänen toistaminen
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null) // Tarkista, että klippi ja audioSource ovat olemassa
        {
            //audioSource.clip = clip;
            //audioSource.Play();
            audioSource.PlayOneShot(clip);
        }
    }

    // Simuloi napin painallusta oven avaamiseksi tai sulkemiseksi
    internal void PressButton()
    {
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }
}
