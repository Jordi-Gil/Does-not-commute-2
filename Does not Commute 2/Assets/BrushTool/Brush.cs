using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Trees))]
[CanEditMultipleObjects]
public class Brush : Editor
{
    SerializedProperty trees;

    void OnEnable()
    {
        trees = serializedObject.FindProperty("trees");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(trees);
        serializedObject.ApplyModifiedProperties();
    }
}
