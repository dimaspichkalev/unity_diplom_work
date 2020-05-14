using UnityEngine;
using System.Collections;

public class Room 
{
    public Wall[] Walls { get; set; } = new Wall[4];

    private Vector2 position;

    public bool HasRoof { get; set; }

    public RoomRay roomRay { get; private set; }

    public int FloorNumber { get; set; }

    public Room(Vector2 position, bool hasRoof = false, RoomRay roomRay = null)
    {
        this.position = position;
        this.HasRoof = hasRoof;
        this.roomRay = roomRay;
    }

    public Vector2 RoomPosition
    {
        get
        {
            return this.position;
        }
    }

}
