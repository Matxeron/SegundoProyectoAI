using UnityEngine;

public class Grid : MonoBehaviour
{
    public int width;
    public int height;
    GameObject[,] _grid;
    public GameObject[] _node;
    private int count = 0;

    private void Awake()
    {
        _grid = new GameObject[width, height];
        for (int x=0; x<width; x++)
        {
            for (int  y=0; y<height; y++)
            {
                _grid[x, y] = _node[count];
                Node n = _node[count].GetComponent<Node>();
                n.x = x;
                n.y = y;
                count++;
            }
        }
    }

    public Node GetNode(int x, int y)
    {
        if (x > width -1 || x <0 || y > height -1 || y<0) { return null; }
        return _grid[x,y].GetComponent<Node>();
    }
}
