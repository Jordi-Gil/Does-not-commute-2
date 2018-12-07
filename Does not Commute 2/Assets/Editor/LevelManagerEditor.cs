using UnityEditor;
using UnityEngine;

public class LevelManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        SerializedProperty roundProp = serializedObject.FindProperty("round");
        GUIContent roundContent = new GUIContent("Round");
        EditorGUILayout.PropertyField(roundProp, roundContent);

        SerializedProperty cameraProp = serializedObject.FindProperty("scriptCamera");
        GUI.enabled = false;
        EditorGUILayout.PropertyField(cameraProp);
        GUI.enabled = true;

        SerializedProperty carsProp = serializedObject.FindProperty("cars");
        GUIContent carsContent = new GUIContent("Cars","List of all cars used in this level.");
        EditorGUILayout.PropertyField(carsProp, carsContent);

        SerializedProperty pathsProp = serializedObject.FindProperty("paths");
        GUIContent pathsContent = new GUIContent("Roads", "List with the roads, with start and end, of this level.");
        EditorGUILayout.PropertyField(pathsProp, pathsContent);

        GUI.enabled = false;
        SerializedProperty activeCarProp = serializedObject.FindProperty("activeCar");
        GUIContent activeCarContent = new GUIContent("Car controlled by player");
        EditorGUILayout.PropertyField(activeCarProp, activeCarContent);
        GUI.enabled = true;

        serializedObject.ApplyModifiedProperties();
    }
}
