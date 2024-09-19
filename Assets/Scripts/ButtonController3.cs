using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController3 : MonoBehaviour
{
    public DoorController doorA;
    public DoorController doorB;
    private bool isPressed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube") && ! isPressed)
        {
            isPressed = true;
            doorA.OpenDoor();
            doorB.OpenDoor();
        }
    }
    
}
