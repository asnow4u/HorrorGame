using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INoise
{
    public void MakeNoise(float range, AudioSource aSource);

    public void HeardNoise(Vector3 pos, float range);
}
