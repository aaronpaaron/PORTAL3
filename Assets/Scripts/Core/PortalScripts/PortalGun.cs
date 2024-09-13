using UnityEngine;
using System.Collections;

public class PortalGun : MonoBehaviour
{
    public GameObject portalA; // Viittaa Portaali A:han
    public GameObject portalB; // Viittaa Portaali B:hen

    public float minimumPortalDistance = 3.0f; // Minimietäisyys portaalien välillä
    public float scalingDuration = 0.5f; // Kuinka kauan skaalaus kestää

    private bool isPortalAActive = true; // Tarkistaa, kumpi portaali on aktiivinen

    void Start()
    {
        // Piilota molemmat portaalit aluksi
        portalA.SetActive(false);
        portalB.SetActive(false);
    }

    void Update()
    {
        // Tarkistaa, onko hiiren vasen painike painettu
        if (Input.GetMouseButtonDown(0) && PickupPortalGun.equippedWeapon == PickupPortalGun.Weapon.PortalGun)
        {
            HandlePortalSwitch();
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

            // Jos toista portaalia ei ole vielä luotu, luo se automaattisesti ensimmäisen jälkeen
            if (!portalA.activeInHierarchy || !portalB.activeInHierarchy)
            {
                // Aseta toinen portaali näkyväksi jonnekin lähelle ensimmäistä portaalia
                Vector3 offset = new Vector3(2, 0, 0); // Esimerkki siirrosta
                if (isPortalAActive && !portalA.activeInHierarchy)
                {
                    portalA.SetActive(true);
                    portalA.transform.position = hitPoint + offset;
                    portalA.transform.rotation = CalculatePortalRotation(hitNormal);
                    StartCoroutine(ScalePortal(portalA));
                }
                else if (!isPortalAActive && !portalB.activeInHierarchy)
                {
                    portalB.SetActive(true);
                    portalB.transform.position = hitPoint + offset;
                    portalB.transform.rotation = CalculatePortalRotation(hitNormal);
                    StartCoroutine(ScalePortal(portalB));
                }
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
