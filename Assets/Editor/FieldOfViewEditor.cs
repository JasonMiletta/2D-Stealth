using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (FieldOfView))]
public class FieldOfViewEditor : Editor {

    void OnSceneGUI()
    {
        FieldOfView fow = (FieldOfView)target;
        Handles.color = Color.white;

        drawViewRadius(fow);
        drawViewAngle(fow);
        //drawViewRayCast(fow);

        Handles.color = Color.red;
        foreach(Transform visibleTarget in fow.visibleTargets)
        {
            Handles.DrawLine(fow.transform.position, visibleTarget.position);
        }
    }

    private void drawViewRadius(FieldOfView fow) {
        Handles.DrawWireArc(fow.transform.position, Vector3.forward, Vector3.up, 360, fow.viewRadius);
    }

    private void drawViewAngle(FieldOfView fow)
    {
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);
       
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);
    }

    private void drawViewRayCast(FieldOfView fow)
    {
        GameObject target = GameObject.FindGameObjectWithTag("Player");

        RaycastHit2D[] hit = Physics2D.RaycastAll(fow.transform.position, target.transform.position - fow.transform.position);

        if (target != null && hit.Length > 1)
        {
            if (hit[1].transform.tag == "Player")
            {
                Debug.DrawRay(fow.transform.position, target.transform.position - fow.transform.position, Color.red);

            }
            else
            {
                Debug.DrawRay(fow.transform.position, target.transform.position - fow.transform.position, Color.green);
            }
        }
    }
    
}
