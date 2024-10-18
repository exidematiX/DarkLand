using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Waypoint))]
public class WaypointEdit : Editor
{
    Waypoint Waypoint => target as Waypoint;
    private void OnSceneGUI()
    {

        Handles.color = Color.cyan;
        for (int i = 0; i < Waypoint.Points.Length; i++)
        {
            EditorGUI.BeginChangeCheck();

            //创造handles
            Vector3 currentWaypointPoint = Waypoint.CurrentPositions + Waypoint.Points[i];
            var fmh_20_17_638647712697184815 = Quaternion.identity; Vector3 newWaypointPoint = Handles.FreeMoveHandle(currentWaypointPoint, 0.7f, new Vector3(0.3f, 0.3f, 0.3f), Handles.SphereHandleCap);
            
            //创建 text
            GUIStyle textStyle = new GUIStyle();
            textStyle.fontStyle = FontStyle.Bold;
            textStyle.fontSize = 16;
            textStyle.normal.textColor = Color.yellow;
            Vector3 textAlligment = Vector3.down * 0.35f + Vector3.right * 0.35f;
            Handles.Label(Waypoint.CurrentPositions + Waypoint.Points[i] + textAlligment,
                $"{i + 1}", textStyle);

            EditorGUI.EndChangeCheck();

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Free Move Handle");
                Waypoint.Points[i] = newWaypointPoint - Waypoint.CurrentPositions;
            }
        }


    }
}
