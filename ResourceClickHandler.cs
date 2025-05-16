using UnityEngine;

public class ResourceClickHandler : MonoBehaviour
{
    public BreakableTilemap breakableTilemap;

    void Start()
    {
        breakableTilemap = FindObjectOfType<BreakableTilemap>();

        if (breakableTilemap == null)
        {
            Debug.LogError("❌ ERROR: BreakableTilemap script not found in the scene!");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f; // Ensure correct 2D position

            Vector3Int tilePosition = breakableTilemap.tilemap.WorldToCell(mouseWorldPos);

            if (breakableTilemap.tilemap.HasTile(tilePosition))
            {
                Debug.Log("✅ Click detected on tile at: " + tilePosition);
                breakableTilemap.ProcessTileClick(tilePosition);
            }
            else
            {
                Debug.LogWarning("⚠ No tile detected at: " + tilePosition);
            }
        }
    }
}
