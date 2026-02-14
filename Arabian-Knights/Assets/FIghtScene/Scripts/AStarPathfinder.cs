using UnityEngine;
using System.Collections.Generic;

public class AStarPathfinder : MonoBehaviour
{
    PathfindingGrid grid;

    void Awake()
    {
        grid = GetComponent<PathfindingGrid>();
    }

    public List<Vector2> FindPath(Vector2 startPos, Vector2 targetPos)
    {
        GridNode startNode = grid.NodeFromWorldPoint(startPos);
        GridNode targetNode = grid.NodeFromWorldPoint(targetPos);

        List<GridNode> openSet = new List<GridNode>();
        HashSet<GridNode> closedSet = new HashSet<GridNode>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            GridNode currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                    openSet[i].fCost == currentNode.fCost &&
                    openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
                return RetracePath(startNode, targetNode);

            foreach (GridNode neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                    continue;

                int newCost = currentNode.gCost +
                    GetDistance(currentNode, neighbour);

                if (newCost < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCost;
                    neighbour.hCost =
                        GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return null;
    }

    List<Vector2> RetracePath(GridNode startNode, GridNode endNode)
    {
        List<Vector2> path = new List<Vector2>();
        GridNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.worldPosition);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    int GetDistance(GridNode a, GridNode b)
    {
        int dstX = Mathf.Abs(a.gridX - b.gridX);
        int dstY = Mathf.Abs(a.gridY - b.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
