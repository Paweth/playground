using System;
using System.Numerics;

namespace Playground.World;


// is base that allows ease of creation multiple other grids (navgrid) that are coherent with each other
public class GridSpace2D //regular square grid whose cell's positions can be easily computed 
{
    float worldOrigin;//or origin/ it sets origin position of GridSpace2D in world space 
    public int CellSize{get;set;}
    public Vector2 this[int i, int j] //get position of i-th (column) and j-th (row) cell from grid
    {
        get{return new Vector2(CellSize * i + worldOrigin, CellSize * j + worldOrigin) ;}
    }
    public GridSpace2D(int CellSize)
    {
        this.CellSize = CellSize;
        this.worldOrigin = 0f;
    }
    public GridSpace2D(int CellSize, float worldOrigin)
    {
        this.CellSize = CellSize;
        this.worldOrigin = worldOrigin;
    }
    public (int xIndex, int yIndex) GetCellIndex(Vector2 position)//returns x and y indexes
    {
        //0: 0 -5
        //1: 5 -10
        //2: 10-15
        int xIndex = (int)Math.Floor(position.X / CellSize);
        int yIndex = (int)Math.Floor(position.Y / CellSize);
        return (xIndex, yIndex);
    }
    
}