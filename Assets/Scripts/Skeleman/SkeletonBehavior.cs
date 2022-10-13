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
    */

    public static SkeletonBehavior instance;

    [Header("Skeleton Properties")]
    [SerializeField] private GameObject skeleton;
    [SerializeField] private Transform skeletonHead;

    [SerializeField] private float sightAngle;
    [SerializeField] private float sightRange;

    //State
    public enum State {dormant, observe, wander, hunt, chase};
    public State state;



    //Properties
    private bool gameStarted;
    private float keyPercent;
    
    //TODO: Some timers can be replaced with animation lengths
    //Set Timers
    [SerializeField] private float dormantTimer;
    [SerializeField] private float wanderIdleRoomTimer;
    [SerializeField] private float searchHidingSpotTimer;
    private bool timerFinished;

    private List<Room> previousRooms;

    //TESTING
    [Header("Debug")]
    public bool debug;
    public bool startWanderState;
    public bool startHuntState;

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
    }

    private void Start()
    {
        Initialize();
    }


    /// <summary>
    /// Call this to begin the game
    /// </summary>
    public void Initialize()
    {
        gameStarted = true;

        previousRooms = new List<Room>();

        state = State.dormant;
        Debug.Log("Skeleton: State Change to dormant");
        StartCoroutine(Wait(dormantTimer));
    }


    private void Update()
    {
        if (!gameStarted) return;


        //NOTE: This is a debug part, not needed
        if (debug)
        {
            if (startWanderState)
            {
                startWanderState = false;
                state = State.wander;
                wanderState = WanderState.startWander;
                skeleton.GetComponent<SkeletonMovement>().enabled = true;
                skeleton.GetComponent<NavMeshAgent>().enabled = true;
            }

            if (startHuntState)
            {
                startHuntState = false;
                state = State.hunt;
                
                skeleton.GetComponent<SkeletonMovement>().enabled = true;
                skeleton.GetComponent<NavMeshAgent>().enabled = true;
            }
        }


        ProgressCheck();
        

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


    private void ProgressCheck()
    {

        //if (state == State.observe || state == State.wander || state == State.hunt)
        {
            if (CheckForPlayer())
            {
                state = State.chase;
            }
        }



        //TODO: change to be time based

        //% of gathered key and or time based
        //int totalKeys = KeyController.instance.HeldGoldKeys;
        //totalKeys += KeyController.instance.HeldSilverKeys;
        //totalKeys += KeyController.instance.UsedGoldKeys;
        //totalKeys += KeyController.instance.UsedSilverKeys;

        //keyPercent = (float)totalKeys / (float)(KeyController.instance.StartGoldKeys + KeyController.instance.StartSilverKeys);
      
        ////dormant to observe 20%
        //if (state == State.dormant)
        //{
        //    if (keyPercent > .2f)
        //    {
        //        state = State.observe;
        //        Debug.Log("Skeleton: State Change to Observant");
        //    }
        //}

        ////observe to wander 40%
        //else if (state == State.observe)
        //{
        //    if (keyPercent > .4f)
        //    {
        //        state = State.wander;
        //        Debug.Log("Skeleton: State Change to Wander");
        //        wanderState = WanderState.startWander;
        //        Debug.Log("Skeleton: WanderState Change to StartWander");
        //        skeleton.GetComponent<SkeletonMovement>().enabled = true;
        //        skeleton.GetComponent<NavMeshAgent>().enabled = true;
        //    }
        //}

        //Random hunts can only occure when wandering
        if (state == State.wander)
        {
            CheckHuntTimer();
        }

        
    }


    public bool CheckForPlayer()
    {
        Vector3 rayVector = PlayerController.instance.MainCamera.transform.position - skeletonHead.position;

        Debug.DrawLine(skeletonHead.position, PlayerController.instance.MainCamera.transform.position);

        //Check for angle
        float angle = Vector3.Angle(-skeletonHead.transform.up, rayVector);
        
        if (angle <= sightAngle)
        {

            Ray ray = new Ray(skeletonHead.position, rayVector);
            Debug.DrawRay(skeletonHead.position, rayVector);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, sightRange, LayerMask.NameToLayer("Room")))
            {
                Debug.Log(hit.transform.name);

                if (hit.transform.tag == "Player")
                {

                    Debug.Log("Player seen");


                }
            }
        }

        return false;
    }



    private IEnumerator Wait(float time)
    {
        timerFinished = false;

        yield return new WaitForSeconds(time);

        timerFinished = true;
    }
}
