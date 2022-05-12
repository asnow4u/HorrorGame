using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public bool isGold;


    public void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {

            if (GetComponent<Renderer>().isVisible)
            {
                Debug.Log("Press E to Pick up");

                if (Input.GetKey(KeyCode.E))
                {
                    Debug.Log("Key Grabbed");

                    if (isGold)
                    {
                        KeyController.instance.IncrementGoldKey();
                    }
                    else
                    {
                        KeyController.instance.IncrementSilverKey();
                    }

                    Destroy(gameObject);
                }
            }
        }
    }
}
