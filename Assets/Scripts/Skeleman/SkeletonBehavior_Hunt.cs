using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class SkeletonBehavior : MonoBehaviour
{

    /*Requirements
     
      * Hunt: Skeleman is activally hunting. 
     * Skeleman will do something to indicate a hunt. A sound cue / effect will also occure.
     * If the skeleman sees the player hell start to chase
     * Vinnete effect of somekind to indicate the skeleman is close (darker in the edges of the screen)
     * If hunt is unsucessful the skeleman will destroy one of the locations to hide (first time should not happen in the room their hiding in so they can find it (They should hear it))
     * Will run through most of the rooms, (sometimes the one their in sometimes not)
     * Need:
     * Number of rooms (length of hunt)
     * Use Room Controller to determine where the player is
     * Locations for each of the rooms and hiding spots
    */

    [Header("Hunt")]
    [SerializeField] private float huntTimeMin;
    [SerializeField] private float huntTimeMax;
    [SerializeField] private float huntRoomMin;
    [SerializeField] private float huntRoomMax;

    private enum HuntState {startHunt, hunt, inTransitHunt, inRoomSearch, hidingSpotSearch, endHunt }
    [SerializeField] private HuntState huntState;

    private float huntDuration;
    private float huntTimer;

    private List<Transform> huntSpots;
    private int huntSpotCount;
    private HidingSpot huntHidingSpot;

    #region Getter/Setter

    //TODO: When picking up a key, the hunt timer will drop a bit
    public float HuntTimer
    {
        get { return huntTimer; }
    }

    
    #endregion

    private void UpdateHunt()
    {
           
        switch (huntState)
        {
            case HuntState.startHunt:

                //TODOS:
                huntSpotCount = 0;
                huntSpots = SelectRoomsToHunt();

                Debug.Log("Hunt: Hunt list created with " + huntSpots.Count + " spots");
                foreach (Transform spot in huntSpots)
                {
                    Debug.Log(RoomController.instance.GetRoom(spot.position));
                }

                //Hunt effect
                StartCoroutine(Wait(0.2f));
                
                huntState = HuntState.hunt;

                break;


            case HuntState.hunt:

                if (timerFinished)
                {
                    Debug.Log("Hunt: Location set to " + RoomController.instance.GetRoom(huntSpots[huntSpotCount].position) + " (huntSpotCount of " + huntSpots.Count + ")");
                    skeleton.GetComponent<SkeletonMovement>().SetTarget(huntSpots[huntSpotCount].position);
                    huntSpotCount++;
                    huntState = HuntState.inTransitHunt;
                }

                break;


            case HuntState.inTransitHunt:

                if (skeleton.GetComponent<SkeletonMovement>().HasArrived())
                {
                    
                    //Continue hunting (not searching room)
                    if (huntSpotCount < huntSpots.Count)
                    {
                        huntState = HuntState.hunt;
                    }

                    //Reached final room
                    else
                    {
                        huntState = HuntState.inRoomSearch;
                    }
                    
                    StartCoroutine(Wait(wanderIdleRoomTimer));
                }

                break;


            case HuntState.inRoomSearch:

                if (timerFinished)
                {
                    huntHidingSpot = SelectHidingSpot();

                    Debug.Log("Hunt: Search hiding spot " + huntHidingSpot.name);

                    skeleton.GetComponent<SkeletonMovement>().SetTarget(huntHidingSpot.GetSkeletonSearchSpot().position);
                    huntState = HuntState.hidingSpotSearch;
                }

                break;


            case HuntState.hidingSpotSearch:

                if (skeleton.GetComponent<SkeletonMovement>().HasArrived())
                {
                    //TODO: if player then they lose
                    //if (playerFoundHiding)
                    //{
                    //    Debug.Log("You Were caught");
                    //}

                    //Destroy hiding spot
                    RoomController.instance.SkeletonRoom.HidingSpots.Remove(huntHidingSpot);
                    Debug.Log("Hunt: Destroy Hiding spot " + huntHidingSpot.name);
                    huntHidingSpot.DestroyHidingSpot();


                    huntState = HuntState.endHunt;
                    StartCoroutine(Wait(searchHidingSpotTimer));
                }

                break;


            case HuntState.endHunt:

                if (timerFinished)
                {
                    state = State.wander;
                    wanderState = WanderState.startWander;

                    //TODO: Reset hunt timer
                }

                break;
        }
     
    }


    public List<Transform> SelectRoomsToHunt()
    {
        List<Transform> huntSpotsList = new List<Transform>();

        //Determine number of rooms to search
        int numRooms = (int)Random.Range(huntRoomMin, huntRoomMax);

        List<Room> availableRooms = new List<Room>(RoomController.instance.Rooms);
        List<Room> selectedRooms = new List<Room>();

        //Create list of rooms
        for (int i=0; i<numRooms; i++)
        {
            int rand = Random.Range(0, availableRooms.Count);

            //First room cant be your room
            while (i == 0 && availableRooms[rand] == RoomController.instance.SkeletonRoom)
            {
                rand = Random.Range(0, availableRooms.Count);
            }

            //Cant check the same room back to back
            while (i > 0 && selectedRooms[i-1] == availableRooms[rand])
            {
                rand = Random.Range(0, availableRooms.Count);
            }

            //Last room must have an available hidingspot
            while (i == numRooms-1 && (selectedRooms[i - 1] == availableRooms[rand] || availableRooms[rand].HidingSpots.Count == 0))
            {
                rand = Random.Range(0, availableRooms.Count);
            }

            selectedRooms.Add(availableRooms[rand]);
        }


        //Selected random wander spots
        foreach (Room room in selectedRooms)
        {
            foreach (Transform transform in room.WanderSpots)
            {
                int rand = Random.Range(0, room.WanderSpots.Count);

                huntSpotsList.Add(room.WanderSpots[rand]);
            }
        }

        return huntSpotsList;
    }


    private void CheckHuntTimer()
    {
        if (huntTimer < 0)
        {
            //TODO: Randomize huntduration between a min and max
            huntTimer = Random.Range(huntTimeMin, huntTimeMax);

            //TODO: Start hunt
            //Debug.Log("HuntStarted");
            
        }


        huntTimer -= Time.deltaTime;
    }
}
