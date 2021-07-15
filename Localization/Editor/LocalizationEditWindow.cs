using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
namespace TNNUtils.Localization.Editor
{
    public class LocalizationEditWindow : EditorWindow
    {
        public static void Open(string key)
        {
            var window = CreateInstance<LocalizationEditWindow>();
            window.titleContent = new GUIContent("Localization window");
            window.key = key;
            window.value = Localization.GetLocalizedValue(key);
            var mouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            var rect = new Rect(mouse.x - 450, mouse.y + 10, 10, 10);
            window.ShowAsDropDown(rect, new Vector2(500, 300));
            
        }

        public string key;
        public string value;

        public void OnGUI()
        {
            key = EditorGUILayout.TextField("Key:", key);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Value:", GUILayout.MaxWidth(50));
            EditorStyles.textArea.wordWrap = true;
            value = EditorGUILayout.TextArea(value, EditorStyles.textArea, GUILayout.ExpandHeight(true), GUILayout.Width(400));
            EditorGUILayout.EndHorizontal();

            if (Localization.GetLocalizedValue(key) == key)
            {
                if (GUILayout.Button("Add"))
                {
                    Localization.Add(key, value);
                    Close();
                }
            }
            else
            {
                if (GUILayout.Button("Edit"))
                {
                    if (EditorUtility.DisplayDialog($"Edit key '{key}'", $"This will edit '{key}' from the localization", "Sure", "Cancel"))
                    {
                        Localization.Edit(key, value);
                        Close();
                    }
                    
                }
            }

            minSize = new Vector2(460, 250);
            maxSize = minSize;
        }
    }
}
#endif