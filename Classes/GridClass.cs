using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridClass<TGridObject>
{
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    private TGridObject[,] gridArray;
    private int width;
    private int height;
    private float tileSize;
    private Vector3 gridOffset;

    private TextMesh[,] debugTextArray;
    private bool debug;

    public GridClass(int _width, int _height, Func<GridClass<TGridObject>, int, int, TGridObject> createGridObject, float _tileSize = 1, Vector2 _gridOffset = default(Vector2), bool _debug = false)
    {
        gridArray = new TGridObject[_width, _height];
        width = _width;
        height = _height;
        tileSize = _tileSize;
        gridOffset = _gridOffset;
        debug = _debug;

        
        debugTextArray = new TextMesh[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);

                if (debug)
                {
                    debugTextArray[x, y] = Utils.CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(tileSize, tileSize) * .5f, 3, null, TextAnchor.MiddleCenter, TextAlignment.Center);
                }
                Debug.DrawLine(GetWorldPosition(x,y), GetWorldPosition(x, y + 1), Color.white, Mathf.Infinity);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, Mathf.Infinity);
            }
        }

        Debug.DrawLine(GetWorldPosition(width, height), GetWorldPosition(width,  0), Color.white, Mathf.Infinity);
        Debug.DrawLine(GetWorldPosition(width, height), GetWorldPosition(0, height), Color.white, Mathf.Infinity);

        OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
        {
            if (debug)
            {
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
            }
        };
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x,y) * tileSize + gridOffset;
    }

    public void GetCordinates(Vector3 position, out int x, out int y)
    {
        x = Mathf.FloorToInt((position - gridOffset).x / tileSize);
        y = Mathf.FloorToInt((position - gridOffset).y / tileSize);
    }

    public bool ValidateCordinates(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return true;
        }
        return false;
    }

    public TGridObject GetObject(int x, int y)
    {
        if (ValidateCordinates(x, y))
        {
            return gridArray[x, y];
        }
        return default(TGridObject);
    }

    public TGridObject GetObject(Vector2 position)
    {
        int x, y;
        GetCordinates(position, out x, out y);

        if (ValidateCordinates(x, y))
        {
            return gridArray[x, y];
        }
        return default(TGridObject);
    }

    public void SetObject(int x, int y, TGridObject gridObject)
    {
        if (ValidateCordinates(x, y))
        {
            gridArray[x, y] = gridObject;
            TriggerGridOnTileChanged(x, y);
            UpdateDebug(x, y, gridObject);
        }
    }

    public void SetObject(Vector2 position, TGridObject gridObject)
    {
        int x, y;
        GetCordinates(position, out x, out y);
        if (ValidateCordinates(x, y))
        {
            gridArray[x, y] = gridObject;
            TriggerGridOnTileChanged(x, y);
            UpdateDebug(x, y, gridObject);
        }
    }

    public int GetGridWidth()
    {
        return gridArray.GetLength(0);
    }

    public int GetGridHeight()
    {
        return gridArray.GetLength(1);
    }

    public float GetTileSize()
    {
        return tileSize;
    }

    public void TriggerGridOnTileChanged(int x, int y)
    {
        if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
    }

    private void UpdateDebug(int x, int y, TGridObject value)
    {
        if (debug)
        {
            debugTextArray[x, y].text = value.ToString();
        }
    }
}
