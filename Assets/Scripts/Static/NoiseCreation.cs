using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class NoiseCreation
{
    public static void CreateNoise(float range, Vector3 pos)
    {
        //todo: Audio clip coming eventually
        //aSource.Play();

        INoise[] findingNoise = (GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[])
            .SelectMany(g => g.GetComponents(typeof(INoise)))
            .Cast<INoise>()
            .ToArray();


        foreach (INoise noise in findingNoise)
        {
            noise.HeardNoise(pos, range);
        }
    }
}
