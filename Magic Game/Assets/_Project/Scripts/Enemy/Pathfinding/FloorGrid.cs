using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FloorGrid : MonoBehaviour
{

    public bool bDisplayGridGizmos;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public TerrainType[] walkableTerrains;
    public int obstacleProximityPenalty = 10;
    Dictionary<int, int> walkableTerrainsDictionary = new Dictionary<int, int>();
    LayerMask walkableMask;

    Node[,] grid;

    Vector3 gridCenter;
    Bounds gameArea = new Bounds();

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    int penaltyMin = int.MaxValue;
    int penaltyMax = int.MinValue;

    Renderer myRenderer;
    Renderer myRenderer2;

    GameObject[] floors;

    Vector3 minGrid, maxGrid, offset;

    void Awake()
    {

        //Get list of floor areas
        if (floors == null)
        {
            floors = GameObject.FindGameObjectsWithTag("Floor");
            myRenderer = floors[0].GetComponent<Renderer>();

            minGrid = myRenderer.bounds.min;
            maxGrid = myRenderer.bounds.max;
        }

        for (int i = 0; i < floors.Length - 1; i++)
        {

            myRenderer2 = floors[i + 1].GetComponent<Renderer>();

            if (myRenderer2.bounds.min.x < minGrid.x)
            {
                minGrid.x = myRenderer2.bounds.min.x;
            }
            if (myRenderer2.bounds.min.z < minGrid.z)
            {
                minGrid.z = myRenderer2.bounds.min.z;
            }
            if (myRenderer2.bounds.max.x > maxGrid.x)
            {
                maxGrid.x = myRenderer2.bounds.max.x;
            }
            if (myRenderer2.bounds.max.z > maxGrid.z)
            {
                maxGrid.z = myRenderer2.bounds.max.z;
            }

        }

        //set new bounds for the game area
        gameArea.SetMinMax(minGrid, maxGrid);

        //get size of the floor
        gridWorldSize.x = gameArea.size.x;
        gridWorldSize.y = gameArea.size.z;

        nodeDiameter = nodeRadius * 2;


        //set grid size
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);


        foreach (TerrainType region in walkableTerrains)
        {
            walkableMask.value |= region.terrainMask.value;
            walkableTerrainsDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
        }

        //get the offset of origo and grid center 
        offset = transform.position - gameArea.center;

        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];

        //get "bottom" left corner of the play area
        Vector3 worldBottomLeft = gameArea.center - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                //TODO: ignore position with no floor

                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

                int movementPenalty = 0;

                Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, walkableMask))
                {
                    walkableTerrainsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                }
                else
                {
                    walkable = false;
                }

                if (!walkable)
                {
                    movementPenalty += obstacleProximityPenalty;
                }


                grid[x, y] = new Node(walkable, worldPoint, x, y, movementPenalty);
            }
        }

        BlurPenaltyMap(3);

    }

    // Add penalties to the node depending on its position and type
    void BlurPenaltyMap(int blurSize)
    {
        int kernelSize = blurSize * 2 + 1;
        int kernelExtents = (kernelSize - 1) / 2;

        int[,] penaltiesHorizontalPass = new int[gridSizeX, gridSizeY];
        int[,] penaltiesVerticalPass = new int[gridSizeX, gridSizeY];

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = -kernelExtents; x <= kernelExtents; x++)
            {
                int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                penaltiesHorizontalPass[0, y] += grid[sampleX, y].movementPenalty;
            }

            for (int x = 1; x < gridSizeX; x++)
            {
                int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
                int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX - 1);

                penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] - grid[removeIndex, y].movementPenalty + grid[addIndex, y].movementPenalty;
            }
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = -kernelExtents; y <= kernelExtents; y++)
            {
                int sampleY = Mathf.Clamp(y, 0, kernelExtents);
                penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
            }

            int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (kernelSize * kernelSize));
            grid[x, 0].movementPenalty = blurredPenalty;

            for (int y = 1; y < gridSizeY; y++)
            {
                int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);
                int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY - 1);

                penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y - 1] - penaltiesHorizontalPass[x, removeIndex] + penaltiesHorizontalPass[x, addIndex];
                blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernelSize * kernelSize));
                grid[x, y].movementPenalty = blurredPenalty;

                if (blurredPenalty > penaltyMax)
                {
                    penaltyMax = blurredPenalty;
                }
                if (blurredPenalty < penaltyMin)
                {
                    penaltyMin = blurredPenalty;
                }
            }
        }
    }

    public List<Node> GetNeighbouringNodes(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }


    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + offset.x + gridWorldSize.x / 2) / gridWorldSize.x;

        float percentY = (worldPosition.z + offset.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(gameArea.center, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (grid != null && bDisplayGridGizmos)
        {
            foreach (Node n in grid)
            {

                Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, n.movementPenalty));
                Gizmos.color = (n.bWalkable) ? Gizmos.color : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter));
            }
        }
    }

    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }
}

