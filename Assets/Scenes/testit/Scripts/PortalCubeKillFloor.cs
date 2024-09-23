using UnityEngine;

public class PortalCubeKillFloor : MonoBehaviour
{
    public Transform respawnPoint; // Määrittele tyhjälle objektin transformi

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            BlockRespawn blockRespawn = other.GetComponent<BlockRespawn>();
            if (blockRespawn != null && respawnPoint != null)
            {
                blockRespawn.Respawn(respawnPoint.position); // Käytä tyhjää objektia
            }
        }
    }
}
