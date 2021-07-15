using UnityEngine;

namespace TNNUtils.Localization
{
    [RequireComponent(typeof(TextMesh))]
    public class UITextLocalization : MonoBehaviour
    {
        public string key;
        private TextMesh _textMesh;

        public void Start()
        {
            _textMesh = gameObject.GetComponent<TextMesh>();

            Localization.OnLanguageChange += OnLanguageChange;

            if (!string.IsNullOrEmpty(_textMesh.text)) key = _textMesh.text;

            UpdateLocalization();
        }

        private void OnLanguageChange(object _, Localization.LanguageChangeEventArgs eventArgs)
        {
            UpdateLocalization();
        }

        private void UpdateLocalization()
        {
            _textMesh.text = Localization.GetLocalizedValue(key);
        }
    }
}