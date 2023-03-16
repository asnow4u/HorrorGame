using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public float ySpeed = 50;
    public float itemHeight = -100f;


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
    }
}
