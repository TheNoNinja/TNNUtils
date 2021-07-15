using System;
using UnityEngine;

namespace TNNUtils.Grid.Examples
{
    public class GridManagerWithCustomGridObjectClass: MonoBehaviour
    {
        //Reference to the grid
        private Grid<GridObjectExample> _grid;
        
        //Required variables to create a grid
        public int width; //Width of grid
        public int height; //Height of grid
        public float tileSize; //Tilesize
        public Vector3 offset; //Offset of grid from 0,0,0

        private void Start()
        {
            //Grid must be created before use
            _grid = new Grid<GridObjectExample>(
                width, 
                height, 
                (grid, x, y) => new GridObjectExample(grid, x, y),  //Define how to call the constructor of the grid object
                tileSize, 
                offset, 
                true);

            
            var tile = _grid.GetObject(0, 0);
            Debug.Log(tile);
            
            _grid.SetObject(0, 0, tile);
        }
    }
}