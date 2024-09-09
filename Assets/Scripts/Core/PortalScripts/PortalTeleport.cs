using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleport : MonoBehaviour
{
    [SerializeField] private Transform otherPortal; // Mihin pelaaja teleportataan
    [SerializeField] private Transform playerCamera; // Pelaajan kamera
    [SerializeField] private CharacterController characterController; // Pelaajan CharacterController
    [SerializeField] private float teleportDelay = 1f; // Aika, jonka pelaaja on "estetty" teleporttaamasta uudelleen

    private bool canTeleport = true; // Tarkistaa, onko pelaaja valmis teleporttaamaan

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canTeleport)
        {
            StartCoroutine(TeleportPlayer(other.transform));
        }
    }

    private IEnumerator TeleportPlayer(Transform playerTransform)
    {
        // Estetään uusi teleporttaus kunnes viive on kulunut
        canTeleport = false;

        // Tallenna pelaajan nykyinen sijainti ja suunta
        Vector3 playerPosition = playerTransform.position;
        Quaternion playerRotation = playerTransform.rotation;

        // Lasketaan sijainti toisen portaalin suhteessa
        Vector3 offset = playerPosition - transform.position;
        Vector3 newPosition = otherPortal.position + offset;

        // Käytä portaalin suuntaa pelaajan kameran suunnan mukaisesti
        Quaternion portalRotationDifference = Quaternion.Inverse(transform.rotation) * playerRotation;
        Quaternion newRotation = otherPortal.rotation * portalRotationDifference;

        // Poista CharacterControllerin liike ja aseta pelaaja suoraan uuteen sijaintiin
        characterController.enabled = false;
        playerTransform.position = newPosition;
        playerTransform.rotation = newRotation;
        characterController.enabled = true;

        // Odota teleportin viiveen ajan ennen kuin teleporttaus on mahdollista uudelleen
        yield return new WaitForSeconds(teleportDelay);

        // Salli uusi teleporttaus
        canTeleport = true;
    }
}
