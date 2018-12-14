using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TransformPair))]
public class PairDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        Rect firstRect = new Rect(position.x, position.y,position.width * 0.33f, position.height);
        Rect secondRect = new Rect(position.x + position.width * 0.33f, position.y, position.width * 0.33f, position.height);
        Rect carRect = new Rect(position.x + position.width * 0.66f, position.y, position.width * 0.33f, position.height);

        SerializedProperty carProp = property.FindPropertyRelative("car");
        SerializedProperty firstProp = property.FindPropertyRelative("start");
        SerializedProperty secondProp = property.FindPropertyRelative("end");

        GUI.enabled = false;
        EditorGUI.PropertyField(carRect, carProp, GUIContent.none);
        EditorGUI.PropertyField(firstRect,firstProp,GUIContent.none);
        EditorGUI.PropertyField(secondRect, secondProp, GUIContent.none);
        GUI.enabled = true;


        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }
}
