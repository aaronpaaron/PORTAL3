using UnityEngine;

public class PortalGun : MonoBehaviour
{
    public GameObject portalA; // Viittaa Portaali A:han
    public GameObject portalB; // Viittaa Portaali B:hen

    public float minimumPortalDistance = 3.0f; // Minimietäisyys portaalien välillä

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

            // Aseta portaalin sijainti ja rotaatio
            if (isPortalAActive)
            {
                portalB.SetActive(true);
                portalB.transform.position = hitPoint;
                portalB.transform.rotation = CalculatePortalRotation(hitNormal);
            }
            else
            {
                portalA.SetActive(true);
                portalA.transform.position = hitPoint;
                portalA.transform.rotation = CalculatePortalRotation(hitNormal);
            }

            // Vaihda aktiivista portaalitilaa
            isPortalAActive = !isPortalAActive;
        }
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