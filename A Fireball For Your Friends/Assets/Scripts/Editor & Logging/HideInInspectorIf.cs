#if UNITY_EDITOR 
using UnityEngine;
using System;
using UnityEditor;
using System.Collections;
using System.Reflection;

/// <summary>
/// This attribute can only be shown if function returns true
/// </summary>
[AttributeUsage(System.AttributeTargets.Field)]
public class HideInInspectorIf : PropertyAttribute {
    public string methodToEvalInCurrentClass;

    public HideInInspectorIf (string evalFunc) {
        methodToEvalInCurrentClass = evalFunc;
    }
}

[CustomPropertyDrawer(typeof(HideInInspectorIf))]
public class HideInInspectorIfPropertyDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        HideInInspectorIf hideInInspectorParams = attribute as HideInInspectorIf;
        Type objectType = property.serializedObject.targetObject.GetType();
        MethodInfo method = objectType.GetMethod(hideInInspectorParams.methodToEvalInCurrentClass);

        if (method == null) {
            throw new ArgumentException("The given method doesn't exist: " + objectType + "." + hideInInspectorParams.methodToEvalInCurrentClass);
        }

        if (!method.ReturnType.Equals(typeof(bool)) || method.GetParameters().Length != 0) {
            throw new ArgumentException("The given method must return a bool and have no params: " + objectType + "." + hideInInspectorParams.methodToEvalInCurrentClass);
        }

        if (method.Invoke(property.serializedObject.targetObject, new object[0]).Equals(true)) {
            EditorGUI.PropertyField(position, property, label);
        }
    }
}
#endif