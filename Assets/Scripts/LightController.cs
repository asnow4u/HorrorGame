using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{

    [SerializeField] private float maxIntensity;

    [Range(0, 10)]
    [SerializeField] private float frequency;

    private Light light;



    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<Light>(out light);
    }


    public IEnumerator LightShift(float time)
    {
        while (time > 0)
        {

            float intensity = maxIntensity * Mathf.Sin(frequency * time) + maxIntensity;
            light.intensity = intensity;
            Debug.Log(intensity);

            time -= Time.deltaTime;

            yield return null;
        }
    }


    //Debug
    public bool lightShift;
    private void Update()
    {
        if (lightShift)
        {
            lightShift = false;
            StartCoroutine(LightShift(100f));
        }
    }
}
