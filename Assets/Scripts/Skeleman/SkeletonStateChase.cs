using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class SkeletonStateManager : MonoBehaviour
{
    private enum ChaseState { startChase, chase, chaseInTransit, chaseSearch, chaseHidingSpot};
    
    [Header("Chase")]
    [SerializeField] private ChaseState chaseState;

    private float playerDirectionRot;


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


            case ChaseState.chaseInTransit:

                ChaseInTrasit();
                break;


            case ChaseState.chaseSearch:

                if (skeletonMovement.HasArrived())
                {
                    ChaseSearchRoom();                
                }

                break;


            case ChaseState.chaseHidingSpot:

                if (skeletonMovement.HasArrived())
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
            skeletonMovement.SetTarget(new Vector3(player.position.x, player.position.y - 2, player.position.z));
            skeleton.transform.LookAt(new Vector3(PlayerController.instance.transform.position.x, skeleton.transform.position.y, PlayerController.instance.transform.position.z));
            playerDirectionRot = PlayerController.instance.transform.rotation.eulerAngles.y;
        }

        else
        {
            if (skeleton.GetComponent<SkeletonMovement>().HasArrived())
            {
                Quaternion tempRot = skeletonHead.transform.rotation;
                skeletonHead.transform.rotation = Quaternion.Euler(skeletonHead.transform.rotation.eulerAngles.x, playerDirectionRot, skeletonHead.transform.rotation.eulerAngles.z);

                if (CheckForPlayer())
                {
                    Transform player = PlayerController.instance.gameObject.transform;
                    skeletonMovement.SetTarget(new Vector3(player.position.x, player.position.y - 2, player.position.z));
                    skeleton.transform.LookAt(new Vector3(PlayerController.instance.transform.position.x, skeleton.transform.position.y, PlayerController.instance.transform.position.z));
                    playerDirectionRot = PlayerController.instance.transform.rotation.eulerAngles.y;
                }
                else
                {
                    chaseState = ChaseState.chaseInTransit;
                }

                skeletonHead.transform.rotation = tempRot;
            }
        }
    }


    private void ChaseInTrasit()
    {
        if (RoomController.instance.PlayerRoom != null)
        {
            //Go to the players room
            int randSpot = Random.Range(0, RoomController.instance.PlayerRoom.WanderSpots.Count);
            skeletonMovement.SetTarget(RoomController.instance.PlayerRoom.WanderSpots[randSpot].position);

            chaseState = ChaseState.chaseSearch;
        }

        else
        {
            ChangeState(State.hunt);
        }
    }


    private void ChaseSearchRoom()
    {
        //TODO: Look around for player
        //if player is in process of hiding search that hiding spot
        

        //Search where player is hiding
        HidingSpot hidingSpot = RoomController.instance.SkeletonRoom.GetRandomHidingSpotWithOutPlayer();
               
        if (hidingSpot != null)
        {
            skeletonMovement.SetTarget(hidingSpot.SkeletonSearchSpot.position);
            chaseState = ChaseState.chaseHidingSpot;
            Debug.Log("Skeleton: chaseState change to chaseHidingSpotSearch");
        }

        else
        {
            ChangeState(State.hunt);
            StartCoroutine(Wait(wanderIdleRoomTimer));
        }
    }


    private void ChaseHidingSpotSearch()
    {
        //TODO: if player then they lose
        //if (playerFoundHiding)
        //{
        //    Debug.Log("You Were caught");
        //}

        ChangeState(State.hunt);
        StartCoroutine(Wait(searchHidingSpotTimer));
    }
}
