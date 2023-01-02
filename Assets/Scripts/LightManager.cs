using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public static LightManager instance;

    [SerializeField] private GameObject lightRef;

    [Header("Color")]
    [SerializeField] private Color normalColor;
    [SerializeField] private Color huntColor;
    [SerializeField] private float colorChangeDuration;

    private Color activeColor;

    private List<Light> lights;

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


    // Start is called before the first frame update
    void Start()
    {
        lights = new List<Light>(lightRef.GetComponentsInChildren<Light>());
    }


    /// <summary>
    /// Change all lights to resemble skeleton hunt
    /// </summary>
    [ContextMenu("Hunt")]
    public void ChangeToHunt()
    {
        if (activeColor == huntColor) return;

        LightController controller;

        foreach (Light light in lights)
        {
            if (light.TryGetComponent<LightController>(out controller))
            {
                StartCoroutine(controller.ChangeColor(huntColor, colorChangeDuration));
            }    
        }

        activeColor = huntColor;
    }


    /// <summary>
    /// Change all lights to resemble nothing special happening
    /// </summary>
    [ContextMenu("Normal")]
    public void ChangeToNormal()
    {
        if (activeColor == normalColor) return;

        LightController controller;

        foreach (Light light in lights)
        {
            if (light.TryGetComponent<LightController>(out controller))
            {
                StartCoroutine(controller.ChangeColor(normalColor, colorChangeDuration));
            }
        }

        activeColor = normalColor;
    }
}
