using System.Collections.Generic;
using System.Numerics;

namespace Playground.Pathfinding;

public static class Pathfinder
{
    const int delay = 10;
    public static Stack<NavigationNode> ReconstructPath(Dictionary<NavigationNode, NavigationNode> predecessorMap, NavigationNode endNode)
    {
        Stack<NavigationNode> path = new Stack<NavigationNode>();
        NavigationNode previousNode = endNode;
        path.Push(previousNode);
        while(predecessorMap[previousNode] != null)
        {
            previousNode = predecessorMap[previousNode];
            path.Push(previousNode);
        }
        return path;
    }

    // //TODO: change (update)
    // public static Stack<NavigationNode> AStarSearch(NavigationNode startNode, NavigationNode targetNode)
    // {
    //     var travelledDistanceMap = new Dictionary<NavigationNode, float>();//<cell,distance>
    //     var predecessorMap = new Dictionary<NavigationNode, NavigationNode>();//<to, from>
    //     var closedSet = new HashSet<NavigationNode>();
    //     var openSet = new PriorityQueue<NavigationNode, float>();//TPriority <= f(n) = h(n) + g(n)

    //     NavigationNode current = startNode;
    //     // openSet.Enqueue(current, CostFunctions.EuclideanHeuristic(current, targetNode));//that heuristic doesnt really matter
    //     predecessorMap.Add(current, null);
    //     travelledDistanceMap.Add(current, 0);
    //     while(current != targetNode)
    //     {
    //         float currentDistance = travelledDistanceMap[current];
    //         foreach (NavigationNode n in current.Neighbours)
    //         {
    //             if(closedSet.Contains(n) || !n.IsWalkable) continue;
    //             predecessorMap.Add(n, current);
    //             float distanceDifference = Vector2.Distance(n.Position, current.Position);
    //             float h = CostFunctions.EuclideanHeuristic(current, targetNode);
    //             float g = currentDistance + distanceDifference; //travelled distance from neighbour point of view
    //             travelledDistanceMap.Add(n, g);
    //             float f = g + h;
    //             openSet.Enqueue(n, f);
    //         }
    //         current = openSet.Dequeue();
    //     }
    //     if(current != targetNode) return null;
    //     return ReconstructPath(predecessorMap, current);
    // }

    

    public static async Task AStarSearch(NavigationNode current, NavigationNode target, Action<NavigationNode, PathState.State> changeState)//or changeDrawState
    {
        if(current is null || target is null) return;
        var travelledDistanceMap = new Dictionary<NavigationNode, float>();//<cell,distance>
        var predecessorMap = new Dictionary<NavigationNode, NavigationNode>();//<to, from>
        var closedSet = new HashSet<NavigationNode>();
        var openSet = new PriorityQueue<NavigationNode, float>();//TPriority <= f(n) = h(n) + g(n)

        if(!current.IsWalkable || !target.IsWalkable) return;

        changeState(current, PathState.State.Current);// nodeStateMap[current] = PathState.State.Current;
        changeState(target, PathState.State.Target);// nodeStateMap[target] = PathState.State.Target;
        
        openSet.Enqueue(current,0f);
        predecessorMap.Add(current, null);
        travelledDistanceMap.Add(current, 0);
        while(openSet.Count != 0 && current != target)//current != target
        {
            current = openSet.Dequeue();
            changeState(current, PathState.State.Current);//nodeStateMap[current] = PathState.State.Current;
            //if(closedSet.Contains(current)) continue;
            await Task.Delay(delay);
            float currentDistance = travelledDistanceMap[current];
            foreach (NavigationNode n in current.Neighbours)
            {

                if(closedSet.Contains(n) || !n.IsWalkable || travelledDistanceMap.ContainsKey(n)) continue;
                if(!predecessorMap.ContainsKey(n)) predecessorMap.Add(n, current);//??????
                
                float distanceDifference = Vector2.Distance(n.Position, current.Position);
                float h = CostFunctions.EuclideanHeuristic(current, target);
                float g = currentDistance + distanceDifference; //travelled distance from neighbour point of view
                travelledDistanceMap.Add(n, g);
                float f = g + h;//
                openSet.Enqueue(n, f);
                if(n != target) //prevent from changing target cell color
                    changeState(n, PathState.State.Open);//nodeStateMap[n] = PathState.State.Open;
                
            }
            closedSet.Add(current);
            changeState(current, PathState.State.Closed);//nodeStateMap[current] = PathState.State.Closed;
            
            //current = openSet.Dequeue();//BLAD (np. po sprawdzeniu wszystkich nodów)

        }
        if(current != target) return;

        var path = ReconstructPath(predecessorMap, current);
        while(path.Count != 0)
        {
            NavigationNode node = path.Pop();
            changeState(node, PathState.State.Result);//nodeStateMap[node] = PathState.State.Result;
        }
    }

    public static async Task AStarSearchCancellable(NavigationNode current, NavigationNode target, Action<NavigationNode, PathState.State> changeState, CancellationToken ct)//or changeDrawState
    {
        if(current is null || target is null) return;
        var travelledDistanceMap = new Dictionary<NavigationNode, float>();//<cell,distance>
        var predecessorMap = new Dictionary<NavigationNode, NavigationNode>();//<to, from>
        var closedSet = new HashSet<NavigationNode>();
        var openSet = new PriorityQueue<NavigationNode, float>();//TPriority <= f(n) = h(n) + g(n)

        if(!current.IsWalkable || !target.IsWalkable) return;

        changeState(current, PathState.State.Current);// nodeStateMap[current] = PathState.State.Current;
        changeState(target, PathState.State.Target);// nodeStateMap[target] = PathState.State.Target;

        openSet.Enqueue(current,0f);
        predecessorMap.Add(current, null);
        travelledDistanceMap.Add(current, 0);
        while(openSet.Count != 0 && current != target)//current != target
        {
            if(ct.IsCancellationRequested)
            {
                ct.ThrowIfCancellationRequested();
            } 
            current = openSet.Dequeue();
            changeState(current, PathState.State.Current);//nodeStateMap[current] = PathState.State.Current;
            //if(closedSet.Contains(current)) continue;
            await Task.Delay(delay);
            float currentDistance = travelledDistanceMap[current];
            foreach (NavigationNode n in current.Neighbours)
            {

                if(closedSet.Contains(n) || !n.IsWalkable || travelledDistanceMap.ContainsKey(n)) continue;
                if(!predecessorMap.ContainsKey(n)) predecessorMap.Add(n, current);//??????
                
                float distanceDifference = Vector2.Distance(n.Position, current.Position);
                float h = CostFunctions.EuclideanHeuristic(current, target);
                float g = currentDistance + distanceDifference; //travelled distance from neighbour point of view
                travelledDistanceMap.Add(n, g);
                float f = g + h;//
                openSet.Enqueue(n, f);
                if(n != target) //prevent from changing target cell color
                {
                    if(ct.IsCancellationRequested)
                    {
                        ct.ThrowIfCancellationRequested();
                    } 
                    changeState(n, PathState.State.Open);//nodeStateMap[n] = PathState.State.Open;
                }
                
            }
            closedSet.Add(current);
            if(ct.IsCancellationRequested)
            {
                ct.ThrowIfCancellationRequested();
            } 
            changeState(current, PathState.State.Closed);//nodeStateMap[current] = PathState.State.Closed;
            
            //current = openSet.Dequeue();//BLAD (np. po sprawdzeniu wszystkich nodów)

        }
        if(current != target) return;

        var path = ReconstructPath(predecessorMap, current);
        while(path.Count != 0)
        {
            NavigationNode node = path.Pop();
            changeState(node, PathState.State.Result);//nodeStateMap[node] = PathState.State.Result;
        }
    }

    public static void DijkstrasAlgorithm()
    {}
    public static void DepthFirstSearch()//DFS
    {}
    public static void BreadthFirstSearch()//BFS
    {}
    // public static void AStar(NavGrid grid, ref PathState.State state)
    // {
    //     return;//yielding state for drawing? or new method for drawing
    // }
}