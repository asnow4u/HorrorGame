using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*NOTE:
 * When baking the navmesh, enable the NavRamp on the main stairs before hand. Once the navmesh is created disable the navramp.
*/

public class SkeletonMovement : MonoBehaviour
{

    /* Requirement
     * 
     * Based on the behaviour, either teleport the skeleton around or move with weighted movement.
     * Weighted movement is random movement that will be steered towards the remaining keys and player
     */

    [SerializeField] private Transform targetPos;

    private NavMeshAgent navAgent;

    // Start is called before the first frame update
    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        navAgent.destination = targetPos.position;
    }
}
