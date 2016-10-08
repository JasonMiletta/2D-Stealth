using UnityEngine;
using System.Collections;

public class FieldOfView : MonoBehaviour {

    public float viewRadius;
    public float viewAngle;

    public Vector3 DirFromAngle(float angleInDegrees)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

	// Update is called once per frame
	void Update () {
        GameObject target = GameObject.FindGameObjectWithTag("Player");

        RaycastHit2D[] hit = Physics2D.RaycastAll(this.transform.position, target.transform.position - this.transform.position);

        if (target != null && hit.Length > 1)
        {
            if (hit[1].transform.tag == "Player")
            {
                Debug.DrawRay(this.transform.position, target.transform.position - this.transform.position, Color.red);

            }
            else
            {
                Debug.DrawRay(this.transform.position, target.transform.position - this.transform.position, Color.green);
            }
        }
    }
}
