using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath 
{
    public readonly Vector3[] lookPoints;
    public readonly Line[] turnBoundaries;
    public readonly int finishedLineIndex;
    public readonly int slowDownIndex;

    public EnemyPath(Vector3[] waypoints, Vector3 startPos, float turnDistance, float stoppingDistance)
    {
        lookPoints = waypoints;
        turnBoundaries = new Line[lookPoints.Length];
        finishedLineIndex = turnBoundaries.Length - 1;

        Vector2 previousPoint = Vector3ToVector2(startPos);

        for(int i = 0; i < lookPoints.Length; i++)
        {
            Vector2 currentPoint = Vector3ToVector2(lookPoints[i]);
            Vector2 directionToCurrentPoint = (currentPoint - previousPoint).normalized;
            Vector2 turnBoundaryPoint = (i==finishedLineIndex)?currentPoint : currentPoint - directionToCurrentPoint * turnDistance;

            turnBoundaries[i] = new Line(turnBoundaryPoint, previousPoint - directionToCurrentPoint * turnDistance);
            previousPoint = turnBoundaryPoint;
        }

        float distanceFromEndPoint = 0;
        for (int i = lookPoints.Length - 1; i > 0; i--)
        {
            distanceFromEndPoint += Vector3.Distance(lookPoints[i], lookPoints[i - 1]);
            if(distanceFromEndPoint > stoppingDistance)
            {
                slowDownIndex = i;
                break;
            }
        }
    }

    Vector2 Vector3ToVector2(Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.z);
    }

    public void DrawWithGizmos()
    {
        Gizmos.color = Color.black;
        foreach (Vector3 point in lookPoints)
        {
            Gizmos.DrawCube(point + Vector3.up, Vector3.one);
        }

        Gizmos.color = Color.white;
        foreach (Line line in turnBoundaries)
        {
            line.DrawWithGizmos(10);
        }
    }
}
