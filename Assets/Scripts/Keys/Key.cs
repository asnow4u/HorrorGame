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
                UIController.instance.ToggleKeyPanel(true);

                if (Input.GetKey(KeyCode.E))
                {
                    UIController.instance.ToggleKeyPanel(false);

                    if (isGold)
                    {
                        KeyController.instance.HeldGoldKeys++;
                    }
                    else
                    {
                        KeyController.instance.HeldSilverKeys++;
                    }

                    Destroy(gameObject);
                }
            }
        }
    }


    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            UIController.instance.ToggleKeyPanel(false);
        }
    }
}
