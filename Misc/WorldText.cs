using UnityEngine;

namespace TNNUtils
{
    public static class WorldText
    {
        public static TextMesh Create(string text, Transform parent = null, Vector3 localPosition = default, Vector3 rotation = default, bool faceCamera = false, int fontSize = 40, Color? color = null, TextAnchor textAnchor = default, TextAlignment textAlignment = default, int sortingOrder = 5000)
        {
            color ??= Color.white;
            return Create(parent, text, localPosition, rotation, faceCamera, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
        }
        private static TextMesh Create(Transform parent, string text, Vector3 localPosition, Vector3 rotation, bool faceCamera, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            var gameObject = new GameObject("World_Text", typeof(TextMesh));
            if (faceCamera) { gameObject.AddComponent<FaceCamera>(); }
            var transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            transform.eulerAngles = rotation;
            var textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            textMesh.characterSize = 0.05f;
            return textMesh;
        }
    }
}