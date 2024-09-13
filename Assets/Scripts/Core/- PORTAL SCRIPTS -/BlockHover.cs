using UnityEngine;

public class BlockHover : MonoBehaviour
{
    public float hoverHeight = 2.0f; // Kuinka korkealle palikka nousee
    public float followSpeed = 5.0f; // Kuinka nopeasti palikka seuraa kameraa
    public float smoothStopDuration = 0.5f; // Kuinka kauan palikan liikettä hidastetaan pysähtyessä
    public float hoverForce = 10f; // Voima, jolla palikka nostetaan
    public float hoverDamping = 0.95f; // Vaimentaa liikettä hoveroinnin aikana

    private Camera mainCamera;
    private Rigidbody rb;
    private bool isHovering = false;

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
                StopHover();
            }
            else
            {
                StartHover();
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
}