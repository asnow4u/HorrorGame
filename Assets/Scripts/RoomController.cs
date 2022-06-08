using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public static RoomController instance;
    public enum RoomType { LivingRoom, Kitchen, Bathroom, Garage, Pantry, MasterBedroom, Office, UpstairsBathroom, Bedroom, None };
    
    private List<Room> rooms;

    private Room playerCurRoom;
    private Room skeletonCurRoom;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    private void Start()
    {
        rooms = new List<Room>();

        foreach (Transform child in transform)
        {
            Room room;
            if (child.TryGetComponent<Room>(out room))
            {
                rooms.Add(room);
            }
        }    
    }


    #region Getter / Setter

    public Room PlayerRoom
    {
        get { return playerCurRoom; }
        set { playerCurRoom = value; }
    }

    public Room SkeletonRoom
    {
        get { return skeletonCurRoom; }
        set { skeletonCurRoom = value; }
    }

    public List<Room> Rooms
    {
        get { return rooms; }
    }

    #endregion


    public Room GetRoom(Vector3 pos)
    {
        foreach(Room room in rooms)
        {
            if (room.Collider.bounds.Contains(pos))
            {
                return room;
            }
        }

        return null;
    }
}
