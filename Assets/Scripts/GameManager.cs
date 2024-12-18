using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Grid grid;

    public GameObject target;

    public FSM[] allie;

    private void Awake()
    {
        Instance = this;
    }

    public void StartPath(Node startNode, Node endNode)
    { 
        StartCoroutine(Pathfinding.PaintAStar(startNode, endNode));
    }

    public void PaintNode(Node n, Color color)
    {
        n.gameObject.GetComponent<Renderer>().material.color = color;
    }


    public void SeeEnemy()
    {
        for (int i = 0; i < allie.Length; i++)
        {
            if (allie[i].alerte) return;
            allie[i].FollowCall();
        }
    }
}
