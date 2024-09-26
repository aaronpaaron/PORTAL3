using UnityEngine;

public class TriggerAudioPlayer : MonoBehaviour
{
    private bool hasPlayed = false; // Tarkistetaan, onko ääni jo soitettu

    private void OnTriggerEnter(Collider other)
    {
        // Tarkistetaan, onko triggeriin osunut pelaaja
        if (other.CompareTag("Player")) // Varmista, että pelaajalla on tag "Player"
        {
            if (!hasPlayed) // Tarkistetaan, onko ääni jo soitettu
            {
                // Yritetään löytää ääniresurssi, jonka nimi vastaa GameObjectin nimeä
                AudioClip clip = Resources.Load<AudioClip>(gameObject.name);
                
                if (clip != null)
                {
                    // Luo uusi AudioSource ja soita ääni
                    AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                    audioSource.clip = clip;
                    audioSource.Play();

                    // Poista AudioSource, kun ääni on soinut
                    Destroy(audioSource, clip.length);
                    
                    hasPlayed = true; // Merkitään, että ääni on soitettu
                }
                else
                {
                    Debug.LogWarning("Audio clip not found for " + gameObject.name);
                }
            }
            else
            {
                Debug.Log("Ääni on jo soitettu. Pelaaja ei voi kuunnella sitä uudestaan.");
            }
        }
    }
}
