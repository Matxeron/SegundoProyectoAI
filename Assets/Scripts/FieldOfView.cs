using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private float viewRadius;
    [SerializeField] private float viewAngle;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask wallLayer;

    public FSM _fms;

    private void Start()
    {
        _fms = GetComponent<FSM>();
    }
    public void FOV()
    {
        Vector3 direction = player.transform.position - transform.position;
        if (Vector3.Angle(transform.forward, direction) <= viewAngle/2 && direction.magnitude < viewRadius)
        {
            player.GetComponent<Renderer>().material.color = Color.green;
            GameManager.Instance.target = player.gameObject;
            _fms.PlayerView = true;
        }
        else
        {
            _fms.PlayerView = false;
            player.GetComponent<Renderer>().material.color = Color.gray;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 lineA = GetVectorFromAngle(viewAngle / 2 + transform.eulerAngles.y);
        Vector3 lineB = GetVectorFromAngle(-viewAngle / 2 + transform.eulerAngles.y);

        Gizmos.DrawLine(transform.position, transform.position + lineA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + lineB * viewRadius);
    }

    private Vector3 GetVectorFromAngle(float angle)
    {
        return new Vector3(Mathf.Sin(angle*Mathf.Deg2Rad), 0, Mathf.Cos(angle*Mathf.Deg2Rad));
    }
}
