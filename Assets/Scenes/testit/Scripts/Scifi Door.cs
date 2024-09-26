using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator anim;
    public Transform player;
    public Transform door;

    // Äänet
    public AudioClip doorOpenSound;   // Ääni oven avautuessa
    public AudioClip doorCloseSound;  // Ääni oven sulkeutuessa
    private AudioSource audioSource;  // AudioSource-komponentti äänten toistoon

    private bool isOpen = false; // Jotta tiedämme onko ovi auki vai kiinni

    void Start()
    {
        // Lisää AudioSource-komponentti, jos sitä ei vielä ole
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.position, door.position);

        if (distance <= 5)
        {
            if (!isOpen)
            {
                // Jos ovi on kiinni ja pelaaja on tarpeeksi lähellä, avaa ovi ja soita avautumisääni
                anim.SetBool("Near", true);
                PlaySound(doorOpenSound);
                isOpen = true;
            }
        }
        else
        {
            if (isOpen)
            {
                // Jos ovi on auki ja pelaaja on kaukana, sulje ovi ja soita sulkeutumisääni
                anim.SetBool("Near", false);
                PlaySound(doorCloseSound);
                isOpen = false;
            }
        }
    }

    // Äänen toistaminen
    void PlaySound(AudioClip clip)
    {
        if (clip != null)  // Tarkista, että ääniklippejä on asetettu
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
