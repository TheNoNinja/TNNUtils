using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localposition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = default(TextAnchor), TextAlignment textAlignment = default(TextAlignment), int sortingOrder = 5000)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localposition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        textMesh.characterSize = 0.05f;
        return textMesh;
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 position = GetMouseWorldPositionWithZ();
        position.z = 0f;
        return position;
    }
    public static Vector3 GetMouseWorldPosition(Camera camera)
    {
        Vector3 position = GetMouseWorldPositionWithZ(Input.mousePosition, camera);
        position.z = 0f;
        return position;
    }
    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Camera camera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, camera);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 mousePosition, Camera camera)
    {
        return camera.ScreenToWorldPoint(mousePosition);
    }
}
