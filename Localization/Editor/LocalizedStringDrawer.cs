using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace TNNUtils.Localization.Editor
{
    [CustomPropertyDrawer(typeof(LocalizedString))]
    public class LocalizedStringDrawer : PropertyDrawer
    {
        private bool _dropdown;
        private float _height;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => _dropdown ? _height + 22 : 18;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (Event.current.type == EventType.Layout) return;
            
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            position.width -= 34;
            position.height = 18;

            var valueRect = new Rect(position);
            valueRect.x += 15;
            valueRect.width -= 15;

            var foldButtonRect = new Rect(position) {width = 15};

            _dropdown = EditorGUI.Foldout(foldButtonRect, _dropdown, "");

            position.x += 15;
            position.width -= 15;

            var key = property.FindPropertyRelative("key");
            key.stringValue = EditorGUI.TextArea(position, key.stringValue);

            position.x += position.width + 2;
            position.width = 17;
            position.height = 17;

            var searchIcon = Resources.Load<Texture>("LocalizationEditorTextures/Search");
            var searchContent = new GUIContent(searchIcon, "Search");
            
            if (GUI.Button(position, searchContent))
            {
                LocalizationSearchWindow.Open();
            }

            position.x += position.width + 2;
            
            var editIcon = Resources.Load<Texture>("LocalizationEditorTextures/Edit");
            var editContent = new GUIContent(editIcon, "Edit");
            
            if (GUI.Button(position, editContent))
            {
                LocalizationEditWindow.Open(key.stringValue);
            }

            if (_dropdown)
            {
                var value = Localization.GetLocalizedValue(key.stringValue);
                var style = GUI.skin.box;
                _height = style.CalcHeight(new GUIContent(value), valueRect.width);
                
                valueRect.height = _height;
                valueRect.y += 21;
                EditorGUI.LabelField(valueRect, value, EditorStyles.wordWrappedLabel);
            }
            
            EditorGUI.EndProperty();
        }
    }
}
#endif