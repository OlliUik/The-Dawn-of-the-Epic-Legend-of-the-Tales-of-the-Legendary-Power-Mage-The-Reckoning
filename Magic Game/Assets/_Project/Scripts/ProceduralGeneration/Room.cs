using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Doorway[] doorways;
    public BoxCollider boxCollider; //MeshCollider meshCollider

    public Bounds RoomBounds
    {
        get
        {
            return boxCollider.bounds; //return meshCollider.bounds
        }
    }
}