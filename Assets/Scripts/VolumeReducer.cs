using UnityEngine;

public class VolumeReducer : MonoBehaviour
{
    public DoorController doorController; // Viittaus DoorController-komponenttiin
    public float fadeDuration = 2f; // Kuinka kauan äänen alennus kestää
    private AudioSource audioSource; // Tämä GameObjectin AudioSource

    private void Start()
    {
        // Hanki AudioSource-komponentti
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Lisää AudioSource, jos sitä ei ole
        }
    }

    private void Update()
    {
        /*
        Debug.Log("Testaa pitääkö äänet laittaa pois!");
        // Tarkista, soittaako doorController bigDoorAlarmSound
        if (doorController != null && IsBigDoorAlarmPlaying())
        {
            Debug.Log("Laita äänet pois!");
            // Muuta äänenvoimakkuus hitaasti nollaksi
            StartCoroutine(FadeOutAudio());
        }
        */
    }

    private bool IsBigDoorAlarmPlaying()
    {
        // Tarkista, soittaako DoorController bigDoorAlarmSound
        return doorController != null &&
               doorController.audioSource.isPlaying &&
               doorController.audioSource.clip == doorController.bigDoorAlarmSound;
    }

    public System.Collections.IEnumerator FadeOutAudio()
    {
        float startVolume = audioSource.volume; // Tallenna aloitusvoimakkuus
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            Debug.Log("Fading out time, variable 'time': " + time);
            audioSource.volume = Mathf.Lerp(startVolume, 0f, time / fadeDuration); // Muuta äänenvoimakkuus
            yield return null; // Odota seuraavaa kehystä
        }

        audioSource.volume = 0f; // Varmista, että äänenvoimakkuus on nolla
    }
}
