using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BreakableTilemap : MonoBehaviour
{
    public Tilemap tilemap;  
    public TileBase emptyTile;  
    public int hitsToBreak = 3;  

    public TileBase[] treeTiles;  
    public TileBase[] rockTiles;  

    public GameObject treeBreakParticlesPrefab;  
    public GameObject rockBreakParticlesPrefab;  

    // 🔹 Audio system added back in properly
    public AudioClip treeHitSound;  
    public AudioClip treeBreakSound;  
    public AudioClip rockHitSound;  
    public AudioClip rockBreakSound;  

    private Dictionary<Vector3Int, int> tileHealth = new Dictionary<Vector3Int, int>();  
    private InventoryManager inventoryManager;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();  
        inventoryManager = FindObjectOfType<InventoryManager>();  
    }

    public void ProcessTileClick(Vector3Int tilePosition)
    {
        if (!tileHealth.ContainsKey(tilePosition))
        {
            tileHealth[tilePosition] = hitsToBreak;
        }

        tileHealth[tilePosition]--;

        TileBase tile = tilemap.GetTile(tilePosition);
        if (tile == null)
        {
            Debug.LogError("❌ ERROR: No tile found at position: " + tilePosition);
            return;
        }

        Debug.Log("✅ Clicked tile: " + tile.name + " at position " + tilePosition);

        // 🎵 Play hit sound
        if (IsTreeTile(tile) && treeHitSound != null)
        {
            AudioSource.PlayClipAtPoint(treeHitSound, Camera.main.transform.position);
        }
        else if (IsRockTile(tile) && rockHitSound != null)
        {
            AudioSource.PlayClipAtPoint(rockHitSound, Camera.main.transform.position);
        }

        if (tileHealth[tilePosition] <= 0)
        {
            DestroyTile(tilePosition);
        }
    }

    void DestroyTile(Vector3Int tilePosition)
    {
        TileBase tile = tilemap.GetTile(tilePosition);

        if (tile == null)
        {
            Debug.LogError("❌ ERROR: Tried to destroy a tile, but no tile found at: " + tilePosition);
            return;
        }

        if (inventoryManager != null)
        {
            if (IsTreeTile(tile))
            {
                inventoryManager.AddWood(5);
                
                // 🎵 Play break sound before destroying tile
                if (treeBreakSound != null)
                {
                    AudioSource.PlayClipAtPoint(treeBreakSound, Camera.main.transform.position);
                }

                SpawnParticles(treeBreakParticlesPrefab, tilePosition);
            }
            else if (IsRockTile(tile))
            {
                inventoryManager.AddStone(5);
                
                // 🎵 Play break sound before destroying tile
                if (rockBreakSound != null)
                {
                    AudioSource.PlayClipAtPoint(rockBreakSound, Camera.main.transform.position);
                }

                SpawnParticles(rockBreakParticlesPrefab, tilePosition);
            }
        }

        Debug.Log("✅ Removing tile at: " + tilePosition);
        tilemap.SetTile(tilePosition, null);

        if (tilemap.GetTile(tilePosition) == null)
        {
            Debug.Log("✅ Tile successfully removed at: " + tilePosition);
        }
        else
        {
            Debug.LogError("ERROR Tile was NOT removed at: " + tilePosition);
        }

        tileHealth.Remove(tilePosition);
    }

    void SpawnParticles(GameObject particlePrefab, Vector3Int tilePosition)
{
    if (particlePrefab == null)
    {
        Debug.LogError("ERROR Particle prefab is NULL! Make sure it's assigned in the Inspector.");
        return;
    }

    // Check if the assigned prefab is actually a GameObject
    if (!(particlePrefab is GameObject))
    {
        Debug.LogError("ERROR: Assigned prefab is NOT a GameObject! Check your prefab assignment in the Inspector.");
        return;
    }

    // Determine world position for the particle effect (slightly in front of tilemap)
    Vector3 worldPosition = tilemap.GetCellCenterWorld(tilePosition);
    worldPosition.z = -0.1f;  // Ensure particles render above the tilemap

    Debug.Log("Instantiating particles at " + worldPosition);

    // Instantiate the particle prefab GameObject
    GameObject particlesInstance = Instantiate(particlePrefab, worldPosition, Quaternion.identity);

    // Find a ParticleSystem on the spawned object and play it
    ParticleSystem ps = particlesInstance.GetComponent<ParticleSystem>();
    if (ps != null)
    {
        ps.Play();
    }
    else
    {
        Debug.LogError("ERROR No ParticleSystem found on prefab: " + particlePrefab.name);
    }

    // Destroy the particle instance after 1 second to clean up
    Destroy(particlesInstance, 1f);
}



    bool IsTreeTile(TileBase tile)
    {
        foreach (TileBase treeTile in treeTiles)
        {
            if (tile == treeTile)
                return true;
        }
        return false;
    }

    bool IsRockTile(TileBase tile)
    {
        foreach (TileBase rockTile in rockTiles)
        {
            if (tile == rockTile)
                return true;
        }
        return false;
    }
}
