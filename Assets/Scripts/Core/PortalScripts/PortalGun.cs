using UnityEngine;
using System.Collections;

public class PortalGun : MonoBehaviour
{
    public GameObject portalA; // Viittaa Portaali A:han
    public GameObject portalB; // Viittaa Portaali B:hen

    public float minimumPortalDistance = 3.0f; // Minimietäisyys portaalien välillä
    public float scalingDuration = 100.0f; // Kuinka kauan skaalaus kestää
    public float fireDelay = 1.0f; // Ampumisviive portaalien luomiseen

    private bool isPortalAActive = true; // Tarkistaa, kumpi portaali on aktiivinen
    private bool hasFirstPortalPlaced = false; // Tarkistaa, onko ensimmäinen portaali asetettu
    private float lastFireTime = 0f; // Aika, jolloin viimeisin ammunta tapahtui

    public Animator animator;

    // Äänet portaalin asettamiseen ja epäonnistumiseen
    public AudioClip portalSpawnSFX; // Ääni portaali spawnille
    public AudioClip portalFailSFX; // Ääni, kun portaalia ei voi asettaa
    public AudioClip portalLoopSFX; // Looppaava ääni portaalille

    private AudioSource audioSource; // AudioSource-viittaus

    // Äänilähteet jokaiselle portaalille
    private AudioSource portalASource;
    private AudioSource portalBSource;

    void Start()
    {
        // Piilota molemmat portaalit aluksi
        portalA.SetActive(false);
        portalB.SetActive(false);

        // Hae tai lisää AudioSource-komponentti tähän GameObjectiin
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Lisää AudioSource komponentit portaaleille
        portalASource = portalA.GetComponent<AudioSource>();
        portalASource.clip = portalLoopSFX;
        portalASource.loop = true; // Aseta loopiksi

        portalBSource = portalB.GetComponent<AudioSource>();
        portalBSource.clip = portalLoopSFX;
        portalBSource.loop = true; // Aseta loopiksi
    }

    void Update()
    {
        // Tarkistaa, onko hiiren vasen painike painettu ja onko kulunut tarpeeksi aikaa edellisestä ammunnasta
        if (Input.GetMouseButtonDown(0) && PickupPortalGun.equippedWeapon == PickupPortalGun.Weapon.PortalGun)
        {
            if (Time.time - lastFireTime >= fireDelay)
            {
                lastFireTime = Time.time; // Päivitä viimeisin ammunta-aika
                HandlePortalSwitch();
            }
        }
    }

    void HandlePortalSwitch()
    {
        // Raycast rayn luominen kamerasta pelaajan katselusuuntaan
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Tarkista, onko osuma ShootableWall-tägillä merkityllä objektilla
            if (hit.collider.CompareTag("ShootableWall"))
            {
                Vector3 hitPoint = hit.point;
                Vector3 hitNormal = hit.normal;

                // Tarkista, onko uusi portaali liian lähellä toista
                if (IsTooCloseToOtherPortal(hitPoint))
                {
                    Debug.Log("Portaalia ei voi asettaa liian lähelle toista portaalia.");
                    PlayPortalFailSound(); // Soitetaan epäonnistumisääni
                    return; // Estetään uuden portaalin sijoittaminen
                }

                // Hanki aktiivinen portaali
                GameObject activePortal = isPortalAActive ? portalB : portalA;

                animator = GameObject.FindWithTag("PortalGun").GetComponent<Animator>();
                animator.Rebind();
                animator.SetTrigger("Shoot");

                if (activePortal.activeInHierarchy)
                {
                    // Skaalaa portaalin takaisin pieneksi ennen sen siirtämistä
                    StartCoroutine(ShrinkAndMovePortal(activePortal, hitPoint, hitNormal));
                }
                else
                {
                    // Aseta portaalin sijainti ja rotaatio
                    activePortal.SetActive(true);
                    activePortal.transform.position = hitPoint;
                    activePortal.transform.rotation = CalculatePortalRotation(hitNormal);

                    // Soita ääni, kun portaali spawnaa
                    PlayPortalSpawnSound();
                    StartLoopingPortalSound(activePortal); // Aloita looppaava ääni

                    // Skaalaa portaali nopeasti suureksi
                    StartCoroutine(ScalePortal(activePortal));
                }

                // Jos tämä on ensimmäinen portaali, asetetaan toinen portaali editorin sijaintiin
                if (!hasFirstPortalPlaced)
                {
                    hasFirstPortalPlaced = true;

                    // Aseta toinen portaali näkyviin editorin sijaintiin
                    GameObject otherPortal = isPortalAActive ? portalA : portalB;
                    otherPortal.SetActive(true);

                    // Soita ääni toisen portaalin spawnautumiselle
                    PlayPortalSpawnSound();
                    StartLoopingPortalSound(otherPortal); // Aloita looppaava ääni toiselle portaalille

                    StartCoroutine(ScalePortal(otherPortal)); // Skaalaa myös toinen portaali esiin
                }

                // Vaihda aktiivista portaalitilaa
                isPortalAActive = !isPortalAActive;
            }
            else
            {
                Debug.Log("Ei voi asettaa portaalin siihen objektiin. Objekti ei ole ShootableWall.");
                PlayPortalFailSound(); // Soita epäonnistumisääni
            }
        }
    }

    // Funktio äänen toistamiselle, kun portaali spawnaa
    void PlayPortalSpawnSound()
    {
        if (portalSpawnSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(portalSpawnSFX); // Soita portaali spawn -ääni kerran
        }
    }

    // Funktio epäonnistuneen äänen toistamiselle
    void PlayPortalFailSound()
    {
        audioSource.PlayOneShot(portalFailSFX); // Soita epäonnistumisääni kerran
    }

    void StartLoopingPortalSound(GameObject portal)
    {
        if (portal == portalA && portalASource != null)
        {
            portalASource.Play(); // Aloita looppaava ääni Portaali A:sta
        }
        else if (portal == portalB && portalBSource != null)
        {
            portalBSource.Play(); // Aloita looppaava ääni Portaali B:sta
        }
    }

    void StopLoopingPortalSound(GameObject portal)
    {
        if (portal == portalA && portalASource != null)
        {
            portalASource.Stop(); // Pysäytä looppaava ääni Portaali A:sta
        }
        else if (portal == portalB && portalBSource != null)
        {
            portalBSource.Stop(); // Pysäytä looppaava ääni Portaali B:sta
        }
    }

    IEnumerator ShrinkAndMovePortal(GameObject portal, Vector3 newPosition, Vector3 newNormal)
    {
        // Skaalaa portaali takaisin pieneksi
        Vector3 initialScale = portal.transform.localScale;
        Vector3 targetScale = Vector3.zero;
        float elapsedTime = 0f;

        while (elapsedTime < scalingDuration)
        {
            portal.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / scalingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        portal.transform.localScale = targetScale;

        // Aseta uusi sijainti ja rotaatio
        portal.transform.position = newPosition;
        portal.transform.rotation = CalculatePortalRotation(newNormal);

        // Skaalaa portaali nopeasti suureksi
        StartCoroutine(ScalePortal(portal));

        // Soita ääni aina, kun portaali siirretään
        PlayPortalSpawnSound();
    }

    IEnumerator ScalePortal(GameObject portal)
    {
        Vector3 initialScale = Vector3.zero; // Portaali on aluksi pienikokoinen
        Vector3 targetScale = Vector3.one; // Portaali kasvaa normaalikokoiseksi

        portal.transform.localScale = initialScale;

        float elapsedTime = 0f;

        while (elapsedTime < scalingDuration)
        {
            portal.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / scalingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        portal.transform.localScale = targetScale; // Varmista, että portaalin skaala on tarkka
    }

    bool IsTooCloseToOtherPortal(Vector3 hitPoint)
    {
        // Tarkista, jos Portaali A on aktiivinen ja liian lähellä uutta portaalia
        if (portalA.activeInHierarchy && Vector3.Distance(hitPoint, portalA.transform.position) < minimumPortalDistance)
        {
            return true;
        }

        // Tarkista, jos Portaali B on aktiivinen ja liian lähellä uutta portaalia
        if (portalB.activeInHierarchy && Vector3.Distance(hitPoint, portalB.transform.position) < minimumPortalDistance)
        {
            return true;
        }

        // Portaali ei ole liian lähellä
        return false;
    }

    Quaternion CalculatePortalRotation(Vector3 hitNormal)
    {
        // Tarkista, onko portaali kiinnittymässä pystysuoraan pintaan (seinä)
        if (Mathf.Abs(hitNormal.y) < 0.1f)
        {
            // Pystysuora pinta (seinä) - portaali asennetaan seinälle
            return Quaternion.LookRotation(-hitNormal, Vector3.up);
        }
        else
        {
            // Vaakasuora pinta (maa/lattia) - portaali asennetaan ylös osoittaen
            return Quaternion.LookRotation(Vector3.forward, hitNormal);
        }
    }

    // Lisää tämä funktio kutsuaksesi looppaavan äänen pysäyttämistä, kun portaali poistuu
    public void DeactivatePortal(GameObject portal)
    {
        if (portal == portalA || portal == portalB)
        {
            StopLoopingPortalSound(portal); // Pysäytä looppaava ääni
            portal.SetActive(false); // Deaktivoidaan portaali
        }
    }
}
