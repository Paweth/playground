using System;
using System.Numerics;
using Playground.World;

namespace Playground.Pathfinding;

public class NavGrid
{
    GridSpace2D recipeGrid; //baseGrid/masterGrid/modelGrid: generated NavGrid Cells will be placed where cells of recipeGrid are placed
    public NavigationNode[,] Cells {get;set;} //Points/IntersectionPoints?

    List<NavigationNode> walkableNodes;//nazwa do zmiany
    public int CellSize{get;set;}
    public Vector2 Dimensions{get;set;}
    Vector2 _offset;
    Vector2 Offset //shifts point that is considered as cell's position in it's square space (default centre of cell). When it's set to (0,0) it means that pathfinding will be computed between nodes that are placed where grid intersection points are (top-left of cell).
    {
        get{return _offset;}
        set //TODO: change so value to be set should be between 0 and 1 range (1 corresponding to CellSize)
        {
            if(value.X > 0 && value.X <= recipeGrid.CellSize
            && value.Y > 0 && value.Y <= recipeGrid.CellSize)
                _offset = value;
            else _offset = new Vector2(0,0);
        }
    }
    public NavGrid(GridSpace2D parentGrid, int columnsCount, int rowsCount)
    {
        this.recipeGrid = parentGrid;
        this.Cells = new NavigationNode[columnsCount,rowsCount];
        this.Dimensions = new Vector2(columnsCount, rowsCount);
        this.CellSize = recipeGrid.CellSize;
        this.Offset = new Vector2(0,0);
        this.walkableNodes = new List<NavigationNode>();
        //InitializeCells(columnsCount, rowsCount);
        InitializeCellsDiagonal(columnsCount, rowsCount);
    }
    public NavGrid(GridSpace2D parentGrid, int columnsCount, int rowsCount, Vector2 offset)
    {
        this.recipeGrid = parentGrid;
        this.Cells = new NavigationNode[columnsCount,rowsCount];
        this.Dimensions = new Vector2(columnsCount, rowsCount);
        this.Offset = offset;
        this.walkableNodes = new List<NavigationNode>();
        InitializeCells(columnsCount, rowsCount);
    }
    // public NavigationNode GetCellByIndex(int xIndex, int yIndex)
    // {
    //     return Cells[xIndex, yIndex];
    // }

    public NavigationNode GetRandomWalkableCell()
    {
        Random random = new Random();
        Console.WriteLine(walkableNodes.Count);
        return walkableNodes[random.Next(0, walkableNodes.Count)];
    }
    private void SetNodeState(NavigationNode node, bool isWalkable)//change node's state of IsWalkable property
    {   
        bool currentState = node.IsWalkable;
        // if(isWalkable == currentState) return;
        if(isWalkable == false)
        {
            if(walkableNodes.Contains(node)) walkableNodes.Remove(node);
        }
        else if(!walkableNodes.Contains(node))
        {
            walkableNodes.Add(node);
        }
        node.IsWalkable = isWalkable;
    }

    public NavigationNode GetCellByPosition(Vector2 position)
    {
        (int ix, int iy) = recipeGrid.GetCellIndex(position);
        if(ix < Cells.GetLength(0) && iy < Cells.GetLength(1))
        {
            return Cells[ix, iy];
        }
        return null;//or return cell that is the closest to requested position
    }

    void InitializeCells(int columnsCount, int rowsCount)
    {
        /*
            +--------->
            | 
            >--------->
        */
        Random random = new Random();
        for(int j = 0; j < rowsCount; j++) //iterating through y-axis cells
        {

            for(int i = 0; i < columnsCount; i++) //iterating through x-axis cells
            {
                Cells[i,j] = new NavigationNode();
                var currentNode = Cells[i,j];
                currentNode.Position = recipeGrid[i,j];
                int number = random.Next(0, 101);
                if(number < 20) currentNode.IsWalkable = false;
                else currentNode.IsWalkable = true;

                var neighbours = currentNode.Neighbours;
                if(i > 0) //adding left neighbour to current node and adding current node as right neighbour of previous node (left neighbour)
                {
                    neighbours.Add(Cells[i - 1, j]); 
                    Cells[i - 1, j].Neighbours.Add(currentNode);
                    Console.WriteLine($"LEFT\ti:{i},j:{j}\tx:{neighbours.Last().Position.X}, y:{neighbours.Last().Position.Y}");
                }                
                if(j > 0) //adding top neighbour
                {
                    neighbours.Add(Cells[i, j - 1]); 
                    Cells[i, j - 1].Neighbours.Add(currentNode);
                    Console.WriteLine($"TOP\ti:{i},j:{j}\tx:{neighbours.Last().Position.X}, y:{neighbours.Last().Position.Y}");
                }   
            }
        }
    }
    void InitializeCellsDiagonal(int columnsCount, int rowsCount)
    {
        /*
            +--------->
            | 
            >--------->
        */
        Random random = new Random();
        for(int j = 0; j < rowsCount; j++) //iterating through y-axis cells
        {

            for(int i = 0; i < columnsCount; i++) //iterating through x-axis cells
            {
                Cells[i,j] = new NavigationNode();
                var currentNode = Cells[i,j];
                currentNode.Position = recipeGrid[i,j];

                int number = random.Next(0, 101);
                if(number < 10) SetNodeState(currentNode, false);
                else SetNodeState(currentNode, true);

                var neighbours = currentNode.Neighbours;
                if(i > 0) //adding left neighbour to current node and adding current node as right neighbour of previous node (left neighbour)
                {
                    neighbours.Add(Cells[i - 1, j]); 
                    Cells[i - 1, j].Neighbours.Add(currentNode);
                }                
                if(j > 0) //adding top neighbour
                {
                    neighbours.Add(Cells[i, j - 1]); 
                    Cells[i, j - 1].Neighbours.Add(currentNode);
                }
                if(i > 0 && j > 0)
                {
                    if(i + 1 < columnsCount)
                    {
                        neighbours.Add(Cells[i + 1, j - 1]);
                        Cells[i + 1, j - 1].Neighbours.Add(currentNode);
                    }
                    neighbours.Add(Cells[i - 1, j - 1]);
                    Cells[i - 1, j - 1].Neighbours.Add(currentNode);
                }
                    
            }
        }        
    }


}