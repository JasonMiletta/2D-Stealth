using UnityEngine;
using System.Collections;

public class WaypointMovement : MonoBehaviour
{
    public WaypointManager waypointManager;

    [SerializeField]
    private float m_DampeningTime = 1.0f;
    [SerializeField]
    private float m_RotationSpeed = 0.10f;
    private float m_MinimumDistanceToWaypoint = 0.05f;
    private Rigidbody2D m_RigidBody;
    private Vector3 m_Velocity = Vector3.zero;

    private int currentWaypointIndex = 0;
    private int maxWaypointIndex;
    private bool reversing = false;

    void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        maxWaypointIndex = waypointManager.waypoints.Length - 1;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        MoveToNextWaypoint();
    }

    private void MoveToNextWaypoint()
    {
        //Check if we reached the current destination, if so, point towards the next destination
        if (Vector3.Distance(transform.position, waypointManager.waypoints[currentWaypointIndex].transform.position) < m_MinimumDistanceToWaypoint)
        {
            if (currentWaypointIndex == maxWaypointIndex)
            {
                reversing = true;
            }
            else if (currentWaypointIndex == 0)
            {
                reversing = false;
            }

            if (!reversing)
            {
                ++currentWaypointIndex;
            }
            else
            {
                --currentWaypointIndex;
            }
        }
        Vector3 waypointDestination = waypointManager.waypoints[currentWaypointIndex].transform.position;
        
        transform.position =  Vector3.SmoothDamp(transform.position, waypointDestination, ref m_Velocity, m_DampeningTime);

        transform.rotation = RotateToTarget(waypointDestination);
    }

    private Quaternion RotateToTarget(Vector3 targetVector)
    {
        Vector3 vectorToTarget = targetVector - transform.position;
        float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) - 90)* Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        return Quaternion.Slerp(transform.rotation, q, Time.deltaTime * m_RotationSpeed);
    }
}

