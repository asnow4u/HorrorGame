using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedHidingSpot : HidingSpot
{
    private bool playerCollision;
    private bool leaveAnimationPlaying;

    private bool delayTimer;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {

        if (leaveAnimationPlaying && !delayTimer)
        {
            //Update Bool
            leaveAnimationPlaying = false;

            //Toggle Movement
            PlayerController.instance.ToggleMovement(true);
        }


        //(1) Hide Action
        if (playerCollision && !isPlayerHiding && !delayTimer)
        {
            //Panel On
            UIController.instance.ToggleHidePanel(true);

            if (Input.GetKey(KeyCode.E))
            {
                //Panel Off
                UIController.instance.ToggleHidePanel(false);
                
                //Keyframe Animation
                PlayerController.instance.ToggleMovement(false);
                StartCoroutine(KeyFrameMovement(PlayerController.instance.transform, hideKeyFrames, hideDurations));

                //Bool Update
                isPlayerHiding = true;
                PlayerController.instance.curHidingSpot = this;
                PlayerController.instance.hidingAnimationPlaying = true;

                //Delay
                float totalTime = 0;
                foreach (float time in hideDurations)
                {
                    totalTime += time;
                }
                StartCoroutine(Delay(totalTime));
            }
        }


        //(2) Leave
        else if (isPlayerHiding && !delayTimer)
        {
            //Panel On
            UIController.instance.ToggleLeavePanel(true);

            if (Input.GetKey(KeyCode.E))
            {
                //Panel Off
                UIController.instance.ToggleLeavePanel(false);

                //Keyframe Animation
                StartCoroutine(KeyFrameMovement(PlayerController.instance.transform, leaveKeyFrames, leaveDurations));

                //Update Bool
                isPlayerHiding = false;
                leaveAnimationPlaying = true;

                PlayerController.instance.curHidingSpot = null;                

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
        playerCollision = false;
        UIController.instance.ClearPanel();
    }

}
