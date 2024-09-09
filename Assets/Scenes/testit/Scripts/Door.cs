using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator anim;
    public Transform palyer;
    public Transform door;
 

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(palyer.position, door.position);

        if (distance <= 25)
        {
            anim.SetBool("Near", true);
        }
        else
        {
            anim.SetBool("Near", false);
        }
    }
}
