using System.Numerics;

namespace Playground.Pathfinding;
public class NavigationNode
{
    public Vector2 Position {get;set;}
    public bool IsWalkable {get;set;}
    public List<NavigationNode> Neighbours {get;set;}
    public NavigationNode()
    {
        this.IsWalkable = true;
        this.Neighbours = new List<NavigationNode>();
    }
}