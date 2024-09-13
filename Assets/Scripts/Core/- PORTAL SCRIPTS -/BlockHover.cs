using UnityEngine;
using System.Collections;

public class BlockHover : MonoBehaviour
{
    public float hoverHeight = 2.0f; // Kuinka korkealle palikka nousee
    public float followSpeed = 5.0f; // Kuinka nopeasti palikka seuraa kameraa
    public float smoothStopDuration = 0.5f; // Kuinka kauan palikan liikettä hidastetaan pysähtyessä
    public float hoverForce = 10f; // Voima, jolla palikka nostetaan
    public float hoverDamping = 0.95f; // Vaimentaa liikettä hoveroinnin aikana
    public float portalForce = 7.5f; // Voima, jolla palikkaa työnnetään pois portaaleista
    public float portalCooldown = 1.0f; // Viive ennen kuin triggerit aktivoituvat uudelleen
    public float raycastDistance = 20f; // Etäisyys, kuinka kaukaa pelaaja voi "katsoa" palikkaa

    private Camera mainCamera;
    private Rigidbody rb;
    private bool isHovering = false;
    private bool canTrigger = true; // Voi aktivoida triggerit aluksi

    void Start()
    {
        mainCamera = Camera.main; // Hanki pääkamera
        rb = GetComponent<Rigidbody>(); // Hanki Rigidbody-komponentti
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Oikea hiirinäppäin
        {
            if (isHovering)
            {
                StopHover(); // Palikan tiputtaminen on aina mahdollista
            }
            else
            {
                if (IsLookingAtBlock()) // Varmista, että pelaaja katsoo palikkaa
                {
                    StartHover();
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (isHovering)
        {
            HoverAndFollowCamera();
        }
    }

    bool IsLookingAtBlock()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); // Ammu ray pelaajan katselusuuntaan
        RaycastHit hit;

        // Tarkista osuuko ray tähän palikkaan ja onko etäisyys tarpeeksi lyhyt
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            if (hit.transform == transform) // Onko osuma tähän palikkaan
            {
                return true;
            }
        }
        return false;
    }

    void StartHover()
    {
        if (!isHovering)
        {
            isHovering = true;

            // Poista gravitaatio hoveroinnin ajaksi
            rb.useGravity = false;

            // Anna nopea nostovoima, jotta palikka saadaan ilmaan
            rb.AddForce(Vector3.up * hoverForce, ForceMode.VelocityChange);
        }
    }

    void StopHover()
    {
        if (isHovering)
        {
            isHovering = false;

            // Palauta gravitaatio, jotta palikka tippuu maahan
            rb.useGravity = true;
        }
    }

    void HoverAndFollowCamera()
    {
        // Kameraa seurataan käyttämällä rigidbodyn MovePositionia
        Vector3 cameraPosition = mainCamera.transform.position;
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0; // Estetään y-akselin liike, jotta palikka pysyy samassa korkeudessa

        // Lasketaan kohdepositio kameran edessä
        Vector3 targetPosition = cameraPosition + cameraForward * 5f; // Säädä etäisyys kamerasta

        // Säädetään palikan y-koordinaatti hoverHeightin mukaan, jotta se nousee sopivasti maasta
        targetPosition.y = Mathf.Max(targetPosition.y, hoverHeight);

        // Lasketaan uusi paikka MovePositionin avulla ja hidastetaan liikettä vaimennuksen avulla
        Vector3 newPosition = Vector3.Lerp(rb.position, targetPosition, followSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        // Vaimennetaan myös y-akselin nopeutta, jotta se ei liikkuisi liikaa
        rb.velocity = new Vector3(rb.velocity.x * hoverDamping, rb.velocity.y * hoverDamping, rb.velocity.z * hoverDamping);
    }

    void OnTriggerEnter(Collider collider)
    {
        // Tarkista, voiko triggerit aktivoitua
        if (canTrigger)
        {
            // Tarkista, onko triggeri Portal-objekti
            if (collider.CompareTag("Portal"))
            {
                StopHover();

                // Lasketaan voima sen mukaan, mistä suunnasta palikka osuu triggeriin
                Vector3 hitNormal = collider.ClosestPointOnBounds(transform.position) - transform.position;
                hitNormal.Normalize();

                // Muutetaan voima triggerin tagin mukaan
                Vector3 forceDirection = (collider.CompareTag("Portal2")) ? -hitNormal : hitNormal;
                rb.AddForce(forceDirection * portalForce, ForceMode.VelocityChange);

                StartCoroutine(PortalCooldown()); // Viiveen asetus
            }

            if (collider.CompareTag("Portal2"))
            {
                StopHover();

                // Lasketaan voima sen mukaan, mistä suunnasta palikka osuu triggeriin
                Vector3 hitNormal = collider.ClosestPointOnBounds(transform.position) - transform.position;
                hitNormal.Normalize();

                // Muutetaan voima triggerin tagin mukaan
                Vector3 forceDirection = (collider.CompareTag("Portal2")) ? -hitNormal : hitNormal;
                rb.AddForce(-forceDirection * portalForce, ForceMode.VelocityChange);
            }
        }
    }

    // Coroutine viiveelle, jotta triggerit aktivoituvat vain tietyn ajan kuluttua
    IEnumerator PortalCooldown()
    {
        canTrigger = false; // Estä triggerit väliaikaisesti
        yield return new WaitForSeconds(portalCooldown); // Viive
        canTrigger = true; // Salli triggerit uudelleen
    }
}
