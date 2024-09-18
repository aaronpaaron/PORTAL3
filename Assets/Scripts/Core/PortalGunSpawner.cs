using UnityEngine;

public class PortalGunSpawner : MonoBehaviour
{
    public GameObject itemToSpawn; // Objektin prefab, joka spawnaa
    public Transform spawnPosition; // Kohde-Transform, johon objekti ilmestyy

    private bool hasSpawned = false; // Tarkistaa, onko ase jo spawnattu

    void Update()
    {
        // Tarkistetaan, onko pelaaja painanut Q-näppäintä ja onko ase vielä spawnattu
        if (Input.GetKeyDown(KeyCode.Q) && !hasSpawned)
        {
            SpawnItem();
        }
    }

    void SpawnItem()
    {
        if (itemToSpawn != null && spawnPosition != null)
        {
            // Instansioidaan objekti spawnPositioniin
            GameObject spawnedGun = Instantiate(itemToSpawn, spawnPosition.position, spawnPosition.rotation);
            spawnedGun.name = itemToSpawn.name;
            hasSpawned = true; // Ase on nyt spawnattu
        }
        else
        {
            Debug.LogWarning("ItemToSpawn tai SpawnPosition ei ole asetettu.");
        }
    }
}
