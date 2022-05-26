using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    
    public enum Room { LivingRoom, Kitchen, Bathroom, Garage, Pantry, MasterBedroom, Office, UpstairsBathroom, Bedroom, None };

    public Room curRoom;
    private List<BoxCollider> colliders;


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

    
    public void SetRoom(Room room)
    {
        curRoom = room;
    }
}
