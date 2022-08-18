using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class SkeletonBehavior : MonoBehaviour
{

    /*Requirements
     
      * Hunt: Skeleman is activally hunting. 
     * Skeleman will do something to indicate a hunt. A sound cue / effect will also occure.
     * If the skeleman catches the player its gameover
     * If the player is seen it will chase the player
     * Vinnete effect of somekind to indicate the skeleman is close (darker in the edges of the screen)
     * If hunt is unsucessful the skeleman will destroy one of the locations to hide (first time should not happen in the room their hiding in so they can find it (They should hear it))
     * Will run through most of the rooms, (sometimes the one their in sometimes not)
     * Need:
     * Timer (length of hunt)
     * Use Room Controller to determine where the player is
     * Locations for each of the rooms and hiding spots
    */

    [Header("Hunt")]
    [SerializeField] private float huntTimeMin;
    [SerializeField] private float huntTimeMax;
    [SerializeField] private float huntDurationMin;
    [SerializeField] private float huntDurationMax;

    private float huntDuration;
    private float huntTimer;

    #region Getter/Setter

    //TODO: When picking up a key, the hunt timer will drop a bit
    public float HuntTimer
    {
        get { return huntTimer; }
    }

    //TODO: When the skelman searches through something the duration timer will drop a bit
    public float HuntDuration
    {
        get { return huntDuration; }
    }

    #endregion

    private void UpdateHunt()
    {
       
     
    } 


    private void CheckHuntTimer()
    {
        if (huntTimer < 0)
        {
            //TODO: Randomize huntduration between a min and max
            huntTimer = Random.Range(huntTimeMin, huntTimeMax);

            //TODO: Start hunt
            //Debug.Log("HuntStarted");
            
        }


        huntTimer -= Time.deltaTime;
    }
}
