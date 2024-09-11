using UnityEngine;

public class PortalGun : MonoBehaviour
{
    public GameObject portalA; // Viittaa Portaali A:han
    public GameObject portalB; // Viittaa Portaali B:hen

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
        if (Input.GetMouseButtonDown(0))
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

            // Aseta portaalin sijainti
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

    Quaternion CalculatePortalRotation(Vector3 hitNormal)
    {
        // Laske portaalin y-akseli ylöspäin suhteessa osumapintaan
        Vector3 up = Vector3.up;
        // Käytetään cameraa määrittämään, mihin suuntaan portaalin etupään pitäisi osoittaa
        Vector3 forward = Camera.main.transform.forward;

        // Laske rotaatio niin, että portaalin y-akseli on ylöspäin
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hitNormal);

        // Kierrä portaalin etupää kameran suuntaan
        Vector3 desiredForward = rotation * forward;
        rotation = Quaternion.LookRotation(desiredForward, up);

        return rotation;
    }
}
