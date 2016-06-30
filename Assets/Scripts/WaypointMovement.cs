using UnityEngine;
using System.Collections;

public class WaypointMovement : MonoBehaviour
{
    public WaypointManager waypointManager;

    [SerializeField]
    private float m_MaxSpeed = 10f;
    [SerializeField]
    private float m_DampeningTime = 1.0f;
    [SerializeField]
    private float m_RotationSpeed = 10f;
    [SerializeField]
    private EnemyCharacter m_self;

    private float m_MinimumDistanceToWaypoint = 0.05f;
    private Rigidbody2D m_RigidBody;
    private Vector3 m_Velocity = Vector3.zero;

    private int currentWaypointIndex = 0;
    private int maxWaypointIndex;
    private bool reversing = false;

    void Awake()
    {
        m_self = GetComponent<EnemyCharacter>();
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

        Vector3 waypointDestination = waypointManager.waypoints[currentWaypointIndex].transform.position;
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
            waypointDestination = waypointManager.waypoints[currentWaypointIndex].transform.position;
        }

        //Rotate
        transform.rotation = Quaternion.Slerp(transform.rotation, RotateToTarget(waypointDestination), 20 * Time.deltaTime);
        //Move
        Move(waypointDestination);
    }

    private void Move(Vector3 targetWaypoint)
    {
        // Move the character
        transform.position = Vector3.SmoothDamp(transform.position, targetWaypoint, ref m_Velocity, m_DampeningTime);
    }

    private Quaternion RotateToTarget(Vector3 targetVector)
    {
        var vectorToTarget = targetVector - transform.position;
        var angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);
        //return Quaternion.RotateTowards(transform.rotation, q, m_RotationSpeed);
    }
}

