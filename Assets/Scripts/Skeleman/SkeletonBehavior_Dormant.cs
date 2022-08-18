using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class SkeletonBehavior : MonoBehaviour
{

    /*Requirements
     * Similar to slender man, the more keys that are gathered the more active the skeleman becomes
     * States of activity => dorment, observing, wander, hunt  
     * States could be specified by a sound effect (grandfather clock?)
     * 
     * dorment: the skeleman will teleport around when not seen. And will be in places like on the stairs, in the kitchen, ect.
     * In front of the stairs, on the bed, falls out of wardrobe
     * Need:
     * List of optional places to teleport
     * Timer (dont want this to occur to often)
     * User Room controller to determine where player is before moving
    */


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

        StartCoroutine(Wait(dormantTimer));
    }
}
