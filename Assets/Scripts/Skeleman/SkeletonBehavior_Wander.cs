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
    private enum WanderState {Wander, InRoomSearch, HidingSpotSearch, FinishSearch};
    [SerializeField] private WanderState wanderState;
    [SerializeField] private int previousRoomCap;

    private Transform selectedHidingSpot;
    private bool playerFoundHiding;


    #region Getter/Setter
    public bool PlayerFoundHiding
    {
        set { playerFoundHiding = value; }
    }
    #endregion


    private void UpdateWander()
    {
        //Determine if arrived at location
        //based on state do stuff

        if (skeleton.GetComponent<SkeletonMovement>().Arrived)
        {
            if (wanderState == WanderState.Wander)
            {
                //Randomize if searching room
                int rand = (int)Random.Range(0f, 100f); //TODO: For hunts there should be a higher chance to search the room
                if (keyPercent * 100f > rand)
                {

                    //Change State
                    wanderState = WanderState.InRoomSearch;

                    //Determine hiding spots
                    SelectHidingSpot();
                }

                //Small pause before search
                StartCoroutine(Wait(wanderIdleRoomTimer));
            }


            if (wanderState == WanderState.HidingSpotSearch)
            {
                
                //TODO: Might need to make a hiding spot class that has this 
                if (playerFoundHiding)
                {
                    //TODO: if player then they lose
                    Debug.Log("YOU Were caught");
                }

                wanderState = WanderState.FinishSearch;
                StartCoroutine(Wait(searchHidingSpotTimer));
            }
        }


        else if (timerFinished)
        {
            if (wanderState == WanderState.InRoomSearch)
            {
                skeleton.GetComponent<SkeletonMovement>().SetTarget(selectedHidingSpot.position); //TODO: Get a position for the skeleton to move to
                wanderState = WanderState.HidingSpotSearch;
            }

            if (wanderState == WanderState.FinishSearch)
            {
                LeaveToNextRoom();
            }
        }
    }


    private void SelectHidingSpot()
    {
        //Check if in a searchable room
        if (RoomController.instance.SkeletonRoom == null)
        {
            return;
        }

        List<HidingSpot> hidingSpots =  new List<HidingSpot>(RoomController.instance.SkeletonRoom.HidingSpots);
        
        //No hiding spots, wait idle
        if (hidingSpots.Count == 0)
        {
            return;
        }

        //One hiding spot
        if (hidingSpots.Count == 1)
        {
            //Check the hiding spot
            Debug.Log("Checking Hiding Spot: " + hidingSpots[0].name);
            selectedHidingSpot = hidingSpots[0].transform;
            return;
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
        
        Debug.Log("Checking Hiding Spot: " + hidingSpots[rand].name);
        selectedHidingSpot = hidingSpots[rand].transform;
    }


    public void LeaveToNextRoom() //TODO: Use this for hunts as well
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
        if (previousRooms.Count > previousRoomCap)
        {
            previousRooms.RemoveAt(0);
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
        wanderState = WanderState.Wander;
    }
}
