using UnityEngine;

public class PathfindingGrid : MonoBehaviour
{
    public LayerMask obstacleLayer;
    public Vector2 gridWorldSize = new Vector2(30, 30);
    public float nodeRadius = 0.5f;

    GridNode[,] grid;
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new GridNode[gridSizeX, gridSizeY];
        Vector2 worldBottomLeft =
            (Vector2)transform.position
            - Vector2.right * gridWorldSize.x / 2
            - Vector2.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint =
                    worldBottomLeft
                    + Vector2.right * (x * nodeDiameter + nodeRadius)
                    + Vector2.up * (y * nodeDiameter + nodeRadius);

                bool walkable =
                    !Physics2D.OverlapCircle(worldPoint, nodeRadius, obstacleLayer);

                grid[x, y] = new GridNode(walkable, worldPoint, x, y);
            }
        }
    }

    public GridNode NodeFromWorldPoint(Vector2 worldPosition)
    {
        float percentX =
            (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY =
            (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    public GridNode[] GetNeighbours(GridNode node)
    {
        var neighbours = new System.Collections.Generic.List<GridNode>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX &&
                    checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours.ToArray();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, gridWorldSize);

        if (grid == null) return;

        foreach (GridNode node in grid)
        {
            Gizmos.color = node.walkable ? Color.white : Color.red;
            Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
        }
    }
}
