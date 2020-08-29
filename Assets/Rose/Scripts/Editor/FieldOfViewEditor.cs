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
        Handles.DrawWireArc(fow.transform.position, fow.transform.up, fow.transform.forward, 360, fow.viewRadius);
        Handles.DrawWireArc(fow.transform.position, fow.transform.forward, fow.transform.up, 360, fow.viewRadius);
        Handles.DrawWireArc(fow.transform.position, fow.transform.right, fow.transform.up, 360, fow.viewRadius);
        Vector3 viewAnglePhiA = fow.DirFromAnglePhi(-fow.viewAnglePhi / 2/*, false*/);
        Vector3 viewAnglePhiB = fow.DirFromAnglePhi(fow.viewAnglePhi / 2/*, false*/);

        Vector3 viewAngleThetaA = fow.DirFromAngleTheta(-fow.viewAngleTheta / 2/*, true*/);
        Vector3 viewAngleThetaB = fow.DirFromAngleTheta(fow.viewAngleTheta / 2/*, true*/);
        
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAnglePhiA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAnglePhiB * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleThetaA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleThetaB * fow.viewRadius);

        Handles.color = Color.red;
        foreach(Transform visibleTarget in fow.visibleTargets)
        {
            Handles.DrawLine(fow.transform.position, visibleTarget.position);
        }
    }
}
