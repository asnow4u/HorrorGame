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
                        controller.ForceOpen();
                        rb.isKinematic = false;
                    }
                }

                else if (KeyController.instance.silverKeyHeldCount > 0)
                {
                    Debug.Log("Press E to Insert key");

                    if (Input.GetKey(KeyCode.E))
                    {
                        KeyController.instance.silverKeyHeldCount--;
                        KeyController.instance.silverKeyUsedCount++;
                        delayTime = Time.time + animationDelay;
                    }
                }

                else if (KeyController.instance.goldKeyHeldCount > 0)
                {
                    Debug.Log("Press E to Insert key");
                
                    if (Input.GetKey(KeyCode.E))
                    {
                        KeyController.instance.goldKeyHeldCount--;
                        KeyController.instance.goldKeyUsedCount++;
                        delayTime = Time.time + animationDelay;
                    }
                }
            }
        }
    }
}
