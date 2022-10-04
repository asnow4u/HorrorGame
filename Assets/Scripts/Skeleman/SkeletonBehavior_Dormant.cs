using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class SkeletonBehavior : MonoBehaviour
{

    private void UpdateDormant()
    {
        if (!timerFinished) return;
        if (PlayerController.instance.InViewOfCamera(skeleton.transform.position)) return;

        Room playerRoom = RoomController.instance.PlayerRoom;
        Room skeletonRoom = RoomController.instance.SkeletonRoom;

        //Prevent movement
        if (playerRoom == null) return;
        if (skeletonRoom != null && playerRoom.Type == skeletonRoom.Type) return;

        List<Transform> availbleSpots = new List<Transform>();

        foreach (Room room in RoomController.instance.Rooms)
        {
            //Spot not available
            if (skeletonRoom != null && room.Type == skeletonRoom.Type) continue;
            if (room.Type == playerRoom.Type) continue;

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

        int rand = Random.Range(0, availbleSpots.Count);

        skeleton.transform.position = availbleSpots[rand].position;
        //TODO: Rotation

        Debug.Log("Skeleton: Dormant location shit to " + RoomController.instance.GetRoom(skeleton.transform.position));

        StartCoroutine(Wait(dormantTimer));
    }
}
