using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBehavior : MonoBehaviour
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
     * 
     * observing: Knows that others exist in the house and will be seen around the house
     * sitting in places like on the couch, standing in corners
     * Head movement, creaky neck
     * Need:
     * List of places to teleport
     * Timer
     * Use Room Controller to determine where player is before moving
     * 
     * wander: Skeleman will start to wander (this will start with a resurection happening when the player is close)
     * (no more teleports at this point)
     * Slow walk walking through the house
     * If the skeleman sees you for an extended period of time it will begin a hunt (need a timer for that)
     * The skeleman will occationally look over the upper railing to the living room (should play with shadows to make it apparent hes there)
     * Need:
     * Timer (Will decrease the time needed based on how well the player is doing
     * Move speed (will increase based on on how well the player is doing)
     * 
     * Hunt: Skeleman is activally hunting. 
     * Skeleman will do something to indicate a hunt. A sound cue / effect will also occure.
     * If the skeleman catches the player its gameover
     * If the player is seen it will chase the player
     * Vinnete effect of somekind to indicate the skeleman is close (darker in the edges of the screen)
     * If hunt is unsucessful the skeleman will destroy one of the locations to hide (first time should not happen in the room their hiding in so they can find it (They should hear it))
     * Will run through most of the rooms, (sometimes the one their in sometimes not)
     * Need:
     * Timer (length of hunt)
     * Use Room Controller to determine where the player is
     * Locations for each of the rooms and hiding spots
     * 
     * Chase: Sekelman is chasing the player
     * When the player is seen the skeleman will chase them. (Navmesh just following them)
     * If line of sight is broken, they will proceed to the last know location and look in the direction the player was looking at that time. 
     *      If they see the player again they will continue pursuit.
     *      If not, they will search the room for a bit, then will destroy on of the hidable locations. If only one remains the player is caught. If more than one exists the other hiding place will be destroyed
     *          We will need to determine that the direction vector points to a room
     * If the skeleman sees the player run into a hiding spot, they will be caught     
    */

    public static SkeletonBehavior instance;

    [SerializeField] private GameObject skeleton;
    [SerializeField] private Transform dormatObj;
    [SerializeField] private Transform observeObj;

    [SerializeField] private float dormantTimer;
    
    private List<Transform> dormatLocations = new List<Transform>();
    private List<Transform> observeLocations = new List<Transform>();

    public enum State {dormant, observe, wander, hunt, chase};
    public State state;

    private bool timerFinished;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        foreach (Transform location in dormatObj)
        {
            dormatLocations.Add(location);
        }


        state = State.dormant;

        StartCoroutine(Wait(dormantTimer));
    }


    private void Update()
    {

        switch(state)
        {
            case State.dormant:
                UpdateDormant();
                break;

            case State.observe:
                UpdateObserve();
                break;

            case State.wander:
                UpdateWander();
                break;

            case State.hunt:
                UpdateHunt();
                break;

            case State.chase:
                UpdateChase();
                break;
        }
    }


    private void UpdateDormant()
    {
        if (!timerFinished) return;
        if (PlayerController.instance.InViewOfCamera(skeleton.transform.position)) return;

        Room playerRoom = RoomController.instance.PlayerRoom;
        Room skeletonRoom = RoomController.instance.SkeletonRoom;

        if (playerRoom == null) return;
        if (skeletonRoom != null && playerRoom.Type == skeletonRoom.Type) return;

        List<Transform> availbleSpots = new List<Transform>();

        foreach (Room room in RoomController.instance.Rooms)
        {
            if (skeletonRoom != null && room.Type == skeletonRoom.Type) continue;
            if (room.Type == playerRoom.Type) continue;

            foreach (Transform transform in room.DormantSpots)
            {
                if (!PlayerController.instance.InViewOfCamera(transform.position))
                {
                    availbleSpots.Add(transform);
                }
            }
        }

        int rand = Random.Range(0, availbleSpots.Count);

        skeleton.transform.position = availbleSpots[rand].position;
        //TODO: Rotation

        StartCoroutine(Wait(dormantTimer));
    }


    private void UpdateObserve()
    {
       
     
    } 


    private void UpdateWander()
    {
        //TODO: travel to destination
        //Once destination is reached set new destination
        //Go to curtain spots, look around, occationally look at a hiding spot
        //! during this stage if check a hiding spot, should not check one the player is in
    }


    private void UpdateHunt()
    {

    }


    private void UpdateChase()
    {

    }



    private IEnumerator Wait(float time)
    {
        timerFinished = false;

        yield return new WaitForSeconds(time);

        timerFinished = true;
    }
}
