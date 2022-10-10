using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoorController : MonoBehaviour
{

    [SerializeField] private Transform door;

    private NavMeshObstacle meshObsticle = null;
    private HingeJoint hinge;
    
    //Limit
    private float maxLimit;
    private float minLimit;

    //Spring
    private float springForce;

    //Debug
    public float force;
    public bool forceOpen;


    // Start is called before the first frame update
    void Start()
    {
        meshObsticle = door.GetComponent<NavMeshObstacle>();

        hinge = door.GetComponent<HingeJoint>();

        //Limit
        maxLimit = hinge.limits.max;
        minLimit = hinge.limits.min;

        //Spring
        springForce = hinge.spring.spring;
    }

    private void Update()
    {
        if (forceOpen)
        {
            forceOpen = false;
            Force(force);
        }
    }


    public void Force(float openForce)
    {
        JointSpring spring = new JointSpring();

        if (openForce > 0)
        {
            spring.targetPosition = minLimit;
            spring.spring = Mathf.Abs(openForce);
        }

        else
        {
            spring.targetPosition = maxLimit;
            spring.spring = Mathf.Abs(openForce);
        }

        hinge.spring = spring;
        hinge.useSpring = true;
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Skeleman" || col.gameObject.tag == "Player")
        {
            JointLimits limit = new JointLimits();
            limit.min = minLimit;
            limit.max = maxLimit;

            hinge.useSpring = false;
            hinge.limits = limit;
        }


        if (col.gameObject.tag == "Skeleman")
        {
            JointSpring spring = new JointSpring();
            
            Vector3 dir = transform.InverseTransformPoint(col.transform.position);
            
            if (dir.x > 0)
            {
                spring.targetPosition = minLimit;
                spring.spring = springForce;
            }

            else
            {
                spring.targetPosition = maxLimit;
                spring.spring = springForce;
            }

            hinge.spring = spring;
            hinge.useSpring = true;

            //meshObsticle.enabled = true;
        }
    }


    private void OnTriggerStay(Collider col)
    {
        //TODO: Not perfect, can sometimes get stuck
        //The Idea was to prevent doors from glitching
        //if (col.gameObject.tag == "Skeleman")
        //{
        //    if (meshObsticle != null)
        //    {
        //        if (Mathf.RoundToInt(hinge.angle) <= minLimit || Mathf.RoundToInt(hinge.angle) >= maxLimit)
        //        {
        //            meshObsticle.enabled = true;
        //        }
        //        else
        //        {
        //            meshObsticle.enabled = false;
        //        }
        //    }
        //}
    }


    private void OnTriggerExit(Collider col)
    {
        
        if (col.gameObject.tag == "Skeleman" || col.gameObject.tag == "Player")
        {
            JointSpring spring = new JointSpring();
            JointLimits limit = new JointLimits();

            if (door.rotation.z < 0)
            {
                limit.min = 0;
                limit.max = maxLimit;
            }

            else
            {
                limit.max = 0;
                limit.min = minLimit;
            }

            spring.targetPosition = 0;
            spring.spring = col.gameObject.tag == "Skeleman" ? springForce : 5;

            hinge.limits = limit;
            hinge.spring = spring;
            hinge.useSpring = true;
        }        
    }
}
