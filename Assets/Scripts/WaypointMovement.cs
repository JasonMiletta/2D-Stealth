using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class WaypointMovement : MonoBehaviour
{
    public WaypointManager waypointManager;

    public bool targetSighted = false;

    [SerializeField]
    private float m_MaxSpeed = 10f;
    [SerializeField]
    private float m_DampeningTime = 1.0f;
    [SerializeField]
    private float m_RotationSpeed = 10f;
    [SerializeField]
    private EnemyCharacter m_self;
    [SerializeField]
    private bool loop = false;

    private float m_MinimumDistanceToWaypoint = 0.05f;
    private Rigidbody2D m_RigidBody;
    private Vector3 m_Velocity = Vector3.zero;
    private float lastSqrMagnitude = Mathf.Infinity;

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
        if (!targetSighted)
        {
            MoveToNextWaypoint();
        }
    }
   
    private void MoveToNextWaypoint()
    {
        Vector3 waypointDestination = SetWaypointDestination();

        //Rotate
        transform.rotation = Quaternion.Slerp(transform.rotation, Utils.RotateToTarget(transform.position, waypointDestination), 20 * Time.deltaTime);
        //Move
        Move(waypointDestination);
    }

    private void Move(Vector3 targetWaypoint)
    {

        var currentSqrMagnitude = (targetWaypoint - transform.position).sqrMagnitude;
        /*if(currentSqrMagnitude > lastSqrMagnitude)
        {
            //tr= Vector3.zero;
        }
        else
        }
        */
        lastSqrMagnitude = currentSqrMagnitude;
        // Move the character
        //transform.position = Vector3.SmoothDamp(transform.position, targetWaypoint, ref m_Velocity, m_DampeningTime);
        transform.position = Vector3.Lerp(transform.position, targetWaypoint, Time.deltaTime);
        
    }

    private Vector3 SetWaypointDestination()
    {
        //Check if we reached the current destination, if so, point towards the next destination
        if (Vector3.Distance(transform.position, waypointManager.waypoints[currentWaypointIndex].transform.position) < m_MinimumDistanceToWaypoint)
        {
            if (loop)
            {
                if(currentWaypointIndex == maxWaypointIndex)
                {
                    currentWaypointIndex = 0;
                }
                else
                {
                    ++currentWaypointIndex;
                }
            }
            else
            {
                if(currentWaypointIndex == maxWaypointIndex)
                {
                    reversing = true;
                } 
                //was reversing and now we need to go back
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
        }

        Vector3 waypointDestination = waypointManager.waypoints[currentWaypointIndex].transform.position;
        return waypointDestination;
    }
}

