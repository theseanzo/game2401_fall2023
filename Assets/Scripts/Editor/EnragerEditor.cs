using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Enrager))]
public class EnragerEditor : Editor
{
    private void OnSceneGUI()
    {
        Enrager enrager = (Enrager)target;

        Handles.color = Color.cyan;
        Handles.DrawWireDisc(enrager.transform.position, Vector3.up, enrager.buffRadius);

        Handles.Label(enrager.transform.position + Vector3.up * enrager.buffRadius, "Buff Radius");

        EditorGUI.BeginChangeCheck();
        float newBuffRadius = Handles.RadiusHandle(Quaternion.identity, enrager.transform.position, enrager.buffRadius);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(enrager, "Change Buff Radius");
            enrager.buffRadius = Mathf.Max(0f, newBuffRadius);
        }
    }
}
