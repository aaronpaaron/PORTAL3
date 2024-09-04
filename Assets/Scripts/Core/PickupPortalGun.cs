using UnityEngine;

public class PickupPortalGun : MonoBehaviour
{
    public GameObject player; // Pelaaja-objekti
    public Transform gunHoldPosition; // Paikka, johon ase kiinnittyy pelaajan kädessä

    private bool isPickedUp = false;

    void Start()
    {
        // Varmista, että ase näkyy alussa
        gameObject.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        // Tarkista, onko pelaaja törmännyt aseeseen
        if (other.gameObject == player && !isPickedUp)
        {
            Pickup();
        }
    }

    void Pickup()
    {
        isPickedUp = true;
        Debug.Log("Picked up Portal Gun");

        // Kytke ase pois näkyvistä sen alkuperäisessä sijainnissa
        gameObject.SetActive(false);

        // Kiinnitä ase pelaajan käteen
        transform.SetParent(gunHoldPosition);
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        // Kytke ase takaisin näkyviin pelaajan kädessä
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (isPickedUp == true)
        {
            transform.position = gunHoldPosition.position;
            transform.rotation = gunHoldPosition.rotation;

            // Kytke ase takaisin näkyviin pelaajan kädessä
            gameObject.SetActive(true);
        }
    }
}
