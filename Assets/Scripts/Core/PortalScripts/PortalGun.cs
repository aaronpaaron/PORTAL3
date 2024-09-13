using UnityEngine;
using System.Collections;

public class PortalGun : MonoBehaviour
{
    public GameObject portalA; // Viittaa Portaali A:han
    public GameObject portalB; // Viittaa Portaali B:hen

    public float minimumPortalDistance = 3.0f; // Minimietäisyys portaalien välillä
    public float scalingDuration = 0.5f; // Kuinka kauan skaalaus kestää
    public float fireDelay = 1.0f; // Ampumisviive portaalien luomiseen

    private bool isPortalAActive = true; // Tarkistaa, kumpi portaali on aktiivinen
    private bool hasFirstPortalPlaced = false; // Tarkistaa, onko ensimmäinen portaali asetettu
    private float lastFireTime = 0f; // Aika, jolloin viimeisin ammunta tapahtui

    void Start()
    {
        // Piilota molemmat portaalit aluksi
        portalA.SetActive(false);
        portalB.SetActive(false);
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
            Vector3 hitPoint = hit.point;
            Vector3 hitNormal = hit.normal;

            // Tarkista, onko uusi portaali liian lähellä toista
            if (IsTooCloseToOtherPortal(hitPoint))
            {
                Debug.Log("Portaalia ei voi asettaa liian lähelle toista portaalia.");
                return; // Estetään uuden portaalin sijoittaminen
            }

            // Hanki aktiivinen portaali
            GameObject activePortal = isPortalAActive ? portalB : portalA;

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
                StartCoroutine(ScalePortal(otherPortal)); // Skaalaa myös toinen portaali esiin
            }

            // Vaihda aktiivista portaalitilaa
            isPortalAActive = !isPortalAActive;
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
}
