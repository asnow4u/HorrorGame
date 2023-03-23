using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    Light light;
    public float battLife;
    public float battDegrade;
    
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponentInChildren<Light>();
        battLife = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            if(light.enabled)
            {
                TurnLightOff();
            }
            else
            {
                TurnLightOn();
            }
        }

        if (light.enabled)
        {
            battLife -= battDegrade;

            if(battLife <= 0)
            {
                TurnLightOff();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Items holding = PlayerController.instance.heldItem;
            if (holding != null)
            {
                if (holding.TryGetComponent<Battery>(out Battery battery))
                {
                    battLife = 100;
                    PlayerController.instance.heldItem = null;
                }
            }
        }


    }

    [ContextMenu("Turn light on")]
    public void TurnLightOn()
    {
        light.enabled = true;
    }

    [ContextMenu("Turn light off")]
    public void TurnLightOff()
    {
        light.enabled = false;
    }
}
