using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour, ILight
{
    private Light light;
    private float originalIntensity;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        originalIntensity = light.intensity;
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

    public IEnumerator Flickering(float wavelength, float amplitude, float duration)
    {
        float timer = 0;
        
        while(timer < duration)
        {
            float intensityMultiplier = Mathf.Sin(2 * Mathf.PI * timer / wavelength) * amplitude + amplitude;
            light.intensity = originalIntensity * intensityMultiplier;

            timer += Time.deltaTime;
            
            yield return null;
        }

        light.intensity = originalIntensity;            
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

