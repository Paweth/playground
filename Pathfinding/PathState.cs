//using 
using System.Numerics;
using rl = Raylib_cs;
using Playground.Shapes;

namespace Playground.Pathfinding;


/*  alternative names: 
    PathPhase, PathCapture, 
    SearchState, SearchPhase, SearchStage
    GridState
*/
public class PathState : IDrawable
{
    NavGrid referenceGrid;
    public Dictionary<NavigationNode, State> StateMap {get;set;}
    public enum State
    {
        Target,
        Obstacle, // excluded from pathfinding
        Inactive, // wasn't marked to be checked
        Current, // currently being checked cell
        Open, // being in open set (waiting to be checked)
        Closed, // was checked (transition from current): open->current->closed
        Result // being part of shortest path
    }
    public PathState(NavGrid grid)
    {
        this.referenceGrid = grid;
        StateMap = new Dictionary<NavigationNode, State>();
        // CellsState = new State[Cells.GetLength(0), Cells.GetLength(1)];
        for(int i = 0; i < referenceGrid.Cells.GetLength(0); i++)
        {
            for(int j = 0; j < referenceGrid.Cells.GetLength(1); j++)
            {
                var cell = referenceGrid.Cells[i,j];
                if(cell.IsWalkable) StateMap.Add(cell, State.Inactive);
                else StateMap.Add(cell, State.Obstacle);

                // if(!cell.IsWalkable) nodeStateMap.Add(cell, State.Obstacle);
                // else nodeStateMap.Add(cell, State.Inactive);
            }
        }
    }
    public void Draw()
    {

        rl.Color color = rl.Color.GRAY;
        foreach(KeyValuePair<NavigationNode, State> pair in StateMap)
        {
            NavigationNode node = pair.Key;
            Vector2 position = node.Position;
            switch(pair.Value)
            {
                case State.Closed:
                    color = rl.Color.DARKBLUE;
                    break;
                case State.Obstacle:
                    color = rl.Color.BLACK;
                    break;
                case State.Current:
                    color = rl.Color.GOLD;
                    break;
                case State.Inactive:
                    color = rl.Color.GRAY;
                    break;
                case State.Open:
                    color = rl.Color.DARKGRAY;
                    break; 
                case State.Result:
                    color = rl.Color.GREEN;
                    break;
                case State.Target:
                    color = rl.Color.RED;
                    break;                                                                             
            }
            rl.Raylib.DrawRectangle((int)position.X, (int)position.Y, referenceGrid.CellSize, referenceGrid.CellSize, color);
        }
    }
}