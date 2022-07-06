using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [SerializeField] private GameObject keyPanel;
    [SerializeField] private GameObject openPanel;
    [SerializeField] private GameObject hidePanel;
    [SerializeField] private GameObject leavePanel;
    [SerializeField] private GameObject turnOnPanel;
    [SerializeField] private GameObject turnOffPanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    public void ToggleKeyPanel(bool active)
    {
        keyPanel.SetActive(active);
    }
    public void ToggleOpenPanel(bool active)
    {
        openPanel.SetActive(active);
    }

    public void ToggleHidePanel(bool active)
    {
        hidePanel.SetActive(active);
    }

    public void ToggleLeavePanel(bool active)
    {
        leavePanel.SetActive(active);
    }

    public void ToggleTurnOnPanel(bool active)
    {
        turnOnPanel.SetActive(active);
    }

    public void ToggleTurnOffPanel(bool active)
    {
        turnOffPanel.SetActive(active);
    }



    public void ClearPanel()
    {
        ToggleKeyPanel(false);
        ToggleOpenPanel(false);
        ToggleHidePanel(false);
        ToggleLeavePanel(false);
        ToggleTurnOnPanel(false);
        ToggleTurnOffPanel(false);
    }
}
