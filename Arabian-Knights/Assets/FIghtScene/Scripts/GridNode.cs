using UnityEngine;

public class GridNode
{
    public bool walkable;
    public Vector2 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public GridNode parent;

    public int fCost => gCost + hCost;

    public GridNode(bool walkable, Vector2 worldPosition, int x, int y)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        gridX = x;
        gridY = y;
    }
}
