using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFloor : MonoBehaviour
{
   public Transform respawnPoint;
   private void OnTriggerEnter (Collider other)
   {
      if (other.CompareTag("Player"))
      {
         other.GetComponent<PlayerRespawn>().RespawnAt(respawnPoint);
      }
   }
}

