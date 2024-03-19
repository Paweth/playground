using System.Numerics;

namespace Playground.Pathfinding;
static class CostFunctions//distance estimation 
{
    //or class: HeuristicFunctions/Heuristics, method: ManhattanDistance, EuclideanDistance
    public static float ManhattanHeuristic(NavigationNode currentNode, NavigationNode targetNode) //taxicab 
    {
        Vector2 difference = Vector2.Abs(targetNode.Position - currentNode.Position);
        return difference.X + difference.Y;
    }
    public static float EuclideanHeuristic(NavigationNode currentNode, NavigationNode targetNode) //in straight line 
    {
        Vector2 difference = targetNode.Position - currentNode.Position ;
        return difference.Length();
    }

}