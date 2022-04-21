using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            KeyController.instance.AddGrabbedKey(gameObject);
            Destroy(gameObject);
        }
    }
}
