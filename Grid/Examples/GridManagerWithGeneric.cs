using UnityEngine;

namespace TNNUtils.Grid.Examples
{
    public class GridManagerWithGeneric : MonoBehaviour
    {
        private Grid<int> _grid;
        
        //Required variables to create a grid
        public int width; //Width of grid
        public int height; //Height of grid
        public float tileSize; //Tilesize
        public Vector3 offset; //Offset of grid from 0,0,0

        private void Start()
        {
            //Grid must be created before use
            _grid = new Grid<int>(width, height, tileSize, offset, true);

            var tile = _grid.GetObject(0, 0); //Get int

            tile++; //Increment int
            
            _grid.SetObject(0, 0, tile); //Save int
        }
    }
}