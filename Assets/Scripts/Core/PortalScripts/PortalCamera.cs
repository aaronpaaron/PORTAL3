using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    public Transform playerCamera;      // Pelaajan kamera
    public Transform portal;            // Tämä portaalin objekti
    public Transform otherPortal;       // Toisen portaalin objekti

    private void Update()
    {
        // Pelaajan sijainti suhteessa toiseen portaaliin
        Vector3 playerOffsetFromPortal = playerCamera.position - otherPortal.position;

        // Päivitetään portaalikameran sijainti
        transform.position = portal.position + playerOffsetFromPortal;

        // Käännetään kamera portaalien rotaatioiden perusteella
        // Lasketaan suunta, jonne pelaajan kamera osoittaa suhteessa toiseen portaaliin
        
        Quaternion portalRotationalDifference = Quaternion.Inverse(otherPortal.rotation) * portal.rotation;
        Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;


        // Käännetään kamera uuden suunnan mukaisesti
        transform.rotation = Quaternion.Inverse(Quaternion.LookRotation(newCameraDirection, Vector3.up));

        // Tee kameran zoomausefekti pelaajan etäisyyden perusteella
        float distanceFromPortal = Vector3.Distance(playerCamera.position, portal.position);

        // Liikutetaan kameraa lähemmäs portaalia, kun pelaaja lähestyy
        transform.position = Vector3.Lerp(transform.position, portal.position, 1f - Mathf.Clamp01(distanceFromPortal / 10f));
    }
}
