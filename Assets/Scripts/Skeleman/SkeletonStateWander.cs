using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class SkeletonStateManager : MonoBehaviour
{
    //TODO: Hunt Timer (Will decrease the time needed based on how well the player is doing
    //TODO: The skeleman will occationally look over the upper railing to the living room (should play with shadows to make it apparent hes there) (railing state)
    //TODO: Skeletons Move speed (will increase based on on how well the player is doing)  

    private enum WanderState {startWander, wander, inRoomSearch, hidingSpotSearch};
    
    [Header("Wander")]
    [SerializeField] private WanderState wanderState;
    [Tooltip("Number of rooms before allowed to return to a room")]
    [SerializeField] private int previousRoomCap;
    [Tooltip("Percentage chance of searching a room")]
    [Range(0, 100)]
    [SerializeField] private int roomSearchChance;

    //Debug
    public string currentSelectedRoom;
    public List<string> prevRoomNames = new List<string>();


    private void UpdateWander()
    {

        switch (wanderState)
        {
            //Set destination to wander to after idle time finishes
            case WanderState.startWander:
               
                if (timerFinished)
                {
                    StartWander();
                }

                break;

            //Actions upon Arival of destination, determine if searching the room
            case WanderState.wander:

                if (skeletonMovement.HasArrived())
                {
                    Wander();
                }

                break;

            //Actions taking place when searching a room
            case WanderState.inRoomSearch:

                if (timerFinished)
                {
                    WanderRoomSearch();
                }

                break;


            case WanderState.hidingSpotSearch:

                if (skeleton.GetComponent<SkeletonMovement>().HasArrived())
                {
                    WanderHidingSpotSearch();
                }

                break;
        }
    }


    /// <summary>
    /// Start Wander state
    /// </summary>
    private void StartWander()
    {       
        //Grab random room
        Room randRoom = RoomController.instance.GetRandomRoom(previousRooms);

        //Update Previous Rooms
        previousRooms.Add(randRoom);
        prevRoomNames.Add(randRoom.name); //Debug

        if (previousRooms.Count > previousRoomCap)
        {
            previousRooms.RemoveAt(0);
            prevRoomNames.RemoveAt(0); //Debug
        }

        //Set target to random spot
        int randSpot = Random.Range(0, randRoom.WanderSpots.Count);
        skeletonMovement.SetTarget(randRoom.WanderSpots[randSpot].position);

        //Update state
        wanderState = WanderState.wander;
        Debug.Log("Skeleton: WanderState change to wander");
        Debug.Log("Skeleton: Wander target set to " + RoomController.instance.GetRoom(randRoom.WanderSpots[randSpot].position));        
    }


    /// <summary>
    /// Wander state
    /// </summary>
    private void Wander()
    {                   
        //Determine if searching room
        if (roomSearchChance / (int)Random.Range(0f, 100f) >= 1.00f)
        {
            //Change State
            wanderState = WanderState.inRoomSearch;
            Debug.Log("Skeleton: WanderState change to InRoomSearch");
        }

        else
        {
            //Change State
            wanderState = WanderState.startWander;
            Debug.Log("Skeleton: WanderState change to startWander");
        }

        //Small pause before search
        StartCoroutine(Wait(wanderIdleRoomTimer)); //Look around room animation        
    }


    /// <summary>
    /// Search room state
    /// </summary>
    private void WanderRoomSearch()
    {
        //Determine hiding spots
        if (RoomController.instance.SkeletonRoom != null)
        {
            HidingSpot hidingSpot = RoomController.instance.SkeletonRoom.GetRandomHidingSpotWithOutPlayer();

            if (hidingSpot != null)
            {
                skeletonMovement.SetTarget(hidingSpot.GetSkeletonSearchSpot().position);
                
                //Chnage State
                wanderState = WanderState.hidingSpotSearch;
                Debug.Log("Skeleton: WanderState change to HidingSpotSearch");
            }
        }

        wanderState = WanderState.startWander;   
    }


    /// <summary>
    /// Search hiding spot search
    /// </summary>
    private void WanderHidingSpotSearch()
    {
        //TODO: if player then they lose
        //if (playerFoundHiding)
        //{
        //    Debug.Log("You Were caught");
        //}

        //TODO: This might need to move to a different state after searching
        wanderState = WanderState.startWander;
        Debug.Log("Skeleton: WanderState change to StartWander");
        StartCoroutine(Wait(searchHidingSpotTimer));
    }
}
