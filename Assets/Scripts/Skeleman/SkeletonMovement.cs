using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*NOTE:
 * When baking the navmesh, enable the NavRamp on the main stairs before hand. Once the navmesh is created disable the navramp.
*/

public class SkeletonMovement : MonoBehaviour
{
    public NavMeshAgent navAgent; //TODO: make private

    public bool debug;
    public GameObject debugSphere;

    // Start is called before the first frame update
    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }


    #region

    public bool HasArrived()
    {
        if (!navAgent.pathPending)
        {
            if (navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                return true;
            }
        }
        
        return false;
    }



    public void SetTarget(Vector3 target)
    {
        navAgent.SetDestination(target);

        Room room = RoomController.instance.GetRoom(target);

        if (SkeletonStateManager.instance.curState == SkeletonStateManager.State.chase)
        {
            if (debug)
            {
                GameObject go = Instantiate(debugSphere);
                go.transform.position = target;
            }
        }
    }


    public void ClearDestination()
    {
        navAgent.ResetPath();
    }


    #endregion

}
