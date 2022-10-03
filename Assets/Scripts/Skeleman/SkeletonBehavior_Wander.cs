using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class SkeletonBehavior : MonoBehaviour
{
    /*
     * wander: Skeleman will start to wander (this will start with a resurection happening when the player is close)
     * (no more teleports at this point)
     * Slow walk walking through the house
     * If the skeleman sees you for an extended period of time it will begin a hunt (need a timer for that)
     * The skeleman will occationally look over the upper railing to the living room (should play with shadows to make it apparent hes there)
     * Need:
     * Timer (Will decrease the time needed based on how well the player is doing
     * Move speed (will increase based on on how well the player is doing)   
    */
    private enum WanderState {startWander, wander, inRoomSearch, hidingSpotSearch};
    [SerializeField] private WanderState wanderState;
    [SerializeField] private int previousRoomCap;

    private Transform selectedHidingSpot;
    private bool playerFoundHiding;

    //Testing
    public string currentSelectedRoom;
    public List<string> prevRoomNames = new List<string>();

    #region Getter/Setter
    public bool PlayerFoundHiding
    {
        set { playerFoundHiding = value; }
    }
    #endregion


    private void UpdateWander()
    {

        switch (wanderState)
        {
            //Set destination to wander to after idle time finishes
            case WanderState.startWander:

                if (timerFinished) 
                    SetNextWanderRoom();

                break;


            //Actions upon Arival of destination, determine if searching the room
            case WanderState.wander:
                
                if (skeleton.GetComponent<SkeletonMovement>().HasArrived())
                {
                
                    int rand = (int)Random.Range(0f, 100f); //TODO: For hunts there should be a higher chance to search the room

                    //TODO: Base this off of key percentage
                    //Searching Hiding Spot
                    //if (keyPercent * 100f >= rand)
                    //{

                    //Change State
                    wanderState = WanderState.inRoomSearch;


                    //Not Searching Hiding Spot
                    //    else
                    //    {
                    //wanderState = WanderState.startWander;
                    //    }

                    //Small pause before search
                    StartCoroutine(Wait(wanderIdleRoomTimer)); //Look around room animation
                }

                break;


            //Actions taking place when searching a room
            case WanderState.inRoomSearch:
                
                if (timerFinished)
                {

                    //Determine hiding spots
                    HidingSpot hidingSpot = SelectHidingSpot();
                   
                    if (hidingSpot == null)
                    {
                        wanderState = WanderState.startWander;
                        return;
                    }

                    skeleton.GetComponent<SkeletonMovement>().SetTarget(hidingSpot.GetSkeletonSearchSpot().position);
                    wanderState = WanderState.hidingSpotSearch;
                }    

                break;


            case WanderState.hidingSpotSearch:

                if (skeleton.GetComponent<SkeletonMovement>().HasArrived())
                {

                    //TODO: if player then they lose
                    //if (playerFoundHiding)
                    //{
                    //    Debug.Log("You Were caught");
                    //}

                    //TODO: This would move to a different state after searching
                    wanderState = WanderState.startWander;
                    StartCoroutine(Wait(searchHidingSpotTimer));
                }

                break;
        }
    }


    /// <summary>
    /// Determine which hiding spot to search
    /// </summary>
    private HidingSpot SelectHidingSpot()
    {
        //Check if in a searchable room
        if (RoomController.instance.SkeletonRoom == null)
        {
            Debug.LogWarning("Skeleton room was null when selecting hiding spot");
            return null;
        }

        List<HidingSpot> hidingSpots =  new List<HidingSpot>(RoomController.instance.SkeletonRoom.HidingSpots);
        
        //No hiding spots, wait idle
        if (hidingSpots.Count == 0)
        {
            Debug.LogWarning("Room has no hiding spots when selecting hiding spot");
            return null;
        }

        //One hiding spot
        if (hidingSpots.Count == 1)
        {
            //Check the hiding spot
            return hidingSpots[0];
        }

        //Two or more hiding spots
        List<HidingSpot> availbleSpots = new List<HidingSpot>();
        foreach (HidingSpot hidingSpot in hidingSpots)
        {
            //Check if player is hiding
            if (hidingSpot.GetComponent<HidingSpot>().IsPlayerHiding) continue;
            
            availbleSpots.Add(hidingSpot);
        }

        int rand = Random.Range(0, availbleSpots.Count);
        
        return hidingSpots[rand];
    }


    /// <summary>
    /// Set up destination target to a room no previously visited
    /// </summary>
    public void SetNextWanderRoom()
    {
        //Randomize Room
        List<Room> availableRooms = new List<Room>();
        
        foreach (Room room in RoomController.instance.Rooms)
        {
            //Room not available
            if (previousRooms.Contains(room)) continue;

            availableRooms.Add(room);
        }

        int rand = Random.Range(0, availableRooms.Count);

        //Previous room
        previousRooms.Add(availableRooms[rand]);
        prevRoomNames.Add(availableRooms[rand].name); //TODO: Remove 

        if (previousRooms.Count > previousRoomCap)
        {
            previousRooms.RemoveAt(0);

            prevRoomNames.RemoveAt(0); //TODO: Remove
        }

        //Randomize Spot
        List<Transform> availableSpots = new List<Transform>();

        foreach (Transform transform in availableRooms[rand].WanderSpots)
        {
            //Add available spot
            availableSpots.Add(transform);
        }

        rand = Random.Range(0, availableSpots.Count);

        //Set target
        skeleton.GetComponent<SkeletonMovement>().SetTarget(availableSpots[rand].position);
        wanderState = WanderState.wander;
    }
}
