using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class SkeletonStateManager : MonoBehaviour
{

    /*Requirements
     * Chase: Skeleman is chasing the player
     * When the player is seen the skeleman will chase them. (Navmesh just following them)
     * If line of sight is broken, they will proceed to the last know location and look in the direction the player was looking at that time. 
     *      If they see the player again they will continue pursuit.
     *      If not, they will search the room for a bit, then will destroy one of the hidable locations. If only one remains the player is caught. If more than one exists the other hiding place will be destroyed
     *          We will need to determine that the direction vector points to a room
     * If the skeleman sees the player run into a hiding spot, they will be caught 
    */

    private enum ChaseState { startChase, chase, chaseSearch, chaseHidingSpot};
    
    [Header("Chase")]
    [SerializeField] private ChaseState chaseState;


    private void UpdateChase()
    {
       
        switch (chaseState)
        {
            case ChaseState.startChase:

                StartChase();
                break;


            case ChaseState.chase:

                Chase();             
                break;


            case ChaseState.chaseSearch:

                ChaseSearchRoom();                
                break;

            case ChaseState.chaseHidingSpot:

                if (skeleton.GetComponent<SkeletonMovement>().HasArrived())
                {
                    ChaseHidingSpotSearch();
                }

                break;
        }


        Debug.Log("Chase Player");
     
    } 


    private void StartChase()
    {
        //Animation or other sign of chase
        //TODO: Adjust speed
        chaseState = ChaseState.chase;
    }


    private void Chase()
    {
        if (CheckForPlayer())
        {
            Transform player = PlayerController.instance.gameObject.transform;
            skeleton.GetComponent<SkeletonMovement>().SetTarget(new Vector3(player.position.x, player.position.y - 2, player.position.z));
            skeleton.transform.LookAt(new Vector3(PlayerController.instance.transform.position.x, skeleton.transform.position.y, PlayerController.instance.transform.position.z));
        }

        else
        {
            if (skeleton.GetComponent<SkeletonMovement>().HasArrived())
            {
                //TODO: rememeber the direction of the player rot
                //look that way and determine if player is in sight

                if (CheckForPlayer())
                {
                    Transform player = PlayerController.instance.gameObject.transform;
                    skeleton.GetComponent<SkeletonMovement>().SetTarget(new Vector3(player.position.x, player.position.y - 2, player.position.z));
                    skeleton.transform.LookAt(new Vector3(PlayerController.instance.transform.position.x, skeleton.transform.position.y, PlayerController.instance.transform.position.z));
                }
                else
                {
                    chaseState = ChaseState.chaseSearch;
                }

            }
        }
    }


    private void ChaseSearchRoom()
    {
        //Check current room
        if (RoomController.instance.SkeletonRoom != null)
        {
            HidingSpot hidingSpot = RoomController.instance.SkeletonRoom.GetRandomHidingSpotWithOutPlayer();

            if (hidingSpot != null)
            {
                skeleton.GetComponent<SkeletonMovement>().SetTarget(hidingSpot.GetSkeletonSearchSpot().position);
                chaseState = ChaseState.chaseHidingSpot;
                Debug.Log("Skeleton: chaseState change to chaseHidingSpotSearch");
            }
        }

        ChangeState(State.hunt);
    }


    private void ChaseHidingSpotSearch()
    {
        //TODO: if player then they lose
        //if (playerFoundHiding)
        //{
        //    Debug.Log("You Were caught");
        //}

        ChangeState(State.wander);
        Debug.Log("Skeleton: State change to Wander");
        StartCoroutine(Wait(searchHidingSpotTimer));
    }
}
