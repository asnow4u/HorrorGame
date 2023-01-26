using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontDoorController : MonoBehaviour
{
    

    public float animationDelay;
    private float delayTime;
    
    private DoorController controller;
    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        controller = GetComponentInChildren<DoorController>();

        rb.isKinematic = true;
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {

            
        }
    }
}
