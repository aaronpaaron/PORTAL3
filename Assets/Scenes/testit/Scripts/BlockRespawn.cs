using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRespawn : MonoBehaviour
{
    public float respawnDelay = 1f;
    
    void Start ()
    {

    }
    public void Respawn(Transform respawnPoint)
    {
        StartCoroutine(RespawnAfterDelay(respawnPoint));
    }
    private IEnumerator RespawnAfterDelay(Transform respawnPoint)
    {
        
        transform.position = respawnPoint.position;

        yield return new WaitForSeconds (respawnDelay);

    }
}