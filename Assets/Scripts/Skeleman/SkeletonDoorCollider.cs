using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDoorCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Door")
        {
            if (SkeletonStateManager.instance.curState != SkeletonStateManager.State.dormant)
            {
                DoorController controller = col.gameObject.GetComponentInParent<DoorController>();

                if (controller != null)
                {
                    if (!controller.IsOpen)
                    {
                        controller.Force(15f, transform.position);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Door")
        {
            if(SkeletonStateManager.instance.curState != SkeletonStateManager.State.dormant)
            {
                DoorController controller = col.gameObject.GetComponentInParent<DoorController>();

                if (controller != null)
                {
                    if (controller.IsOpen)
                    {
                        controller.Force(15f, transform.position);
                    }
                }
            }
        }
    }
}
