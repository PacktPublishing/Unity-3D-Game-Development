using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEngine.VFX;

public class VFXGraphToolWindow : EditorWindow
{
    private int vfxParticleCount = 0;


    [MenuItem("Window/VFX Tools/VFX Tool Window", priority = 10)]
    public static void OpenFromWindow()
    {
        if (HasOpenInstances<VFXGraphToolWindow>())
        {
            GetWindow<VFXGraphToolWindow>().Close();
        }
        else
        {
            var myWindow = GetWindow<VFXGraphToolWindow>();
            myWindow.titleContent = new GUIContent("VFX Tool Window");
        }
    }

    private void OnGUI()
    {
        var allVFXGraphs = FindObjectsOfType(typeof(VisualEffect));
        int currentParticleCount = 0;
        foreach (VisualEffect VFXGraph in allVFXGraphs)
        {
            currentParticleCount += VFXGraph.aliveParticleCount;
        }

        vfxParticleCount = currentParticleCount;

        EditorGUILayout.LabelField("VFX Asset count", $"{allVFXGraphs.Count()}");
        EditorGUILayout.LabelField("VFX particle count", $"{vfxParticleCount}");


    }

    public void OnInspectorUpdate()
    {
        // This will only get called 10 times per second.
        Repaint();
    }
}
