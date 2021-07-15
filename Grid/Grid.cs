using System;
using UnityEngine;

namespace TNNUtils.Grid
{
    public class Grid<T>
    {
        #region fields

        //Grid variables
        public int Width { get; }
        public int Height { get; }
        public float TileSize { get; }
        private readonly T[,] _gridArray;
        private readonly Vector3 _positionOffset;

        

        //Event variables
        public class ONGridObjectChangedEventArgs : EventArgs
        {
            public readonly int X, Y;

            public ONGridObjectChangedEventArgs(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
        public event EventHandler<ONGridObjectChangedEventArgs> ONGridObjectChanged;
        
        //Debug variables
        private readonly bool _debug;
        private TextMesh[,] _debugTextArray;
        
        #endregion

        #region Constructor

        public Grid(int width, 
                    int height, 
                    Func<Grid<T>, int, int, T> gridObject, 
                    float tileSize = 1f,
                    Vector3 positionOffset = default, 
                    bool debug = false)
        {
            _gridArray = new T[width, height];
            Width = width;
            Height = height;
            this.TileSize = tileSize;
            _positionOffset = positionOffset;
            _debug = debug;
            
            InstantiateGridWithCustomClass(gridObject);
        }
        
        public Grid(int width, 
            int height,  
            float tileSize = 1f,
            Vector3 positionOffset = default, 
            bool debug = false)
        {
            _gridArray = new T[width, height];
            Width = width;
            Height = height;
            this.TileSize = tileSize;
            _positionOffset = positionOffset;
            _debug = debug;
            
            InstantiateGridWithGeneric();
        }

        #endregion

        #region Private Methods

        private void InstantiateGridWithCustomClass(Func<Grid<T>, int, int, T> gridObject)
        {
            if (_debug) _debugTextArray = new TextMesh[Width, Height];
            
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    _gridArray[x, y] = gridObject(this, x, y);

                    if (!_debug) continue;
                    
                    _debugTextArray[x, y] = WorldText.Create(_gridArray[x, y].ToString(), 
                        null, 
                        GetWorldPosition(x, y) + new Vector3(TileSize, 0, TileSize) * .5f, 
                        new Vector3(), 
                        true, 
                        60, 
                        null, 
                        TextAnchor.MiddleCenter, 
                        TextAlignment.Center);

                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.grey, Mathf.Infinity);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.grey, Mathf.Infinity);
                }
            }

            if (!_debug) return;
            
            Debug.DrawLine(GetWorldPosition(Width, Height), GetWorldPosition(Width, 0), Color.grey, Mathf.Infinity);
            Debug.DrawLine(GetWorldPosition(Width, Height), GetWorldPosition(0, Height), Color.grey, Mathf.Infinity);

            ONGridObjectChanged += (_, eventArgs) => _debugTextArray[eventArgs.X, eventArgs.Y].text = _gridArray[eventArgs.X, eventArgs.Y]?.ToString();
        }
        
        private void InstantiateGridWithGeneric()
        {
            if (!_debug) return;
            
            _debugTextArray = new TextMesh[Width, Height];
            
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    _debugTextArray[x, y] = WorldText.Create(_gridArray[x, y].ToString(), 
                        null, 
                        GetWorldPosition(x, y) + new Vector3(TileSize, 0, TileSize) * .5f, 
                        new Vector3(), 
                        true, 
                        60, 
                        null, 
                        TextAnchor.MiddleCenter, 
                        TextAlignment.Center);

                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.grey, Mathf.Infinity);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.grey, Mathf.Infinity);
                }
            }

            Debug.DrawLine(GetWorldPosition(Width, Height), GetWorldPosition(Width, 0), Color.grey, Mathf.Infinity);
            Debug.DrawLine(GetWorldPosition(Width, Height), GetWorldPosition(0, Height), Color.grey, Mathf.Infinity);

            ONGridObjectChanged += (_, eventArgs) => _debugTextArray[eventArgs.X, eventArgs.Y].text = _gridArray[eventArgs.X, eventArgs.Y]?.ToString();
        }

        private void ONGridObjectChangedTrigger(int x, int y) => ONGridObjectChanged?.Invoke(this, new ONGridObjectChangedEventArgs(x, y));
        

        #endregion
        
        #region Public Methods
        
        public bool ValidateCoordinates(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;
        public Vector3 GetWorldPosition(int x, int y) => new Vector3(x, 0, y) * TileSize + _positionOffset;
        
        public void GetCoordinates(Vector3 position, out int x, out int y)
        {
            x = Mathf.FloorToInt((position - _positionOffset).x / TileSize);
            y = Mathf.FloorToInt((position - _positionOffset).y / TileSize);
        }
        
        public T GetObject(int x, int y)
        {
            if (ValidateCoordinates(x, y)) return _gridArray[x, y];
            if (_debug) { Debug.LogWarning("[TNNUtils.Grid] Called object out of bounds at coordinates: (" + x + ", " + y + ")"); }
            return default;
        }
        public T GetObject(Vector3 position) 
        {
            GetCoordinates(position, out var x, out var y);
            return GetObject(x, y);
        }
        
        public void SetObject(int x, int y, T gridObject)
        {
            if (ValidateCoordinates(x,y))
            {
                _gridArray[x, y] = gridObject;
                ONGridObjectChangedTrigger(x, y);
            }
            else if (_debug){ Debug.LogError("[TNNUtils.Grid] Can't set object to: (" + x + ", " + y + ")"); }
        }
        public void SetObject(Vector3 position, T gridObject)
        {
            GetCoordinates(position, out var x, out var y);
            SetObject(x, y, gridObject);
        }

        #endregion
    }
}
