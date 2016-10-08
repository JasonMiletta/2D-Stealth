﻿using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (FieldOfView))]
public class FieldOfViewEditor : Editor {

    void OnSceneGUI()
    {
        FieldOfView fow = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
    }
    
}
