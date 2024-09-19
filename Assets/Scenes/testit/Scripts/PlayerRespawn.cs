using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public float respawnDelay = 1f;
    public void RespawnAt(Transform respawnPoint)
    {
        StartCoroutine(RespawnAfterDelay(respawnPoint));
    }
    private IEnumerator RespawnAfterDelay(Transform respawnPoint)
    {
        yield return new WaitForSeconds (respawnDelay);
        transform.position = respawnPoint.position;
    }
}