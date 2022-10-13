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



    private void UpdateChase()
    {
        //TODO:
        //Update navmesh to follow of player
        //if line of sight broken go to the last point and guess (look in nearby room)

        //Alway break an object if person is not found

        Debug.Log("Chase Player");
     
    } 
}
