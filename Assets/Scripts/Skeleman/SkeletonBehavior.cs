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

    [SerializeField] private GameObject skeleton;

    //State
    private enum State {dormant, observe, wander, hunt, chase};
    private State state;

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
        StartCoroutine(Wait(dormantTimer));
    }


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


    private void ProgressCheck()
    {
        //% of gathered key and or time based
        int totalKeys = KeyController.instance.HeldGoldKeys;
        totalKeys += KeyController.instance.HeldSilverKeys;
        totalKeys += KeyController.instance.UsedGoldKeys;
        totalKeys += KeyController.instance.UsedSilverKeys;

        keyPercent = (float)totalKeys / (float)(KeyController.instance.StartGoldKeys + KeyController.instance.StartSilverKeys);
      
        //dormant to observe 20%
        if (state == State.dormant)
        {
            if (keyPercent > .2f)
            {
                state = State.observe;
            }
        }

        //observe to wander 40%
        else if (state == State.observe)
        {
            if (keyPercent > .4f)
            {
                state = State.wander;
                wanderState = WanderState.Wander;
                skeleton.GetComponent<SkeletonMovement>().enabled = true;
                skeleton.GetComponent<NavMeshAgent>().enabled = true;
            }
        }

        //Random hunts can only occure when wandering
        if (state == State.wander)
        {
            CheckHuntTimer();
        }
    }


    private IEnumerator Wait(float time)
    {
        timerFinished = false;

        yield return new WaitForSeconds(time);

        timerFinished = true;
    }
}
