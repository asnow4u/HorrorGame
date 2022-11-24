using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decay_Control : MonoBehaviour
{
    //Variable Declaration
    public Material DecayMat;
    [Header("Play the scene to update."), Range(0f, 1f)]
    public float DecayValue = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DecayMat.SetFloat("_LerpValue", DecayValue);
    }
}
