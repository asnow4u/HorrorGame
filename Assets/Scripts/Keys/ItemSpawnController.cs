using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnController : MonoBehaviour
{
    public static ItemSpawnController instance;

    public GameObject keyPrefab;
    public GameObject batteryPrefab;

    public List<Transform> locations;
    public int numberOfKeys;
    public int numberOfBatteries;
    
    private List<Key> keys;
    private List<Battery> batteries;


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
        if (locations.Count < (numberOfKeys + numberOfBatteries))
        {
            Debug.LogError("To Many Keys and Batteries for Locations provided");
        }

        keys = new List<Key>();
        batteries = new List<Battery>();

        SpawnKeys();
        SpawnBatteries();
    }

    

    public void SpawnKeys()
    {
        for (int i = 0; i < numberOfKeys; i++)
        {
            int rand = Random.Range(0, locations.Count);
            GameObject go = Instantiate(keyPrefab);
            Transform trans = locations[rand];
            locations.RemoveAt(rand);
            go.transform.position = trans.position;
            go.transform.rotation = trans.rotation;

            Key k = go.AddComponent<Key>();

            keys.Add(k);
        }
    }

    public void SpawnBatteries()
    {
        for (int i = 0; i < numberOfBatteries; i++)
        {
            int rand = Random.Range(0, locations.Count);
            GameObject go = Instantiate(batteryPrefab);
            Transform trans = locations[rand];
            locations.RemoveAt(rand);
            go.transform.position = trans.position;
            go.transform.rotation = trans.rotation;

            Battery battery = go.AddComponent<Battery>();

            batteries.Add(battery);
        }
    }
}
