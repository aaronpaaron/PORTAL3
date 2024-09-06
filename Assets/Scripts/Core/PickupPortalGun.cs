using UnityEngine;

public class PickupPortalGun : MonoBehaviour
{
    public GameObject player; // Pelaaja-objekti
    public Transform gunHoldPosition; // Paikka, johon ase kiinnittyy pelaajan kädessä

    public bool isPickedUp = false;

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

        // Aseta ase gunHoldPositionin kohdalle ja suunnalle
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // Kytke ase takaisin näkyviin pelaajan kädessä
        gameObject.SetActive(true);
    }

    void LateUpdate() // Käytetään LateUpdatea varmistamaan, että kamera on jo liikkunut ennen kuin ase asetetaan
    {
        if (isPickedUp)
        {
            // Päivitä aseen sijainti ja rotaatio suhteessa pelaajan kameraan
            transform.position = gunHoldPosition.position;
            transform.rotation = gunHoldPosition.rotation;
        }
    }
}
