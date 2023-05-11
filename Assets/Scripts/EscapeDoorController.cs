using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeDoorController : DoorController
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
            PlayerController playerController = col.GetComponent<PlayerController>();
            Items holding = playerController.heldItem;

            if (holding != null)
            {
                if (holding.TryGetComponent<Key>(out Key key))
                {
                    
                    //holding.DestroyItem();
                }
            }
        }
    }

    public void IsTheOne(Key key)
    {
        if (key.theONE == true)
        {
            //TODO: Open the door and victory screech.
            Debug.Log("You have won.");
        }
    }
}
