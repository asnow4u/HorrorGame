using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [Header("Flicker Properties")]
    [SerializeField] private bool flicker;
    [SerializeField] float maxFlickerIntensity;
    [SerializeField] float minFlickerIntensity;
    [SerializeField] float flickerDuration;

    private bool inFlickerRise = true;
    private float flickerInterval = 0;

    private Light light;
    private bool changeingColor;

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
        if (changeingColor) return;

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


    public IEnumerator ChangeColor(Color color, float duration)
    {
        changeingColor = true;
        float currentIntensity = light.intensity;
        float time = 0;

        //while (light.intensity > 0)
        //{
        //    light.intensity = Mathf.Lerp(currentIntensity, 0, time / lightChangeDuration);
        //    time += Time.deltaTime;

        //    yield return null;
        //}

        //light.color = color;

        //while (light.intensity < currentIntensity)
        //{
        //    light.intensity = Mathf.Lerp(currentIntensity, 0, time / lightChangeDuration);
        //    time -= Time.deltaTime;

        //    yield return null;
        //}

        Color currentColor = light.color;

        while (time / duration < 1)
        {
            light.color = Color.Lerp(currentColor, color, time / duration);
            time += Time.deltaTime;

            yield return null;
        }


        light.intensity = currentIntensity;
        changeingColor = false;
    }
}

