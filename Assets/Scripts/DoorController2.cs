using UnityEngine;

public class DoorController2 : MonoBehaviour
{
    public Vector3 openPosition; // Ovea siirretään tähän paikkaan, kun se avataan
    public Vector3 closedPosition; // Ovea siirretään tähän paikkaan, kun se suljetaan
    public float speed = 2f; // Kuinka nopeasti ovi liikkuu
    
    private bool isOpen = false; // Tieto siitä, onko ovi auki vai ei

    private int buttonsPressed = 0; // Kuinka monta nappia on painettu

    private void Update()
    {
        // Jos kaikki napit on painettu (esim. 2), avaa ovi, muuten sulje ovi
        if (isOpen)
        {
            transform.position = Vector3.Lerp(transform.position, openPosition, Time.deltaTime * speed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, closedPosition, Time.deltaTime * speed);
        }
    }

    public void PressButton()
    {
        buttonsPressed++;

        // Jos kaksi nappia on painettuna, avaa ovi
        if (buttonsPressed == 2)
        {
            isOpen = true;
        }
    }

    public void ReleaseButton()
    {
        buttonsPressed--;

        // Jos yhtäkään nappia ei ole enää painettuna, sulje ovi
        if (buttonsPressed < 2)
        {
            isOpen = false;
        }
    }
}