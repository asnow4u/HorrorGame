using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*NOTE:
 * When baking the navmesh, enable the NavRamp on the main stairs before hand. Once the navmesh is created disable the navramp.
*/

public class SkeletonMovement : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private bool arrived;

    // Start is called before the first frame update
    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (arrived == false)
        {
            if (navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                arrived = true;
            }
        }
    }


    #region

    public bool Arrived
    {
        get { return arrived; }
    }

    public bool HasDestination
    {
        get { return navAgent.hasPath; }
    }


    public void SetTarget(Vector3 target)
    {
        if (arrived == false) return;

        navAgent.destination = target;
        arrived = false;
    }



    #endregion

}
