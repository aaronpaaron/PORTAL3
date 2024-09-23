using UnityEngine;

public class BlockRespawn : MonoBehaviour
{
    public void Respawn(Vector3 position)
    {
        transform.position = position; // Siirrä objekti uuteen sijaintiin
    }
}
