using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TVController : MonoBehaviour
{
    [SerializeField] private GameObject screen;
    [SerializeField] private Material staticScreen;
    [SerializeField] private Material blankScreen;

    private VideoPlayer player;
    private Renderer rend;
    private Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        if (screen != null)
        {
            player = screen.GetComponent<VideoPlayer>();
            rend = screen.GetComponent<Renderer>();
            collider = screen.GetComponent<BoxCollider>();
        }
    }


    public void ToggleTV()
    {
        if (!player.isPlaying)
        {
            rend.material = staticScreen;
            player.Play();
        }

        else
        {
            rend.material = blankScreen;
            player.Stop();
        }
    }


    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (player.isPlaying)
            {
                UIController.instance.ToggleTurnOffPanel(true);
                UIController.instance.ToggleTurnOnPanel(false);
            }
            else
            {
                UIController.instance.ToggleTurnOffPanel(false);
                UIController.instance.ToggleTurnOnPanel(true);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleTV();
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        { 
            UIController.instance.ToggleTurnOffPanel(false);
            UIController.instance.ToggleTurnOnPanel(false);
        }
    }
}
