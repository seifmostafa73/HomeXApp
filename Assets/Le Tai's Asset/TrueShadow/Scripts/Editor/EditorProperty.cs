using System;
using System.Reflection;
using UnityEditor;

namespace LeTai.TrueShadow.Editor
{
public class EditorProperty
{
    public readonly SerializedProperty serializedProperty;

    readonly SerializedObject   serializedObject;
    readonly PropertyInfo       property;
    readonly SerializedProperty dirtyFlag;

    public EditorProperty(SerializedObject obj, string name)
    {
        serializedObject   = obj;
        serializedProperty = serializedObject.FindProperty(char.ToLower(name[0]) + name.Substring(1));
        property           = serializedObject.targetObject.GetType().GetProperty(name);

        dirtyFlag = serializedObject.FindProperty("modifiedFromInspector");
    }

    public void Draw()
    {
        using (var scope = new EditorGUI.ChangeCheckScope())
        {
            EditorGUILayout.PropertyField(serializedProperty);


            if (scope.changed)
            {
                dirtyFlag.boolValue = true;
                serializedObject.ApplyModifiedProperties();

                foreach (var target in serializedObject.targetObjects)
                {
                    switch (serializedProperty.propertyType)
                    {
                        case SerializedPropertyType.Float:
                            property.SetMethod.Invoke(target, new object[] {serializedProperty.floatValue});
                            break;
                        case SerializedPropertyType.Enum:
                            property.SetMethod.Invoke(target, new object[] {serializedProperty.enumValueIndex});
                            break;
                        case SerializedPropertyType.Boolean:
                            property.SetMethod.Invoke(target, new object[] {serializedProperty.boolValue});
                            break;
                        case SerializedPropertyType.Color:
                            property.SetMethod.Invoke(target, new object[] {serializedProperty.colorValue});
                            break;
                        default: throw new NotImplementedException();
                    }
                }
            }
        }
    }
}
}
