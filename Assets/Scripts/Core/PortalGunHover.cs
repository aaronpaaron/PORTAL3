using UnityEngine;

public class PortalGunHover : MonoBehaviour
{
    public float hoverHeight = 0.5f; // Leijumiskorkeus
    public float hoverSpeed = 2f; // Leijumisnopeus

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Tee pieni leijumisliike ylös ja alas
        float newY = Mathf.Sin(Time.time * hoverSpeed) * hoverHeight + startPosition.y;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
