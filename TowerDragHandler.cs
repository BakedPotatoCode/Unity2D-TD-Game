using UnityEngine;
using UnityEngine.EventSystems;

public class TowerDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public GameObject towerPrefab;
    public int towerCost;
    public GameObject towerOL;

    private GameObject previewTower;
    private Vector3 originalPosition;
    private bool isDragging = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        CreatePreviewTower();
    }
    void Update()
    {
        if (Singleton.Instance.CoinsInPocket < towerCost)
        {
            towerOL.SetActive(true);
        }
        else
        {
            towerOL.SetActive(false);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            // Calculate the world position of the mouse pointer
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
            worldPos.z = 0; // Ensure the tower is placed at the same Z position as other objects in the scene

            // Update the position of the preview tower
            if (previewTower != null)
                previewTower.transform.position = worldPos;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        // Check if the tower is placed over a valid area
        if (previewTower != null && CanPlaceTower())
        {
            // Instantiate the tower at the position of the preview tower
            Instantiate(towerPrefab, previewTower.transform.position, Quaternion.identity);

            // Deduct the tower cost from the CoinsInPocket
            Singleton.Instance.CoinsInPocket -= towerCost;
        }

        // Destroy the preview tower
        Destroy(previewTower);

        // Reset the tower position to the original position
        transform.position = originalPosition;
    }

    private void CreatePreviewTower()
    {
        if (towerPrefab != null)
        {
            // Instantiate the preview tower at the original position
            previewTower = Instantiate(towerPrefab, originalPosition, Quaternion.identity);

            // Disable colliders and any other components that might interfere with dragging
            Collider2D[] colliders = previewTower.GetComponentsInChildren<Collider2D>();
            foreach (Collider2D collider in colliders)
            {
                collider.enabled = false;
            }
        }
    }

    private bool CanPlaceTower()
{
    // Check if the preview tower overlaps with any colliders of objects in the scene
    Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(previewTower.transform.position, 0.1f);

    foreach (Collider2D collider in overlappedColliders)
    {
        // Example: Check if the overlapped collider belongs to an object that blocks tower placement
        if (collider.CompareTag("BlockTowerPlacement"))
        {
            // Tower placement is blocked in this area
            return false;
        }
        if (collider.CompareTag("Tower"))
        {
            // Tower placement is blocked in this area
            return false;
        }
    }

    // Tower placement is allowed in this area
    return true;
}
}
