using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class SkeletonStateManager : MonoBehaviour
{
    public static SkeletonStateManager instance;

    [Header("Skeleton Properties")]
    [SerializeField] private GameObject skeleton;
    [SerializeField] private Transform skeletonHead;

    [SerializeField] private float sightAngle;
    [SerializeField] private float sightRange;

    //TODO: Timers can be replaced with animation lengths
    [Header("Timers")]
    [SerializeField] private float dormantTimer;
    [SerializeField] private float wanderIdleRoomTimer;
    [SerializeField] private float searchHidingSpotTimer;

    //States
    public enum State {dormant, wander, hunt, chase};
    public State curState;
    
    //Bools
    private bool gameStarted;
    private bool timerFinished;

    //Other
    private SkeletonMovement skeletonMovement;
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
        //TODO: Grab this from somewhere else (Menu)
        gameStarted = true;

        skeletonMovement = skeleton.GetComponent<SkeletonMovement>();
        previousRooms = new List<Room>();

        curState = State.dormant;
        StartCoroutine(Wait(dormantTimer));
        
        Debug.Log("Skeleton: State Change to dormant");
    }


    /// <summary>
    /// Update based on state
    /// </summary>
    private void Update()
    {
        if (!gameStarted) return;

        //Check for player in sight
        if (curState != State.chase && CheckForPlayer())
        {
            ChangeState(State.chase);
        }
        
        CheckHuntTimer();       

        UpdateState();
    }


    /// <summary>
    /// Update based on current state
    /// </summary>
    private void UpdateState()
    {
        switch (curState)
        {
            case State.dormant:
                UpdateDormant();
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


    /// <summary>
    /// Change the current state 
    /// Reset values
    /// </summary>
    /// <param name="state"></param>
    public void ChangeState(State state)
    {
        curState = state;

        wanderState = WanderState.startWander;
        huntState = HuntState.startHunt;
        chaseState = ChaseState.startChase;

        skeletonMovement.ClearDestination();
    }


    /// <summary>
    /// Determine if player is in sight
    /// Switch to chase state
    /// </summary>
    /// <returns></returns>
    public bool CheckForPlayer()
    {
        if (curState == State.wander || curState == State.hunt || curState == State.chase)
        {

            Vector3 rayVector = PlayerController.instance.MainCamera.transform.position - skeletonHead.position;

            Debug.DrawLine(skeletonHead.position, PlayerController.instance.MainCamera.transform.position);

            //Check for angle
            float angle = Vector3.Angle(-skeletonHead.transform.up, rayVector);

            //Within sightrange
            if (angle <= sightAngle)
            {
                Ray ray = new Ray(skeletonHead.position, rayVector);
                Debug.DrawRay(skeletonHead.position, rayVector);

                RaycastHit hit;

                //Check for obsticales obsuring vision
                if (Physics.Raycast(ray, out hit, sightRange, LayerMask.NameToLayer("Room")))
                {
                    if (hit.transform.tag == "Player")
                    {
                        Debug.Log("Player seen");

                        if (curState != State.chase)
                        {
                            ChangeState(State.chase);
                        }

                        return true;
                    }
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
        skeleton.GetComponent<SkeletonMovement>().enabled = true;
        skeleton.GetComponent<NavMeshAgent>().enabled = true;
        ChangeState(State.wander);
    }

    [ContextMenu("Set To Hunt")]
    public void SetToHunt()
    {
        skeleton.GetComponent<SkeletonMovement>().enabled = true;
        skeleton.GetComponent<NavMeshAgent>().enabled = true;
        ChangeState(State.hunt);
    }

    [ContextMenu("Set To Chase")]
    public void SetToChase()
    {

    }
}
