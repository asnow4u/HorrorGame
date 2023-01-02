using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour
{
    [SerializeField] private float maxLightIntensity;
    [SerializeField] private float minLightIntensity;
    [SerializeField] private float minTimer;
    [SerializeField] private float maxTimer;
    [SerializeField] private float minLength;
    [SerializeField] private float maxLength;

    private bool isLightning;
    private float timer;
    private Light light;

    private void Start()
    {
        timer = Random.Range(minTimer, maxTimer);
        light = GetComponent<Light>();
    }


    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0 && !isLightning)
        {
            StartCoroutine(PlayLightningEffect(Random.Range(minLength, maxLength)));            
        }
    }


    private IEnumerator PlayLightningEffect(float length)
    {
        float intensity = Random.Range(minLightIntensity, maxLightIntensity);
        float t = 0;
        
        isLightning = true;
        
        while (t < length / 2)
        {
            light.intensity = Mathf.Lerp(0, intensity, t / (length / 2));

            t += Time.deltaTime;

            yield return null;
        }

        while (t > 0)
        {
            light.intensity = Mathf.Lerp(0, intensity, t / (length / 2));

            t -= Time.deltaTime;

            yield return null;
        }


        timer = Random.Range(minTimer, maxTimer);
        isLightning = false;
    }


    [ContextMenu("Force Lightning")]
    public void ForceLightningEffect()
    {
        if (!isLightning)
        {
            StartCoroutine(PlayLightningEffect(Random.Range(minLength, maxLength)));
        }
    }
}
