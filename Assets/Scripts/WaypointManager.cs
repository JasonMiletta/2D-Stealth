using UnityEngine;
using System.Collections;

public class WaypointManager : MonoBehaviour
{
    public Waypoint[] waypoints;
    
    private void Awake()
    {
        waypoints = GetComponentsInChildren<Waypoint>();
    }
}