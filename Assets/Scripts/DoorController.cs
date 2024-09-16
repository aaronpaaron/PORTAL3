using System;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Vector3 openPosition; // Ovea siirretään tähän paikkaan, kun se avataan
    public Vector3 closedPosition; // Ovea siirretään tähän paikkaan, kun se suljetaan
    public float speed = 2f; // Kuinka nopeasti ovi liikkuu

    private bool isOpen = false; // Tieto siitä, onko ovi auki vai ei

    private void Update()
    {
        // Jos ovi on auki, siirrä sitä openPositioniin, muuten closedPositioniin
        if (isOpen)
        {
            transform.position = Vector3.Lerp(transform.position, openPosition, Time.deltaTime * speed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, closedPosition, Time.deltaTime * speed);
        }
    }

    public void OpenDoor()
    {
        isOpen = true; // Avaa ovi
    }

    public void CloseDoor()
    {
        isOpen = false; // Sulje ovi
    }

    internal void PressButton()
    {
        throw new NotImplementedException();
    }
}