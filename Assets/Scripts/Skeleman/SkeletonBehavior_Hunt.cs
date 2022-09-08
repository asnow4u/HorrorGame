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
    [Range(1,2)]
    [SerializeField] private float huntRoomMin;
    [SerializeField] private float huntRoomMax;

    private enum HuntState {startHunt, hunt, inTransitHunt, inRoomSearch, endHunt }
    [SerializeField] private HuntState huntState;

    private float huntDuration;
    private float huntTimer;

    private List<Transform> huntSpots;
    private int huntSpotCount;

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
                //Reset
                huntSpotCount = 0;
                //Hunt effect
                SelectRoomsToHunt();
                StartCoroutine(Wait(0.2f));
                huntState = HuntState.hunt;

                break;


            case HuntState.hunt:

                if (timerFinished)
                {
                    skeleton.GetComponent<SkeletonMovement>().SetTarget(huntSpots[huntSpotCount].position);
                    huntSpotCount++;
                    huntState = HuntState.inTransitHunt;
                }

                break;


            case HuntState.inTransitHunt:

                if (skeleton.GetComponent<SkeletonMovement>().HasArrived())
                {
                    //TODO: Randomize if searching (based on key percentage)

                    if (true)
                    {
                        huntState = HuntState.inRoomSearch;

                    }

                    else
                    {

                        //Continue hunting (not searching room)
                        if (huntSpotCount < huntSpots.Count)
                        {
                            huntState = HuntState.hunt;
                        }

                        else
                        {
                            huntState = HuntState.endHunt;
                        }
                    }

                    StartCoroutine(Wait(wanderIdleRoomTimer));
                }

                break;


            case HuntState.inRoomSearch:

                break;


            case HuntState.endHunt:

                if (timerFinished)
                {
                    //TODO: restore to previous state

                }

                break;
        }
     
    }


    public void SelectRoomsToHunt()
    {

        //Determine number of rooms to search
        int numRooms = (int)Random.Range(huntRoomMin, huntRoomMax);

        //Randomize Rooms
        List<Room> availableRooms = new List<Room>();

        foreach (Room room in RoomController.instance.Rooms)
        {
            //Dont do room your in
            if (room == RoomController.instance.SkeletonRoom) continue;

            availableRooms.Add(room);
        }


        //Randomize Spot
        for (int i=0; i<numRooms; i++)
        {
            int rand = Random.Range(0, availableRooms.Count);

            List<Transform> availableSpots = new List<Transform>();

            foreach (Transform transform in availableRooms[rand].WanderSpots)
            {
                //Add available spot
                availableSpots.Add(transform);

                rand = Random.Range(0, availableSpots.Count);

                huntSpots.Add(availableSpots[rand]);
            }

            availableRooms.RemoveAt(rand);
        }
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
