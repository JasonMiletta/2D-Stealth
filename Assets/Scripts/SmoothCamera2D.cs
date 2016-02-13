using UnityEngine;
using System.Collections;

public class SmoothCamera2D : MonoBehaviour {

    public float dampeningTime = 0.15f;
    public Transform target;

    private Vector3 m_velocity = Vector3.zero;
    private Camera m_camera;

    // Use this for initialization
    void Start () {
        m_camera = this.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 point = m_camera.WorldToViewportPoint(target.position);
        Vector3 delta = target.position - m_camera.ViewportToWorldPoint(new Vector3 (0.5f, 0.5f, point.z));
        Vector3 destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref m_velocity, dampeningTime);
	}
}
