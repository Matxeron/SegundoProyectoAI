using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinding
{
    public static IEnumerator PaintAStar(Node start, Node end)
    {
        if (start == null || end == null) yield return null;

        PriorityQueue frontier = new PriorityQueue();
        frontier.Put(start, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(start, null);

        Dictionary<Node, float> costSoFar = new Dictionary<Node, float>();
        costSoFar.Add(start, 0);

        while (frontier.Count > 0)
        {
            Node node = frontier.Get();

            GameManager.Instance.PaintNode(node, Color.blue);

            if (node == end)
            {
                List<Node> path = new List<Node>();
                Node nodeToAdd = node;

                while (nodeToAdd != null)
                {
                    path.Add(nodeToAdd);
                    nodeToAdd = cameFrom[nodeToAdd];
                }

                foreach (Node n in path)
                {
                    GameManager.Instance.PaintNode(n, Color.green);
                }
                break;
            }

            foreach (var n in node.GetNeighbours())
            {
                if (n.isBlocked) continue;

                float dist = Vector3.Distance(n.transform.position, end.transform.position);
                float newCost = costSoFar[node] + n.cost;
                float priority = newCost + dist;

                //Si el nodo vecino no se recorrio previamente se lo agrega a la PriorityQueue y a los diccionarios.
                if (!cameFrom.ContainsKey(n))
                {
                    frontier.Put(n, priority);
                    cameFrom.Add(n, node);
                    costSoFar.Add(n, newCost);
                }
                else
                {
                    if (newCost < costSoFar[n])//Si ya se recorrio pero se encontro un camino mas corto para llegar se le sobreescriben los datos de la manera mas corta.
                    {
                        frontier.Put(n, priority);
                        cameFrom[n] = node;
                        costSoFar[n] = newCost;
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public static List<Node> AStar(Node start, Node end)
    {
        if (start == null || end == null) return null;

        PriorityQueue frontier = new PriorityQueue();
        frontier.Put(start, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(start, null);

        Dictionary<Node, float> costSoFar = new Dictionary<Node, float>();
        costSoFar.Add(start, 0);

        while (frontier.Count > 0)
        {
            Node node = frontier.Get();
            if (node == end)
            {
                List<Node> path = new List<Node>();
                Node nodeToAdd = node;

                while (nodeToAdd != null)
                {
                    path.Add(nodeToAdd);
                    nodeToAdd = cameFrom[nodeToAdd];
                }

                path.Reverse();
                return path;
            }

            foreach (var n in node.GetNeighbours())
            {
                if (n.isBlocked) continue;

                float dist = Vector3.Distance(n.transform.position, end.transform.position);
                float newCost = costSoFar[node] + n.cost;
                float priority = newCost + dist;
                //Si el nodo vecino no se recorrio previamente se lo agrega a la PriorityQueue y a los diccionarios.
                if (!cameFrom.ContainsKey(n))
                {
                    frontier.Put(n, priority);
                    cameFrom.Add(n, node);
                    costSoFar.Add(n, newCost);
                }
                else
                {
                    if (newCost < costSoFar[n])//Si ya se recorrio pero se encontro un camino mas corto para llegar se le sobreescriben los datos de la manera mas corta.
                    {
                        frontier.Put(n, priority);
                        cameFrom[n] = node;
                        costSoFar[n] = newCost;
                    }
                }
            }
        }
        return null;
    
    }
}
