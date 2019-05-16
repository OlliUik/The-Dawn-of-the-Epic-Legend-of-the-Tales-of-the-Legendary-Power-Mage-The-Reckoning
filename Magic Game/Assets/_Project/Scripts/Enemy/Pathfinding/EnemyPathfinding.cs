using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class EnemyPathfinding : MonoBehaviour
{
    FloorGrid grid;

    private void Awake()
    {     
        grid = GetComponent<FloorGrid>();
    }

    public void FindPath(PathRequest request, Action<PathResult> callback)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        Vector3[] waypoints = new Vector3[0];
        bool bPathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(request.pathStart);
        Node targetNode = grid.NodeFromWorldPoint(request.pathEnd);

        if(startNode.bWalkable && targetNode.bWalkable)
        {
            //Great openNodes heap and add the start node to openNodes
            Heap<Node> openNodes = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedNodes = new HashSet<Node>();
            openNodes.Add(startNode);

            while (openNodes.Count > 0)
            {
                Node currentNode = openNodes.RemoveFirst();
                closedNodes.Add(currentNode);

                //check if currentNode is the targetNode
                if (currentNode == targetNode)
                {
                    //stopwatch.Stop();
                    //print("Path found in: " + stopwatch.ElapsedMilliseconds + " ms");
                    bPathSuccess = true;
                    break;
                }

                //check if the neighbours are walkable
                foreach (Node neighbour in grid.GetNeighbouringNodes(currentNode))
                {
                    if (!neighbour.bWalkable || closedNodes.Contains(neighbour))
                    {
                        continue;
                    }

                    //set new cost to the neighbouring node
                    int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;
                    if (newCostToNeighbour < neighbour.gCost || !openNodes.Contains(neighbour))
                    {
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        //add to open nodes if not added already
                        if (!openNodes.Contains(neighbour))
                        {
                            openNodes.Add(neighbour);
                        }

                        else
                        {
                            openNodes.UpdateItem(neighbour);
                        }
                    }
                }
            }
        }

        if(bPathSuccess)
        {
            waypoints = BacktrackPath(startNode, targetNode);
            bPathSuccess = waypoints.Length > 0;
        }

        callback(new PathResult(waypoints, bPathSuccess, request.callback));
    }

    // reverse path
    Vector3[] BacktrackPath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector3[] waypoints =  SimplifyPath(path);
        Array.Reverse(waypoints);

        return waypoints;
    }

    // Simplify path so that only nodes where direction changes are saved to the path
    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for(int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);

            if(directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
        }

        return waypoints.ToArray();
    }


    int GetDistance(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);


        if (distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        else
        {
            return 14 * distanceX + 10 * (distanceY - distanceX);
        }
    }
}
