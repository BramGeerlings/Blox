using UnityEngine;
using System.Collections;

public struct TileData{
    //This struct is used so I can store multiple values in one Dictionary key.
    public Vector2 tilePosition;
    public GameObject tileGameObject;
    public bool IsAccessible;
}
