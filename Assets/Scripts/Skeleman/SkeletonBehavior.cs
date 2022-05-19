using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBehavior : MonoBehaviour
{

    /*Requirements
     * Similar to slender man, the more keys that are gathered the more active the skeleman becomes
     * States of activity => playing dead, observing, wander, hunt  
     * Playing dead: the skeleman will teleport around when not seen. And will be in places like on the stairs, in the kitchen, ect.
     * observing: Knows that others exist in the house and will be seen sitting in places like on the couch, standing in corners, watching.
     * wander: Skeleman will start to wander (this will start with a resurection happening when the player is close). will stop occationally. At this point the skeleman is a threat. Staring at the skeleman or being in sight will proceed it to the next stage faster. (no more teleports at this point)
     * Haunt: Skeleman is activally hunting. 
     * 
     * States of pursuit => hunting, chasing, searching
     * 
     * 
    */


    private enum State {dorment, observe, wander, hunt};
    private State state;

    // Start is called before the first frame update
    void Start()
    {
        state = State.dorment;
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    public void NextState()
    {
        switch (state)
        {
            case State.dorment:

                state = State.observe;
                break;

            case State.observe:

                state = State.wander;
                break;

            case State.wander:

                state = State.hunt;
                break;

        }
    }
}
