using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Parallax
{
    [CustomEditor(typeof(ParallaxItem))]
    [CanEditMultipleObjects]
    public class ParallaxItemEditor : Editor
    {
        SerializedProperty position;

        void OnEnable()
        {
            position = serializedObject.FindProperty("naturalPositionRaw");
        }

        public override void OnInspectorGUI()
        {
            ParallaxItem myTarget = (ParallaxItem)target;

            serializedObject.Update();
            myTarget.useStartingTransformAsNaturalPosition = EditorGUILayout.ToggleLeft("Use Transform Position as Natural Position", myTarget.useStartingTransformAsNaturalPosition);
            if (!myTarget.useStartingTransformAsNaturalPosition)
            {
                EditorGUILayout.PropertyField(position, new GUIContent("Natural Position"));
            }
            serializedObject.ApplyModifiedProperties();
        }
    }