using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleport : MonoBehaviour
{
    [SerializeField] private Transform otherPortal; // Toinen portaali
    [SerializeField] private Transform player; // Pelaajan Transform
    [SerializeField] private CharacterController playerController; // Pelaajan CharacterController

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Varmistaa, että vain pelaaja teleportataan
        {
            TeleportPlayer();
        }
    }

    private void TeleportPlayer()
    {
        // Lasketaan pelaajan sijainti suhteessa portaaliin
        Vector3 localPlayerPosition = transform.InverseTransformPoint(player.position);

        // Sijoitetaan pelaaja toisen portaalin sisälle
        Vector3 newPosition = otherPortal.TransformPoint(localPlayerPosition);
        playerController.enabled = false; // Deaktivoidaan CharacterController, jotta ei tapahdu 'törmäystä' teleportin aikana
        player.position = newPosition;
        playerController.enabled = true; // Aktivoi CharacterController uudelleen

        // Liikutetaan pelaajaa samankaltaiseen suuntaan kuin ennen teleporttia
        Vector3 localPlayerDirection = transform.InverseTransformDirection(player.forward);
        playerController.transform.forward = otherPortal.TransformDirection(localPlayerDirection);
    }
}
