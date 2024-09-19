using UnityEngine;

public class ImageAnimator : MonoBehaviour
{
    public string[] imageNames = new string[8]; // Kuvien nimet ilman tiedostopäätettä, esim. "image1", "image2", jne.
    public float frameRate = 10f; // Kuinka monta kuvaa näytetään sekunnissa

    private Texture2D[] images; // Taulukko kuville
    private int currentFrame = 0; // Nykyinen kuva (frame)
    private float timer = 0f; // Ajastin framenvaihdolle
    private Renderer quadRenderer; // Renderer Quad-objektin materiaalin asettamista varten

    void Start()
    {
        // Lataa kuvat Resources-kansiosta
        images = new Texture2D[imageNames.Length];
        for (int i = 0; i < imageNames.Length; i++)
        {
            images[i] = Resources.Load<Texture2D>(imageNames[i]);
            if (images[i] == null)
            {
                Debug.LogError("Kuvaa ei löytynyt: " + imageNames[i]);
            }
        }

        // Haetaan Quad-objektin Renderer
        quadRenderer = GetComponent<Renderer>();

        // Asetetaan ensimmäinen kuva materiaalin tekstuuriksi
        if (quadRenderer != null && images.Length > 0)
        {
            quadRenderer.material.mainTexture = images[0];
        }
    }

    void Update()
    {
        if (images == null || images.Length == 0)
        {
            return;
        }

        // Päivitetään ajastin
        timer += Time.deltaTime;

        // Vaihdetaan kuva kun ajastin ylittää framenopeuden
        if (timer >= 1f / frameRate)
        {
            // Nollataan ajastin
            timer = 0f;

            // Siirrytään seuraavaan kuvaan
            currentFrame = (currentFrame + 1) % images.Length;

            // Päivitetään Quad-objektin tekstuuri
            if (quadRenderer != null)
            {
                quadRenderer.material.mainTexture = images[currentFrame];
            }
        }
    }
}
