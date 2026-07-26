using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Level))]
public class MyBehaviourEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Level myTarget = (Level)target;

        if (GUILayout.Button("Сгенерировать"))
        {
            myTarget.generate();
        }
        if (GUILayout.Button("Очистить"))
        {
            myTarget.clear();
        }
    }
}