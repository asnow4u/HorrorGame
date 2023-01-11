using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardrobeHidingSpot : HidingSpot
{
    private Transform parent;

    private bool playerCollision;
    private bool hideAnimationPlaying;
    private bool leaveAnimationPlaying;

    private bool isOpen;
    private bool delayTimer;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;

    }

    private void Update()
    {
        if (hideAnimationPlaying)
        {
            if (PercentageFinished > 0.8f)
            {
                //Close Doors
                foreach (DoorController door in parent.GetComponents<DoorController>())
                {
                    door.Force(15f);
                }

                //Update Bool
                hideAnimationPlaying = false;
                isOpen = false;
            }
        }


        else if (leaveAnimationPlaying)
        {
            if (PercentageFinished == 1f)
            {
                //Close Doors
                foreach (DoorController door in parent.GetComponents<DoorController>())
                {
                    door.Force(15f);
                }

                //Toggle Movement
                PlayerController.instance.ToggleMovement(true);

                //Update Bool
                leaveAnimationPlaying = false;
                isOpen = false;
            }
        }


        //(1) Open Action
        else if (playerCollision && !isOpen && !isPlayerHiding && !delayTimer)
        {

            if (Input.GetKey(KeyCode.E))
            {
                //Open Doors
                foreach (DoorController door in parent.GetComponents<DoorController>())
                {
                    door.Force(-15f);
                }

                //Bool Update
                isOpen = true;
                
                //Delay
                StartCoroutine(Delay(0.7f));
            }
        }


        //(2) Hide Action
        else if (playerCollision && isOpen && !isPlayerHiding && !delayTimer)
        {

            if (Input.GetKey(KeyCode.E))
            {
                
                //Keyframe Animation
                PlayerController.instance.ToggleMovement(false);
                StartCoroutine(KeyFrameMovement(PlayerController.instance.transform, hideKeyFrames, hideDurations));

                //Bool Update
                hideAnimationPlaying = true;
                isPlayerHiding = true;

                //Delay
                float totalTime = 0;
                foreach (float time in hideDurations)
                {
                    totalTime += time;
                }
                StartCoroutine(Delay(totalTime));
            }
        }


        //(3) Leave
        else if (!isOpen && isPlayerHiding && !delayTimer)
        {

            if (Input.GetKey(KeyCode.E))
            {

                //Open Doors
                foreach (DoorController door in parent.GetComponents<DoorController>())
                {
                    door.Force(-15f);
                }

                //Keyframe Animation
                StartCoroutine(KeyFrameMovement(PlayerController.instance.transform, leaveKeyFrames, leaveDurations));

                //Update Bool
                leaveAnimationPlaying = true;
                isOpen = true;
                isPlayerHiding = false;

                //Delay
                float totalTime = 0;
                foreach (float time in leaveDurations)
                {
                    totalTime += time;
                }
                StartCoroutine(Delay(totalTime));
            }
        }
    }



    private IEnumerator Delay(float timer)
    {
        delayTimer = true;
        yield return new WaitForSeconds(timer);
        delayTimer = false;
    }


    private void OnTriggerEnter(Collider col)
    {
        
        if (col.gameObject.tag == "Player")
        {
            playerCollision = true;
        }
    }


    private void OnTriggerExit(Collider col)
    {

        if (col.gameObject.tag == "Player")
        {
            playerCollision = false;
            UIController.instance.ClearPanel();

            if (isOpen)
            {
                isOpen = false;
                foreach (DoorController door in parent.GetComponents<DoorController>())
                {
                    door.Force(15f);
                }
            }
        }

        //if (col.gameObject.tag == "Skeleman")
        //{
        //    SkeletonBehavior.instance.PlayerFoundHiding = isPlayerHiding;
        //}
    }
}
