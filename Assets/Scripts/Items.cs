using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Items : MonoBehaviour, INoise
{
    public float ySpeed = 50;
    public float itemHeight = -100f;
    public event Action<Items> destroyItemEvent;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        gameObject.tag = "Item";
        itemHeight = -1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, ySpeed * Time.deltaTime);
    }

    public void HideItem()
    {
        MeshRenderer rend = GetComponent<MeshRenderer>();
        Collider col = GetComponent<Collider>();

        rend.enabled = false;
        col.enabled = false;
    }

    public void ShowItem()
    {
        MeshRenderer rend = GetComponent<MeshRenderer>();
        Collider col = GetComponent<Collider>();

        rend.enabled = true;
        col.enabled = true;

        MakeNoise(5, null);
    }

    public void DestroyItem()
    {
        destroyItemEvent?.Invoke(this);
        Destroy(gameObject);
    }

    [ContextMenu("Make Noise")]
    public void MakeTestNoise()
    {
        //todo: Audio clip coming eventually
        //aSource.Play();
        INoise[] findingNoise = FindObjectsOfType<MonoBehaviour>()
            .Where(obj => obj is INoise)
            .Cast<INoise>()
            .ToArray();

        foreach (INoise noise in findingNoise)
        {
            noise.HeardNoise(transform.position, 10);
        }
    }
    public void MakeNoise(float range, AudioSource aSource)
    {
        //todo: Audio clip coming eventually
        //aSource.Play();
        INoise[] findingNoise = (INoise[])FindObjectsOfType(typeof(INoise));            

        foreach(INoise noise in findingNoise)
        {
            noise.HeardNoise(transform.position, range);
        }
    }

    public void HeardNoise(Vector3 pos, float range)
    {
        float dist = Vector3.Distance(pos, transform.position);
        if(dist < range)
        {
            Debug.Log("Played Sound");
        }
    }
}
