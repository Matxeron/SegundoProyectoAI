using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public int x, y;
    public bool isBlocked = false;
    public int cost = 0;

    public List<Node> GetNeighbours()
    {
        List<Node> neighbours = new List<Node>();
        Grid grid = GameManager.Instance.grid;
        Node n = grid.GetNode(x, y - 1);

        if (n != null) neighbours.Add(n);

        n = grid.GetNode(x -1, y);
        if (n!= null) neighbours.Add(n);

        n = grid.GetNode(x,y + 1);
        if (n != null) neighbours.Add(n);

        n = grid.GetNode(x +1, y);
        if (n != null) neighbours.Add(n);

        return neighbours;
    }
}
