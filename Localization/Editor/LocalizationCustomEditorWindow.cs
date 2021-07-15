using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;

#if UNITY_EDITOR
namespace TNNUtils.Localization.Editor
{
    public class LocalizationCustomEditorWindow : EditorWindow
    {
        [MenuItem("Window/TNNUtils/Localization", priority = 10000)]
        public static void Open()
        {
            var window = GetWindow<LocalizationCustomEditorWindow>();
            var localizationIcon = Resources.Load<Texture>("LocalizationEditorTextures");
            var content = new GUIContent("Localization", localizationIcon);
            window.titleContent = content;
            window.value = "";
        }
        
        public string value;
        public Vector2 scroll;
        private LocalizedLanguage _localizedLanguage;

        public void OnFocus()
        {
            _localizedLanguage = Localization.CurrentLocalizedLanguage;
        }

        public void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical("Box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true), GUILayout.MinWidth(150));
            foreach (var language in (Language[]) Enum.GetValues(typeof(Language)))
            {
                if (Localization.IsLanguageLoaded(language))
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space(0.5f);
                    EditorGUILayout.BeginHorizontal();
                    
                    if (GUILayout.Button(language.ToString(),GUILayout.ExpandWidth(true)))
                    {
                        Localization.CurrentLanguage = language;
                        _localizedLanguage = Localization.CurrentLocalizedLanguage;
                    }
                    
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space(0.5f);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space(1);

                }

                
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Search:", EditorStyles.boldLabel, GUILayout.MaxWidth(47));
            value = EditorGUILayout.TextField(value);
            
            var createContent = new GUIContent("Create new");
            if (GUILayout.Button(createContent, GUILayout.Height(20), GUILayout.ExpandWidth(false)))
            {
                LocalizationEditWindow.Open("");
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            scroll = EditorGUILayout.BeginScrollView(scroll);
            _localizedLanguage ??= Localization.CurrentLocalizedLanguage;
            
            foreach (var kvp in _localizedLanguage.Localization.Where(kvp => kvp.Key.ToLower().Contains(value.ToLower()) || kvp.Value.ToLower().Contains(value.ToLower())))
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
                
                EditorGUILayout.TextField(kvp.Key);
                
                var editIcon = Resources.Load<Texture>("LocalizationEditorTextures/Edit");
                var editContent = new GUIContent(editIcon, "Edit");
            
                if (GUILayout.Button(editContent, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                {
                    LocalizationEditWindow.Open(kvp.Key);
                }
                
                var deleteIcon = Resources.Load<Texture>("LocalizationEditorTextures/Delete");
                var content = new GUIContent(deleteIcon);
                if (GUILayout.Button(content, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                {
                    if (EditorUtility.DisplayDialog($"Remove key '{kvp.Key}'", $"This will remove '{kvp.Key}' from the localization", "Sure", "Cancel"))
                    {
                        Localization.Remove(kvp.Key);
                        Localization.UpdateLocalization();
                        _localizedLanguage = Localization.CurrentLocalizedLanguage;
                    }
                }
                
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.LabelField(kvp.Value, EditorStyles.wordWrappedLabel);
                EditorGUILayout.EndVertical();
            }
            
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif