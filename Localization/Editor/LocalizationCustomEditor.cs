using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace TNNUtils.Localization.Editor
{
    [CustomEditor(typeof(Localization))]
    public class LocalizationCustomEditor: UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Reload language files"))
            {
                Localization.UpdateLocalization();
            }
        }
    }
}
#endif