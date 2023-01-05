using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class SkeletonStateManager : MonoBehaviour
{    
    private void UpdateDormant()
    {
        if (!timerFinished) return;
        if (PlayerController.instance.InViewOfCamera(skeleton.transform.position)) return;

        Room playerRoom = RoomController.instance.PlayerRoom;
        Room skeletonRoom = RoomController.instance.SkeletonRoom;
        
        if (skeletonRoom != null && playerRoom != null && playerRoom.Type == skeletonRoom.Type) return;

        List<Transform> availbleSpots = new List<Transform>();

        //Determine all available spots
        foreach (Room room in RoomController.instance.Rooms)
        {
            //Spot not available
            if (skeletonRoom != null && room.Type == skeletonRoom.Type) continue;
            if (playerRoom != null && room.Type == playerRoom.Type) continue;

            foreach (Transform transform in room.DormantSpots)
            {
                //Spot not in view of player
                if (!PlayerController.instance.InViewOfCamera(transform.position))
                {
                    //Add available spot
                    availbleSpots.Add(transform);
                }
            }
        }
        
        if (availbleSpots.Count == 0) return;

        int rand = Random.Range(0, availbleSpots.Count);

        skeleton.transform.position = availbleSpots[rand].position;
        //TODO: Rotation
        //skeleton.transform.rotation = availbleSpots[rand].rotation;

        Debug.Log("Skeleton: Dormant location shift to " + RoomController.instance.GetRoom(skeleton.transform.position));

        StartCoroutine(Wait(dormantTimer));
    }
}
