namespace TNNUtils.Grid.Examples
{
    //An example of an gridObject for a grid with a custom class
    public class GridObjectExample
    {
        //Required variables 
        public int X, Y; //You want to know the coordinates of the object in the grid.
        public readonly Grid<GridObjectExample> ParentGrid; // You want to know in what grid the object is.

        //Required Constructor. This is required to generate the grid array.
        public GridObjectExample(Grid<GridObjectExample> parentGrid, int x, int y)
        {
            ParentGrid = parentGrid;
            X = x;
            Y = y;
        }
        
        //Optional for debug text
        public override string ToString() => $"({X},{Y})";
    }
}