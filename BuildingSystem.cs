using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public Camera mainCamera;
    public InventoryManager inventoryManager;

    public LayerMask placementBlockingLayers;
    public BuildableObject selectedObject;

    private GameObject previewInstance;
    private SpriteRenderer previewRenderer;
    private bool isBuilding = false;

    private Color validColor = new Color(1f, 1f, 1f, 0.5f);         // semi-transparent white
    private Color invalidColor = new Color(1f, 0.4f, 0.4f, 0.5f);   // semi-transparent red

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) // Toggle build mode
        {
            ToggleBuildMode();
        }

        if (!isBuilding || selectedObject == null)
            return;

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPos = Vector3Int.FloorToInt(mousePos);
        gridPos.z = 0;

        if (previewInstance == null)
        {
            previewInstance = Instantiate(selectedObject.prefab);
            previewRenderer = previewInstance.GetComponent<SpriteRenderer>();
            SetPreviewStyle(previewInstance, true);
        }

        previewInstance.transform.position = gridPos;

        bool canPlace = CanPlaceAt(gridPos);
        if (previewRenderer != null)
        {
            previewRenderer.color = canPlace ? validColor : invalidColor;
        }

        if (Input.GetMouseButtonDown(0) && canPlace)
        {
            TryPlaceObject(gridPos);
        }

        if (Input.GetMouseButtonDown(1))
        {
            CancelBuildMode();
        }
    }

    void ToggleBuildMode()
    {
        isBuilding = !isBuilding;

        if (!isBuilding && previewInstance != null)
        {
            Destroy(previewInstance);
        }
    }

    void CancelBuildMode()
    {
        isBuilding = false;

        if (previewInstance != null)
        {
            Destroy(previewInstance);
        }
    }

    void TryPlaceObject(Vector3Int position)
{
    if (!CanPlaceAt(position))
    {
        Debug.Log("Can't place here: Something is in the way.");
        return;
    }

    if (inventoryManager.woodCount >= selectedObject.woodCost &&
        inventoryManager.stoneCount >= selectedObject.stoneCost)
    {
        GameObject placed = Instantiate(selectedObject.prefab, position, Quaternion.identity);
        placed.layer = LayerMask.NameToLayer("Buildings");

        inventoryManager.woodCount -= selectedObject.woodCost;
        inventoryManager.stoneCount -= selectedObject.stoneCost;
        inventoryManager.SendMessage("UpdateUI", SendMessageOptions.DontRequireReceiver);
    }
    else
    {
        Debug.Log("Not enough resources!");
    }
}


    bool CanPlaceAt(Vector3Int position)
    {
        Vector3 worldPos = new Vector3(position.x + 0.5f, position.y + 0.5f, 0f);
        Vector2 boxSize = new Vector2(0.75f, 0.75f);

        Collider2D previewCollider = previewInstance?.GetComponent<Collider2D>();
        if (previewCollider != null) previewCollider.enabled = false;

        Collider2D[] hits = Physics2D.OverlapBoxAll(worldPos, boxSize, 0f, placementBlockingLayers);

        if (previewCollider != null) previewCollider.enabled = true;

        foreach (Collider2D c in hits)
        {
            if (c.gameObject != previewInstance && c.gameObject.layer == LayerMask.NameToLayer("Buildings"))
                return false;
        }


        return true;
    }


    void SetPreviewStyle(GameObject obj, bool isPreview)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = isPreview ? validColor : Color.white;
        }
    }
}
