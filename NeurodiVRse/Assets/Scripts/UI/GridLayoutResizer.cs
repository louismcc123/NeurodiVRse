using UnityEngine;
using UnityEngine.UI;

public class GridLayoutResizer : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup;
    public RectTransform parentRectTransform;
    public int columns = 2;

    void Start()
    {
        AdjustCellSize();
    }

    void Update()
    {
        AdjustCellSize();
    }

    void AdjustCellSize()
    {
        float parentWidth = parentRectTransform.rect.width;
        float parentHeight = parentRectTransform.rect.height;

        if (columns <= 0 || parentWidth <= 0 || parentHeight <= 0)
        {
            Debug.LogWarning("Invalid layout parameters.");
            return;
        }

        int rows = Mathf.CeilToInt((float)gridLayoutGroup.transform.childCount / columns);
        if (rows == 0)
        {
            Debug.LogWarning("No child elements in GridLayoutGroup.");
            return;
        }

        float cellWidth = (parentWidth - (gridLayoutGroup.padding.left + gridLayoutGroup.padding.right + (gridLayoutGroup.spacing.x * (columns - 1)))) / columns;
        float cellHeight = (parentHeight - (gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom + (gridLayoutGroup.spacing.y * (rows - 1)))) / rows;

        if (float.IsNaN(cellWidth) || float.IsNaN(cellHeight) || cellWidth <= 0 || cellHeight <= 0)
        {
            Debug.LogWarning($"Calculated cell size is invalid. Width: {cellWidth}, Height: {cellHeight}");
            return;
        }

        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);
    }
}
