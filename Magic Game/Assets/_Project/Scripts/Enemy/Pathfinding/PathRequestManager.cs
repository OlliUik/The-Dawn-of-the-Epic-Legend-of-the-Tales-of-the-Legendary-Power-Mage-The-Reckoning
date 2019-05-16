using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathResult> results = new Queue<PathResult>();

    static PathRequestManager instance;
    EnemyPathfinding pathfinding;

    //bool isProcessingPath;

    private void Awake()
    {
        instance = this;
        pathfinding = GetComponent<EnemyPathfinding>();
    }

    private void Update()
    {
        if(results.Count > 0)
        {
            int itemsInQueue = results.Count;
            lock(results)
            {
                for (int i = 0; i < itemsInQueue; i++)
                {
                    PathResult result = results.Dequeue();
                    result.callback(result.path, result.success);
                }
            }
        }
    }

    // Request path from current position to target position
    public static void RequestPath(PathRequest request)
    {
        ThreadStart threadStart = delegate { instance.pathfinding.FindPath(request, instance.FinishedProcessingPath); };

        threadStart.Invoke();
    } 

    // Called from EnemyPathfinding after finished processing path
    public void FinishedProcessingPath(PathResult result)
    {      
        lock (results)
        {
            results.Enqueue(result);
        }
    }    
}

public struct PathResult
{
    public Vector3[] path;
    public bool success;
    public Action<Vector3[], bool> callback;

    // Constructor
    public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback)
    {
        this.path = path;
        this.success = success;
        this.callback = callback;
    }
}

public struct PathRequest
{
    public Vector3 pathStart;
    public Vector3 pathEnd;

    public Action<Vector3[], bool> callback;

    // Set path's start and end
    public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
    {
        pathStart = _start;
        pathEnd = _end;
        callback = _callback;
    }
}