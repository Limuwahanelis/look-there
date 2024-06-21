using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ComboAttack))]
public class ComboAttackEditor: Editor
{
    string[] toHide = { "_nextAttackWindowStart", "_nextAttackWindowEnd", "_nextAttackWindowStartFrame", "_nextAttackWindowEndFrame" };
    SerializedProperty _useFrames;
    SerializedProperty _nextAttackWindowStart;
    SerializedProperty _nextAttackWindowEnd;
    SerializedProperty _nextAttackWindowStartFrame;
    SerializedProperty _nextAttackWindowEndFrame;
    SerializedProperty _associatedAnimation;
    private void OnEnable()
    {
        _useFrames = serializedObject.FindProperty("_useFrames");
        _nextAttackWindowStart = serializedObject.FindProperty("_nextAttackWindowStart");
        _nextAttackWindowEnd = serializedObject.FindProperty("_nextAttackWindowEnd");
        _nextAttackWindowStartFrame = serializedObject.FindProperty("_nextAttackWindowStartFrame");
        _nextAttackWindowEndFrame = serializedObject.FindProperty("_nextAttackWindowEndFrame");
        _associatedAnimation = serializedObject.FindProperty("_associatedAnimation");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawPropertiesExcluding(serializedObject, toHide);
        //EditorGUILayout.PropertyField(_associatedAnimation);
        if (_useFrames.boolValue)
        {
            EditorGUILayout.PropertyField(_nextAttackWindowStartFrame);
            EditorGUILayout.PropertyField(_nextAttackWindowEndFrame);
        }
        else
        {
            EditorGUILayout.PropertyField(_nextAttackWindowStart);
            EditorGUILayout.PropertyField(_nextAttackWindowEnd);
        }
        serializedObject.ApplyModifiedProperties();
    }
}