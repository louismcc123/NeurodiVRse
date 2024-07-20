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

        float cellWidth = (parentWidth - (gridLayoutGroup.padding.left + gridLayoutGroup.padding.right + (gridLayoutGroup.spacing.x * (columns - 1)))) / columns;
        float cellHeight = (parentHeight - (gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom + (gridLayoutGroup.spacing.y * ((gridLayoutGroup.transform.childCount / columns) - 1)))) / (gridLayoutGroup.transform.childCount / columns);

        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);
    }
}
