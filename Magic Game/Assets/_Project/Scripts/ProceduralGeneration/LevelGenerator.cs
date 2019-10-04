using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    #region VARIABLES

    public List<Segment> segmentPrefabs = new List<Segment>();
    public Vector2 segmentCount = new Vector2();

    private Segment firstSegment = null;
    private List<Segment> placedSegments = new List<Segment>();
    private List<Doorway> availableDoorways = new List<Doorway>();
    private LayerMask layerMask = 0;

    #endregion

    #region UNITY_FUNCTIONS

    private void Start()
    {
        layerMask = LayerMask.GetMask("Segment");
        StartCoroutine("GenerateLevel");
    }

    #endregion

    #region CUSTOM_FUNCTIONS

    private IEnumerator GenerateLevel()
    {
        WaitForFixedUpdate interval = new WaitForFixedUpdate();

        PlaceFirstSegment();
        yield return interval;

        int iterations = Random.Range((int)segmentCount.x, (int)segmentCount.y);

        for (int i = 0; i < iterations; i++)
        {
            //Place random segment from list of prefabs
            PlaceSegment();
            yield return interval;
        }

        FinalCheckForOverlaps();

        Debug.Log("Level generation finished");
    }

    private void PlaceFirstSegment()
    {
        firstSegment = Instantiate(segmentPrefabs[Random.Range(0, segmentPrefabs.Count)]) as Segment;
        firstSegment.transform.parent = transform;

        //Get doorways from current segment and add randomly to the list of available doorways
        AddDoorwaysToList(firstSegment, ref availableDoorways);

        firstSegment.transform.position = Vector3.zero;
        firstSegment.transform.rotation = Quaternion.identity;
    }

    private void AddDoorwaysToList(Segment segment, ref List<Doorway> list)
    {
        foreach (Doorway doorway in segment.doorways)
        {
            int r = Random.Range(0, list.Count);
            list.Insert(r, doorway);
        }
    }

    private void PlaceSegment()
    {
        Segment currentSegment = Instantiate(segmentPrefabs[Random.Range(0, segmentPrefabs.Count)]) as Segment;
        currentSegment.transform.parent = transform;

        //Create doorway lists to loop over
        List<Doorway> allAvailableDoorways = new List<Doorway>(availableDoorways);
        List<Doorway> currentSegmentDoorways = new List<Doorway>();
        AddDoorwaysToList(currentSegment, ref currentSegmentDoorways);

        //Get doorways from current segment and add randomly to the list of available doorways
        AddDoorwaysToList(currentSegment, ref availableDoorways);

        bool segmentPlaced = false;

        //Try all available doorways
        foreach (Doorway availableDoorway in allAvailableDoorways)
        {
            //Try all available doorways in current segment
            foreach (Doorway currentDoorway in currentSegmentDoorways)
            {
                //Position segment
                PositionSegmentAtDoorway(ref currentSegment, currentDoorway, availableDoorway);

                //Check segment overlaps
                if (CheckSegmentOverlap())
                {
                    continue;
                }

                segmentPlaced = true;

                //Add segment to the list of placed segments
                placedSegments.Add(currentSegment);

                //Remove occupied doorways
                currentDoorway.gameObject.SetActive(false);
                availableDoorways.Remove(currentDoorway);

                availableDoorway.gameObject.SetActive(false);
                availableDoorways.Remove(availableDoorway);

                //Exit loop if segment has been placed
                break;
            }

            //Exit loop if segment has been placed
            if (segmentPlaced)
            {
                break;
            }
        }

        //Segment couldn't be placed. Restart generator and try again (think some better way to do this?)
        if (!segmentPlaced)
        {
            Debug.Log("Restarting generator");
            Destroy(currentSegment.gameObject);
            ResetLevelGenerator();
        }
    }

    private void PositionSegmentAtDoorway(ref Segment segment, Doorway segmentDoorway, Doorway targetDoorway)
    {
        //Reset segment position/rotation
        segment.transform.position = Vector3.zero;
        segment.transform.rotation = Quaternion.identity;

        //Rotate segment to match previous doorway orientation
        Vector3 targetDoorwayEuler = targetDoorway.transform.eulerAngles;
        Vector3 segmentDoorwayEuler = segmentDoorway.transform.eulerAngles;
        float deltaAngle = Mathf.DeltaAngle(segmentDoorwayEuler.y, targetDoorwayEuler.y);
        Quaternion currentSegmentTargetRotation = Quaternion.AngleAxis(deltaAngle, Vector3.up);
        segment.transform.rotation = currentSegmentTargetRotation * Quaternion.Euler(0, 180f, 0);

        //Position segment
        Vector3 segmentPositionOffset = segmentDoorway.transform.position - segment.transform.position;
        segment.transform.position = targetDoorway.transform.position - segmentPositionOffset;
    }

    private bool CheckSegmentOverlap()
    {
        for (int i = 0; i < placedSegments.Count; i++)
        {
            Segment segment = placedSegments[i];
            Bounds bounds = segment.RoomBounds;
            bounds.Expand(-0.1f);

            Collider[] colliders = Physics.OverlapBox(bounds.center, bounds.size / 2, segment.transform.rotation, layerMask);

            if (colliders.Length > 0)
            {
                //Ignore collisions with current segment
                foreach (Collider c in colliders)
                {
                    if (c.transform.parent.gameObject.Equals(segment.gameObject))
                    {
                        continue;
                    }

                    else
                    {
                        Debug.LogError("Overlap detected");
                        return true;
                    }
                }
            }
        }
        
        return false;
    }

    private void ResetLevelGenerator()
    {
        Debug.LogError("Reset level generator");
        StopCoroutine("GenerateLevel");

        //Delete all segments (think other way for not resetting everything?)
        if (firstSegment)
        {
            Destroy(firstSegment.gameObject);
        }

        foreach (Segment segment in placedSegments)
        {
            Destroy(segment.gameObject);
        }

        //Clear lists
        placedSegments.Clear();
        availableDoorways.Clear();

        //Reset coroutine
        StartCoroutine("GenerateLevel");
    }

    private void FinalCheckForOverlaps()
    {
        for (int i = 0; i < placedSegments.Count; i++)
        {
            Segment segment = placedSegments[i];
            Bounds bounds = segment.RoomBounds;
            bounds.Expand(-0.1f);

            Collider[] colliders = Physics.OverlapBox(bounds.center, bounds.size / 2, segment.transform.rotation, layerMask);

            if (colliders.Length > 0)
            {
                //Ignore collisions with current room
                foreach (Collider c in colliders)
                {
                    if (c.transform.parent.gameObject.Equals(segment.gameObject))
                    {
                        continue;
                    }

                    else
                    {
                        Debug.LogError("FUCK YOU OVERLAP");
                        ResetLevelGenerator();
                    }
                }
            }
        }
    }

    #endregion
}