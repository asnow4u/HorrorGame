using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardrobeController : MonoBehaviour
{

    private HingeJoint[] hinges;

    //Debug
    public bool forceClose;
    public bool forceOpen;

    private void Awake()
    {

        hinges = GetComponentsInChildren<HingeJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (forceClose)
        {
            forceClose = false;
            ForceClose();
        }

        if (forceOpen)
        {
            forceOpen = false;
            ForceOpen();
        }
    }


    public void ForceOpen()
    {
        JointMotor motor = new JointMotor();
        motor.force = 100;
        motor.targetVelocity = 80;

        foreach(HingeJoint hinge in hinges)
        {
            hinge.motor = motor;
        }
    }


    public void ForceClose()
    {
        JointMotor motor = new JointMotor();
        motor.force = 100;
        motor.targetVelocity = -80;

        foreach (HingeJoint hinge in hinges)
        {
            hinge.motor = motor;
        }
    }
}
