using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class SkeletonBehavior : MonoBehaviour
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

    private enum ChaseState { startChase, chase, chaseSearch, endChase};
    
    [Header("Chase")]
    [SerializeField] private ChaseState chaseState;


    private void UpdateChase()
    {
       
        switch (chaseState)
        {
            //Set properties to begin chase
            case ChaseState.startChase:

                //Animation or other sign of chase
                //Adjust speed
                chaseState = ChaseState.chase;
                break;


            //Update pathing as long as player is in sight
            //Search if arived to target
            case ChaseState.chase:

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
                        chaseState = ChaseState.chaseSearch;
                    }
                }

                break;

            case ChaseState.chaseSearch:

                //TODO: Search for player in nearby area
                //TODO: can check through some rooms if needed






                chaseState = ChaseState.endChase;


                //TODO: Alway break an object if person is not found

                break;

            case ChaseState.endChase:

                //state = State.wander;
                chaseState = ChaseState.startChase;
                break;
        }


        Debug.Log("Chase Player");
     
    } 
}
