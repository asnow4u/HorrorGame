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

            if (Time.time > delayTime)
            {

                if (KeyController.instance.KeysFound())
                {
                    Debug.Log("Press E to Open Door");

                    if (Input.GetKey(KeyCode.E))
                    {
                        //controller.Force(5); //TODO: Play with the force value
                        rb.isKinematic = false;
                    }
                }

                else if (KeyController.instance.HeldSilverKeys > 0)
                {
                    Debug.Log("Press E to Insert key");

                    if (Input.GetKey(KeyCode.E))
                    {
                        KeyController.instance.HeldSilverKeys--;
                        KeyController.instance.UsedSilverKeys++;
                        delayTime = Time.time + animationDelay;
                    }
                }

                else if (KeyController.instance.HeldGoldKeys > 0)
                {
                    Debug.Log("Press E to Insert key");
                
                    if (Input.GetKey(KeyCode.E))
                    {
                        KeyController.instance.HeldGoldKeys--;
                        KeyController.instance.UsedGoldKeys++;
                        delayTime = Time.time + animationDelay;
                    }
                }
            }
        }
    }
}
