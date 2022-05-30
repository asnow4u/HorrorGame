using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public static RoomController instance;
    public enum RoomType { LivingRoom, Kitchen, Bathroom, Garage, Pantry, MasterBedroom, Office, UpstairsBathroom, Bedroom, None };

    private RoomType playerCurRoom;
    private RoomType skeletonCurRoom;

    private List<BoxCollider> colliders;


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

    // Start is called before the first frame update
    void Start()
    {
        colliders = new List<BoxCollider>();

        foreach (Transform room in transform)
        {
            BoxCollider collider = room.GetComponent<BoxCollider>();
            colliders.Add(collider);
        }
    }

    
    public RoomType PlayerRoom
    {
        get { return playerCurRoom; }
        set { playerCurRoom = value; }
    }


    public RoomType SkeletonRoom
    {
        get { return skeletonCurRoom; }
        set { skeletonCurRoom = value; }
    }


    public RoomType GetRoom(Vector3 pos)
    {
        foreach(Transform room in transform)
        {
            Collider collider = room.GetComponent<BoxCollider>();
            if (collider.bounds.Contains(pos))
            {
                return room.GetComponent<Room>().GetRoom();
            }
        }

        return RoomType.None;
    }
}
