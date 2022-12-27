using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public static RoomController instance;
    public enum RoomType { LivingRoom, Kitchen, Bathroom, Garage, Pantry, MasterBedroom, Office, UpstairsBathroom, Bedroom, None };
    
    [SerializeField] private List<Room> rooms;

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


    /// <summary>
    /// Return room that pos is in
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
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


    /// <summary>
    /// Return random room not included in the provided list
    /// </summary>
    /// <param name="excludeList"></param>
    /// <returns></returns>
    public Room GetRandomRoom(List<Room> excludeList)
    {
        List<Room> availableRooms = new List<Room>();

        foreach (Room room in Rooms)
        {
            if (excludeList.Contains(room)) continue;

            availableRooms.Add(room);
        }

        int rand = Random.Range(0, availableRooms.Count);

        return availableRooms[rand];
    }
}
