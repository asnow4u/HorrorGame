using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{

    [SerializeField] private float killDistance;
    [Range(0, 10)]
    [SerializeField] private float frequency;

    private Light light;
    private float maxIntensity;
    public float curIntensity;
    private float distance;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();

        maxIntensity = light.intensity;
        curIntensity = maxIntensity;
        distance = Mathf.Infinity;
        time = 0;
    }


    //TODO: Probably need to update after each iteration of the sine wave
    private void Update()
    {
        //distance = Vector3.Distance(transform.position, SkeletonBehavior.instance.Skeleton.transform.position);

        //if (distance < killDistance)
        //{
        //    if (light.enabled)
        //    {
        //        light.enabled = false;
        //    }

        //    return;
        //}

        ////NOTE: Well have the light turn on only after the skeloton has left
        //if (!light.enabled)
        //{
        //    light.enabled = true;
        //}

        //curIntensity = distance / 15f;

        //float intensity = curIntensity * Mathf.Sin(frequency * time) + curIntensity;

        //light.intensity = intensity;

        //time += Time.deltaTime;
    }
}
