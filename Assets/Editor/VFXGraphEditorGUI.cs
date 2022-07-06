using UnityEngine;
using UnityEditor;
using UnityEngine.VFX;

[CustomEditor(typeof(VisualEffect))]
public class VFXGraphEditorGUI : Editor
{
    private int vfxParticleCount = 0;

    public void OnInspectorUpdate()
    {
        // This will only get called 10 times per second.
        Repaint();
    }

    void OnSceneGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 200, 100));

        var rect = EditorGUILayout.BeginVertical();
        GUI.color = Color.yellow;
        GUI.Box(rect, GUIContent.none);

        GUI.color = Color.white;

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("VFX Graph Particles");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label($"{(target as VisualEffect).aliveParticleCount}");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        GUILayout.EndArea();
    }
}