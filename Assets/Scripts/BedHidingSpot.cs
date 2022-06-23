using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedHidingSpot : MonoBehaviour
{
    [SerializeField] private List<Transform> hideKeyFrames;
    [SerializeField] private List<float> hideDurations;
    [SerializeField] private List<Transform> leaveKeyFrames;
    [SerializeField] private List<float> leaveDurations;

    private Transform parent;
    private KeyFrameController frameController;
    private DoorController doorController;

    private bool playerCollision;
    private bool hideAnimationPlaying;
    private bool leaveAnimationPlaying;

    private bool isPlayerHiding;
    private bool delayTimer;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        frameController = GetComponent<KeyFrameController>();
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
                StartCoroutine(frameController.KeyFrameMovement(PlayerController.instance.transform, hideKeyFrames, hideDurations));

                //Bool Update
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


        //(2) Leave
        else if (isPlayerHiding && !delayTimer)
        {
            //Panel On
            UIController.instance.ToggleLeavePanel(true);

            if (Input.GetKey(KeyCode.E))
            {
                //Panel Off
                UIController.instance.ToggleLeavePanel(false);

                //Open Doors
                foreach (DoorController door in parent.GetComponents<DoorController>())
                {
                    door.Force(-15f);
                }

                //Keyframe Animation
                StartCoroutine(frameController.KeyFrameMovement(PlayerController.instance.transform, leaveKeyFrames, leaveDurations));

                //Update Bool
                isPlayerHiding = false;
                leaveAnimationPlaying = true;

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
    
        //TODO: Close door if open?
    }

}
