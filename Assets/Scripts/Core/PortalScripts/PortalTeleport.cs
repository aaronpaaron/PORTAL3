using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleport : MonoBehaviour
{
    [SerializeField] private Transform otherPortal; // Toinen portaali
    [SerializeField] private Transform player; // Pelaajan Transform
    [SerializeField] private CharacterController playerController; // Pelaajan CharacterController
    [SerializeField] private float teleportCooldown = 1.0f; // Viive teleportin välillä

    private bool canTeleport = true; // Määrittää, voiko pelaaja teleportata

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canTeleport) // Varmistaa, että vain pelaaja teleportataan ja viive on ohi
        {
            StartCoroutine(TeleportPlayer());
        }
    }

    private IEnumerator TeleportPlayer()
    {
        canTeleport = false; // Estetään pelaajaa teleporttaamasta heti uudelleen

        // Sijoitetaan pelaaja toisen portaalin sisälle
        Vector3 newPosition = otherPortal.transform.position;
        // playerController.enabled = false; // Deaktivoidaan CharacterController, jotta ei tapahdu 'törmäystä' teleportin aikana
        
        // Vector3 moveDelta = newPosition;
        playerController.Move(newPosition);

        Debug.Log($"Pelaajan positio: {player.transform.position}, portaali: {otherPortal.transform.position}, new position: {newPosition}");

        // Odotetaan ennen kuin pelaaja voi teleportata uudelleen
        yield return new WaitForSeconds(teleportCooldown);
        canTeleport = true; // Pelaaja voi teleportata uudelleen viiveen jälkeen
    }
}
