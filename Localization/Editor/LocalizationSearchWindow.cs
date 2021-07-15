using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace TNNUtils.Localization.Editor
{
    public class LocalizationSearchWindow : EditorWindow
    {
        public static void Open()
        {
            var window = CreateInstance<LocalizationSearchWindow>();
            window.titleContent = new GUIContent("Localization search");

            var mouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            var rect = new Rect(mouse.x - 450, mouse.y + 10, 10, 10);
            window.ShowAsDropDown(rect, new Vector2(500, 300));
        }

        public string value;
        public Vector2 scroll;
        public LocalizedLanguage LocalizedLanguage;

        private void OnEnable()
        {
            Localization.UpdateLocalization();
            LocalizedLanguage = Localization.CurrentLocalizedLanguage;
        }

        public void OnGUI()
        {
            EditorGUILayout.BeginHorizontal("Box");
            EditorGUILayout.LabelField("Search: ", EditorStyles.boldLabel, GUILayout.MaxWidth(47));
            value = EditorGUILayout.TextField(value);
            EditorGUILayout.EndHorizontal();

            value ??= "";
            
            EditorGUILayout.BeginVertical();
            scroll = EditorGUILayout.BeginScrollView(scroll);

            foreach (var kvp in LocalizedLanguage.Localization.Where(kvp => kvp.Key.ToLower().Contains(value.ToLower()) || kvp.Value.ToLower().Contains(value.ToLower())))
            {
                EditorGUILayout.BeginHorizontal("Box");
                var deleteIcon = Resources.Load<Texture>("LocalizationEditorTextures/Delete");
                var content = new GUIContent(deleteIcon);

                if (GUILayout.Button(content, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                {
                    if (EditorUtility.DisplayDialog($"Remove key '{kvp.Key}'", $"This will remove '{kvp.Key}' from the localization", "Sure", "Cancel"))
                    {
                        Localization.Remove(kvp.Key);
                        Localization.UpdateLocalization();
                        LocalizedLanguage = Localization.CurrentLocalizedLanguage;
                    }
                }

                EditorGUILayout.TextField(kvp.Key);
                EditorGUILayout.LabelField(kvp.Value);
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
    }
}
#endif