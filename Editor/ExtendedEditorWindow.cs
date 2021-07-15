using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TNNUtils.Editor
{
    public class ExtendedEditorWindow : EditorWindow
    {
        protected SerializedObject SerializedObject;
        protected SerializedProperty CurrentProperty;

        protected ReorderableList list;
        
        private string _selectedPropertyPath;
        protected SerializedProperty SelectedProperty;

        protected void DrawProperties(SerializedProperty prop, bool drawChildren)
        {
            var lastPropPath = string.Empty;

            foreach (SerializedProperty p in prop)
            {
                if (p.isArray && p.propertyType == SerializedPropertyType.Generic)
                {
                    EditorGUILayout.BeginHorizontal();
                    p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                    EditorGUILayout.EndHorizontal();

                    if (p.isExpanded)
                    {
                        EditorGUI.indentLevel++;
                        DrawProperties(p, drawChildren);
                        EditorGUI.indentLevel--;
                        
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath)) continue;

                    lastPropPath = p.propertyPath;
                    EditorGUILayout.PropertyField(p, drawChildren);
                }
            }
        }

        protected void DrawUnorderedSidebar(SerializedProperty prop)
        {
            foreach (SerializedProperty p in prop)
            {
                if (GUILayout.Button(p.displayName))
                {
                    _selectedPropertyPath = p.propertyPath;
                }
            }
            
            if (!string.IsNullOrEmpty(_selectedPropertyPath))
            {
                SelectedProperty = SerializedObject.FindProperty(_selectedPropertyPath);
            }
            
        }

        protected void DrawOrderedSidebar(SerializedProperty prop, ReorderableList.ElementCallbackDelegate drawElementCallback = default)
        {
            if (list == null) InitiateReorderableList(prop, drawElementCallback);
            list.DoLayoutList();
            
            if (!string.IsNullOrEmpty(_selectedPropertyPath))
            {
                SelectedProperty = SerializedObject.FindProperty(_selectedPropertyPath);
            }
        }

        private void InitiateReorderableList(SerializedProperty prop, ReorderableList.ElementCallbackDelegate drawElementCallback)
        {
            list = new ReorderableList(SerializedObject, prop, true, false, true, true)
            {
                multiSelect = false,
                onSelectCallback = reorderableList =>
                    _selectedPropertyPath = reorderableList.serializedProperty
                        .GetArrayElementAtIndex(reorderableList.selectedIndices.Single())
                        .propertyPath,
                onChangedCallback = reorderableList =>
                {
                    for (var x = 0; x < reorderableList.serializedProperty.arraySize; x++)
                    {
                        reorderableList.serializedProperty.GetArrayElementAtIndex(x).FindPropertyRelative("ID")
                            .intValue = x;
                    }
                },
                drawElementCallback = drawElementCallback
            };
        }

        protected void DrawField(string propName, bool relative)
        {
            if (relative && CurrentProperty != null)
            {
                EditorGUILayout.PropertyField(CurrentProperty.FindPropertyRelative(propName), true);
            } else if (SerializedObject != null)
            {
                EditorGUILayout.PropertyField(SerializedObject.FindProperty(propName), true);
            }
        }

        protected void Apply()
        {
            SerializedObject.ApplyModifiedProperties();
        }
    }
}
