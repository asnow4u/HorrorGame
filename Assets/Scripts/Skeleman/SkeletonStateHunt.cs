using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class SkeletonStateManager : MonoBehaviour
{
    //TODO: Skeleman will do something to indicate a hunt. A sound cue / effect will also occure.

    [Header("Hunt")]
    [SerializeField] private float huntTimeMin;
    [SerializeField] private float huntTimeMax;
    [SerializeField] private float huntRoomMin;
    [SerializeField] private float huntRoomMax;

    private enum HuntState {startHunt, hunt, inTransitHunt, inRoomSearch, hidingSpotSearch, endHunt }
    [SerializeField] private HuntState huntState;
    
    public float huntTimer;

    private List<Transform> huntSpots;
    private int huntSpotCount;
    private HidingSpot huntHidingSpot;

    private void UpdateHunt()
    {           
        switch (huntState)
        {
            case HuntState.startHunt:
                StartHunt();
                break;


            case HuntState.hunt:

                if (timerFinished)
                {
                    Hunt();
                }

                break;


            case HuntState.inTransitHunt:

                if (skeleton.GetComponent<SkeletonMovement>().HasArrived())
                {
                    HuntInTransit();
                }

                break;


            case HuntState.inRoomSearch:

                if (timerFinished)
                {
                    HuntSearchRoom();
                }

                break;


            case HuntState.hidingSpotSearch:

                if (skeleton.GetComponent<SkeletonMovement>().HasArrived())
                {
                    HuntSearchHidingSpot();                    
                }

                break;


            case HuntState.endHunt:

                if (timerFinished)
                {
                    EndHunt();
                }

                break;
        }
     
    }


    /// <summary>
    /// Start hunt state
    /// </summary>
    public void StartHunt()
    {
        huntSpotCount = 0;
        huntSpots = SelectRoomsToHunt();

        //Hunt effect
        StartCoroutine(Wait(0.2f));

        //Lighting effect
        LightManager.instance.ChangeToHunt();

        huntState = HuntState.hunt;
    }


    /// <summary>
    /// Hunt state
    /// </summary>
    public void Hunt()
    {
        skeletonMovement.SetTarget(huntSpots[huntSpotCount].position);
        Debug.Log("Hunt: Location set to " + RoomController.instance.GetRoom(huntSpots[huntSpotCount].position) + " (huntSpotCount of " + huntSpots.Count + ")");
        
        huntSpotCount++;
        huntState = HuntState.inTransitHunt;
        Debug.Log("Skeleton: HuntState change to InTransitHunt");
    }


    /// <summary>
    /// In transit state
    /// </summary>
    public void HuntInTransit()
    {
        //Continue hunting (not searching room)
        if (huntSpotCount < huntSpots.Count)
        {
            huntState = HuntState.hunt;
            Debug.Log("Skeleton: HuntState change to Hunt");
        }

        //Reached final room
        else
        {
            huntState = HuntState.inRoomSearch;
            Debug.Log("Skeleton: HuntState change to InRoomSearch");
        }

        StartCoroutine(Wait(wanderIdleRoomTimer));
    }


    /// <summary>
    /// Searching room state
    /// </summary>
    public void HuntSearchRoom()
    {
        if (RoomController.instance.SkeletonRoom != null)
        {
            huntHidingSpot = RoomController.instance.SkeletonRoom.GetRandomHidingSpotWithOutPlayer();

            if (huntHidingSpot != null)
            {
                Debug.Log("Skeleton: Search hunt hiding spot " + huntHidingSpot.name);

                skeleton.GetComponent<SkeletonMovement>().SetTarget(huntHidingSpot.SkeletonSearchSpot.position);
                huntState = HuntState.hidingSpotSearch;
                Debug.Log("Skeleton: HuntState change to HidingSpotSearch");
            }
        }
    }


    /// <summary>
    /// Search hidingspot state
    /// </summary>
    public void HuntSearchHidingSpot()
    {
        //TODO: if player then they lose
        //if (playerFoundHiding)
        //{
        //    Debug.Log("You Were caught");
        //}

        //Destroy hiding spot
        RoomController.instance.SkeletonRoom.HidingSpots.Remove(huntHidingSpot);
        Debug.Log("Skeleton: Destroy Hiding Spot " + huntHidingSpot.name);
        huntHidingSpot.DestroyHidingSpot();

        huntState = HuntState.endHunt;
        Debug.Log("Skeleton: HuntState change to EndHunt");
        StartCoroutine(Wait(searchHidingSpotTimer));
    }


    /// <summary>
    /// End hunt state
    /// </summary>
    public void EndHunt()
    {
        LightManager.instance.ChangeToNormal();

        ChangeState(State.wander);
    }


    /// <summary>
    /// List of rooms to hunt
    /// </summary>
    /// <returns></returns>
    private List<Transform> SelectRoomsToHunt()
    {
        List<Transform> huntSpotsList = new List<Transform>();

        //Determine number of rooms to search
        int numRooms = (int)Random.Range(huntRoomMin, huntRoomMax);

        List<Room> availableRooms = new List<Room>(RoomController.instance.Rooms);     

        //Randomize list
        for (int i = 0; i < availableRooms.Count; i++)
        {
            Room temp = availableRooms[i];
            int randomIndex = Random.Range(i, availableRooms.Count);
            availableRooms[i] = availableRooms[randomIndex];
            availableRooms[randomIndex] = temp;
        }

        //Create list of rooms
        for (int i=0; i<availableRooms.Count; i++)
        {
            //First room cant be your room
            if (huntSpotsList.Count == 0 && availableRooms[i] == RoomController.instance.SkeletonRoom)
            {
                continue;
            }

            //Last room must have an available hidingspot if possible
            if (huntSpotsList.Count == numRooms - 1)
            {
                if (availableRooms[i].HidingSpots.Count == 0)
                {
                    if (i != availableRooms.Count - 1)
                    {
                        continue;
                    }
                }
            }

            int rand = Random.Range(0, availableRooms[i].WanderSpots.Count);
            huntSpotsList.Add(availableRooms[i].WanderSpots[rand]);
        }

        return huntSpotsList;
    }


    private void CheckHuntTimer()
    {
        if (curState == State.wander)
        {
            if (huntTimer <= 0)
            {
                ChangeState(State.hunt);
                huntTimer = Random.Range(huntTimeMin, huntTimeMax);
            }

            huntTimer -= Time.deltaTime;
        }
    }
}
