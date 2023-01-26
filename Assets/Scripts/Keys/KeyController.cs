using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    public static KeyController instance;
    public List<Transform> keyLocations;
    public List<Key> keys;
    public int numberOfKeys;
    public GameObject keyPrefab;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }    
        else
        {
            Destroy(this);
        }
    }


    void Start()
    {
        keys = new List<Key>();

        SpawnKeys();
    }

    

    public void SpawnKeys()
    {
        for (int i = 0; i < numberOfKeys; i++)
        {
            int rand = Random.Range(0, keyLocations.Count);
            GameObject go = Instantiate(keyPrefab);
            Transform trans = keyLocations[rand];
            go.transform.position = trans.position;
            go.transform.rotation = trans.rotation;

            Key k = new Key(go);

            keys.Add(k);
        }
    }
}
