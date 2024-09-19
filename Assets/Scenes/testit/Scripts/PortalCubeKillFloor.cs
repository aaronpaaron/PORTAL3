using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCubeKillFloor : MonoBehaviour
{
   public Transform respawnPoint;
   private void OnTriggerEnter (Collider other)
   {
      if (other.CompareTag("Cube"))
      {
         other.GetComponent<PlayerRespawn>().RespawnAt(respawnPoint);
      }
   }
}

