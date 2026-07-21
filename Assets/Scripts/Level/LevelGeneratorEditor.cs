using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelGenerator))]
public class MyBehaviourEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelGenerator myTarget = (LevelGenerator)target;

        if (GUILayout.Button("Сгенерировать"))
        {
            myTarget.generate();
        }
    }
}