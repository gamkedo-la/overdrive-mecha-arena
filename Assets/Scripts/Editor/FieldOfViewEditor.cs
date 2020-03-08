using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fow = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position + Vector3.up * 18.0f, Vector3.up, Vector3.forward, 360, fow._viewRadius);

        Vector3 viewAngleA = fow.DirFromAng(-fow._viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAng(fow._viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow._viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow._viewRadius);
    }
}
