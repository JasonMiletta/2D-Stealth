using UnityEngine;
using System.Collections;

[System.Serializable]
public class Waypoint : MonoBehaviour {

    public Vector3 location;

	// Use this for initialization
	void Start () {
        location = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
