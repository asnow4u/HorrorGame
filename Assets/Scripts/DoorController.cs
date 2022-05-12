using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private HingeJoint hinge;

    //Limit
    private float maxLimit;
    private float minLimit;
    private float bouceLimit;

    //Spring
    private float springTarget;
    private float springForce;
    private float springDamp;

    //Motor
    private float motorVelocity;
    private float motorForce;
    

    //Debug
    public bool forceClose;
    public bool reset;
    public bool forceOpen;


    // Start is called before the first frame update
    void Start()
    {
        hinge = GetComponent<HingeJoint>();

        //Limit
        maxLimit = hinge.limits.max;
        minLimit = hinge.limits.min;
        bouceLimit = hinge.limits.bounciness;

        //Spring
        springTarget = hinge.spring.targetPosition;
        springForce = hinge.spring.spring;
        springDamp = hinge.spring.damper;

        //Motor
        motorVelocity = hinge.motor.targetVelocity;
        motorForce = hinge.motor.force;
    }

    private void Update()
    {
        if (forceClose)
        {
            ForceClose();
        }

        if (forceOpen)
        {
            ForceOpen();
        }

        if (reset)
        {
            Reset();
            reset = false;
        }
    }


    public void ForceClose()
    {
        JointLimits limit = new JointLimits();
        limit.min = hinge.limits.min;
        limit.max = hinge.limits.max;
        limit.bounciness = 0;

        JointSpring spring = new JointSpring();
        spring.targetPosition = 0;
        spring.spring = 100;
        spring.damper = 0;

        hinge.limits = limit;
        hinge.spring = spring;
        hinge.useSpring = true;
    }


    public void ForceOpen()
    {
        JointMotor motor = new JointMotor();
        motor.force = 50;
        motor.targetVelocity = 20;
        
        hinge.motor = motor;
        hinge.useMotor = true;
    }


    public void Reset()
    {
        //Limit
        JointLimits limit = new JointLimits();
        limit.min = minLimit;
        limit.max = maxLimit;
        limit.bounciness = bouceLimit;

        //Spring
        JointSpring spring = new JointSpring();
        spring.targetPosition = springTarget;
        spring.spring = springForce;
        spring.damper = springDamp;

        //Motor
        JointMotor motor = new JointMotor();
        motor.force = 0;
        motor.targetVelocity = 0;

        hinge.useSpring = false;
        hinge.useMotor = false;

        hinge.limits = limit;
        hinge.spring = spring;
        hinge.motor = motor;
    }
}
