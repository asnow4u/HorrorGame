using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class SkeletonBehavior : MonoBehaviour
{
    public static SkeletonBehavior instance;

    [Header("Skeleton Properties")]
    [SerializeField] private GameObject skeleton;
    [SerializeField] private Transform skeletonHead;

    [SerializeField] private float sightAngle;
    [SerializeField] private float sightRange;

    //TODO: Some timers can be replaced with animation lengths
    [Header("Timers")]
    [SerializeField] private float dormantTimer;
    [SerializeField] private float wanderIdleRoomTimer;
    [SerializeField] private float searchHidingSpotTimer;

    //States
    public enum State {dormant, observe, wander, hunt, chase};
    public State state;
    
    private bool gameStarted;
    private bool timerFinished;
    private List<Room> previousRooms;


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
        gameStarted = true; //TODO: Grab this from somewhere else
        previousRooms = new List<Room>();

        state = State.dormant;
        Debug.Log("Skeleton: State Change to dormant");
        StartCoroutine(Wait(dormantTimer));
    }


    /// <summary>
    /// Update frame by frame
    /// </summary>
    private void Update()
    {
        if (!gameStarted) return;

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

    public GameObject Skeleton
    {
        get { return skeleton; }
    }

    private void ProgressCheck()
    {

        if (state == State.observe || state == State.wander || state == State.hunt)
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

                if (hit.transform.tag == "Player")
                {

                    Debug.Log("Player seen");

                    //TODO: if observe start a timer, if contact is never broken start chase
                    //Set observe off
                    

                    if (state != State.chase)
                    {
                        wanderState = WanderState.startWander;
                        huntState = HuntState.startHunt;

                        skeleton.GetComponent<SkeletonMovement>().ClearDestination();

                        state = State.chase;
                        chaseState = ChaseState.startChase;
                    }

                    return true;
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




    //Debug
    [ContextMenu("Set To Wander")]
    public void SetToWander()
    {
        state = State.wander;
     
        wanderState = WanderState.startWander;
        skeleton.GetComponent<SkeletonMovement>().enabled = true;
        skeleton.GetComponent<NavMeshAgent>().enabled = true;
    }

    [ContextMenu("Set To Hunt")]
    public void SetToHunt()
    {
        state = State.hunt;

        skeleton.GetComponent<SkeletonMovement>().enabled = true;
        skeleton.GetComponent<NavMeshAgent>().enabled = true;
    }

    [ContextMenu("Set To Chase")]
    public void SetToChase()
    {

    }
}
