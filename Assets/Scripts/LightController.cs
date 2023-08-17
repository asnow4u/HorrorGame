using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour, ILight
{
    [Header("Flicker Properties")]
    [SerializeField] private bool flicker;
    [SerializeField] float maxFlickerIntensity;
    [SerializeField] float minFlickerIntensity;
    [SerializeField] float flickerDuration;

    private bool inFlickerRise = true;
    private float flickerInterval = 0;

    private Light light;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();

        if (flicker)
        {
            light.intensity = minFlickerIntensity;
        }
    }


    private void Update()
    {

        //Flicker
        if (flicker)
        {
            //Intensity value increasing
            if (inFlickerRise)
            {
                light.intensity = Mathf.Lerp(minFlickerIntensity, maxFlickerIntensity, flickerInterval / flickerDuration);
                flickerInterval += Time.deltaTime;

                if (flickerInterval > flickerDuration)
                {
                    inFlickerRise = false;
                    flickerInterval = flickerDuration;
                }
            }

            else
            {
                light.intensity = Mathf.Lerp(minFlickerIntensity, maxFlickerIntensity, flickerInterval / flickerDuration);
                flickerInterval -= Time.deltaTime;

                if (flickerInterval < 0)
                {
                    inFlickerRise = true;
                    flickerInterval = 0;
                }
            }
        }
    }


    public IEnumerator ChangeColor(Color color)
    {
        float time = 0;
        float duration = 1.5f;

        Color currentColor = light.color;

        while (time / duration < 1)
        {
            light.color = Color.Lerp(currentColor, color, time / duration);
            time += Time.deltaTime;

            yield return null;
        }
    }

    [ContextMenu("Turn Off")]
    public void TurnOff()
    {
        light.enabled = false;
    }

    [ContextMenu("Turn On")]
    public void TurnOn()
    {
        light.enabled = true;
    }

    public void Flicker(float wavelength, float amplitude, float duration)
    {
        
    }

    [ContextMenu("color change maybe")]
    public void SetColorBS()
    {
        SetColor(Color.blue);
    }
    public void SetColor(Color color)
    {
        StartCoroutine(ChangeColor(color));
    }
}

