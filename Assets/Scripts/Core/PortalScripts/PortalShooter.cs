using UnityEngine;

public class PortalShooter : MonoBehaviour
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
            Quaternion hitRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            if (isPortalAActive)
            {
                // Aseta Portaali B näkyviin ja aseta sen sijainti raycast-kohtaan
                portalB.SetActive(true);
                portalB.transform.position = hitPoint;
                portalB.transform.rotation = hitRotation;
            }
            else
            {
                // Aseta Portaali A näkyviin ja aseta sen sijainti raycast-kohtaan
                portalA.SetActive(true);
                portalA.transform.position = hitPoint;
                portalA.transform.rotation = hitRotation;
            }

            // Vaihda aktiivista portaalitilaa
            isPortalAActive = !isPortalAActive;
        }
    }
}
