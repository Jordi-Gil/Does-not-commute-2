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

        Rect firstRect = new Rect(position.x, position.y,position.width * 0.5f, position.height);
        Rect secondRect = new Rect(position.x + position.width * 0.5f, position.y, position.width * 0.5f, position.height);

        SerializedProperty firstProp = property.FindPropertyRelative("first");
        SerializedProperty secondProp = property.FindPropertyRelative("second");

        EditorGUI.PropertyField(firstRect,firstProp,GUIContent.none);
        EditorGUI.PropertyField(secondRect, secondProp, GUIContent.none);

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }
}
