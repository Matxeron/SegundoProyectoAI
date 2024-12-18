using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    private enum State { Patrol, Chase, Alert }
    private State currentState = State.Patrol;
    public LayerMask layerWall;
    public LayerMask layerWaypoint;
    public float viewRadius;
    public float speed;
    public Transform targetPlayer;
    public List<Node> allnodes;

    public Node[] Waypoints;
    [SerializeField] private int currentWaypointIndex;
    [SerializeField]int nextWaypoint;
    public GameObject targetMove;
    public bool PlayerView = false;
   
    public FieldOfView _fieldofview;
    Node playerPosition;
    Node currentPosition;
    public bool printeo;

    public bool alerte = false;

    int lastWaypoint;

    private void Start()
    {
        _fieldofview = GetComponent<FieldOfView>();
    }

    //FSM para decidir si patrullo, persigo al jugador o alerto a los demas NPCs
    void Update()
    {
        _fieldofview.FOV();


        switch (currentState)
        {
            case State.Patrol:
                WalkingAStar();
                break;
            case State.Chase:
                ChasingState();
                break;

        }
    }

    private void WalkingAStar()
    {
        //Si ya llego al waypoint vuelve a llamar a la rutina
        if (currentWaypointIndex == nextWaypoint)
        {
            StartCoroutine(FollowPath(CallAstar()));
            alerte = false;

        }

        if (PlayerView)
        {
            lastWaypoint= nextWaypoint;
            StopAllCoroutines();
            playerPosition = null;
            currentState = State.Chase;
            GameManager.Instance.SeeEnemy();

            alerte = true;
        }

    }

    //Deberia seguir al player en todo momento, no distingue paredes
    void ChasingState()
    {

            currentPosition = GetClosestNodeToTarget(transform.position);

        //= GetClosestNodeToTarget(targetPlayer.position);
        if (playerPosition == null)
        {
            playerPosition = GetClosestNodeToTarget(targetPlayer.position);
            List<Node> path = Pathfinding.AStar(currentPosition, playerPosition);
            StartCoroutine(ChasingFollowPath(path));

        }
        else if (currentPosition == playerPosition)
        {
            if (PlayerView)
            {
                playerPosition = GetClosestNodeToTarget(targetPlayer.position);
                List<Node> path = Pathfinding.AStar(currentPosition, playerPosition);
                StartCoroutine(ChasingFollowPath(path));
                GameManager.Instance.SeeEnemy();
                alerte = true;
            }
            else
            {
                StopAllCoroutines();
                currentWaypointIndex = lastWaypoint;
                currentState = State.Patrol;
                alerte = false;

            }
        }

       
    }

    //solo para el Walking
    private IEnumerator FollowPath(List<Node> _path)
    {
        //se meuve entre los Nodos
        foreach (Node node in _path)
        {
           //En el Eje Y lleva el position del NPC.Y para evitar que se hunda o vuele
            Vector3 targetPosition = new Vector3(node.transform.position.x, transform.position.y , node.transform.position.z);
            while (Vector3.Distance(transform.position, node.transform.position) > 0.75f)
            {
                 transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                transform.LookAt(targetPosition);
                yield return null;
            }
        }
        currentWaypointIndex = nextWaypoint;
    }

    //para el chasing (La diferencia con el otro esta en lo que hacen cuando llegan al final)
    private IEnumerator ChasingFollowPath(List<Node> _path)
    {
        //se mueve entre los Nodos
        foreach (Node node in _path)
        {
            
            //En el Eje Y lleva el position del NPC.Y para evitar que se hunda o vuele
            Vector3 targetPosition = new Vector3(node.transform.position.x, transform.position.y, node.transform.position.z);
            while (Vector3.Distance(transform.position, node.transform.position) > 0.75f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                transform.LookAt(targetPosition);
                yield return null;
            }
            if (PlayerView)
            {
                StopAllCoroutines();
                playerPosition = null;
                currentState = State.Chase;
                GameManager.Instance.SeeEnemy();
                alerte = true;
            }
        }
        if (_path.Count > 0)
        {
            Node lastNode = _path[_path.Count - 1];
            transform.position = new Vector3(lastNode.transform.position.x, transform.position.y, lastNode.transform.position.z);
        }
        alerte = false;
    }

    //Crea el path solo cuando se lo llama
    private List<Node> CallAstar()
    {
        nextWaypoint = currentWaypointIndex+ 1;

        if (nextWaypoint >= Waypoints.Length) nextWaypoint = 0;
        
        List<Node> path = Pathfinding.AStar(GetClosestNodeToTarget(transform.position), Waypoints[nextWaypoint]);

        return path;

    }

    //Agarra al nodo mas cercano del player
    private Node GetClosestNodeToTarget(Vector3 targetPosition)
    {
        Node closestNode = null;
        float closestDistance = Mathf.Infinity;

        foreach (Node node in allnodes)
        {
            float distance = Vector3.Distance(node.transform.position, targetPosition);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestNode = node;
            }
        }

        return closestNode;
    }


   public void FollowCall()
    {
       lastWaypoint = nextWaypoint;
       StopAllCoroutines();
        playerPosition = null;
   currentState = State.Chase;
        
    }
}
