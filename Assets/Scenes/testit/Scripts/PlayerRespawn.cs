using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public float respawnDelay = 1f;

    CharacterController controller;
    
    void Start ()
    {
        controller = GetComponent<CharacterController> ();
    }
    public void RespawnAt(Transform respawnPoint)
    {
        StartCoroutine(RespawnAfterDelay(respawnPoint));
    }
    private IEnumerator RespawnAfterDelay(Transform respawnPoint)
    {
        
        transform.position = respawnPoint.position;
        controller.enabled = false;
        yield return new WaitForSeconds (respawnDelay);
        controller.enabled = true;
    }
}