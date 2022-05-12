using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{

    public static KeyController instance;

    [SerializeField] private GameObject goldKeyPrefab;
    [SerializeField] private GameObject silverKeyPrefab;
    [SerializeField] private Transform locationParent;
    
    private List<Transform> keyLocations = new List<Transform>();

    //TODO: make private
    public int goldKeyStartCount; //Harder to find keys
    public int silverKeyStartCount; //Easier to find keys

    public int goldKeysCount;
    public int silverKeysCount;
    

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
        foreach (Transform child in locationParent)
        {
            keyLocations.Add(child);
        }

        SpawnKeys();
    }


    public void IncrementSilverKey()
    {
        silverKeysCount++;
    }


    public void IncrementGoldKey()
    {
        goldKeysCount++;
    }


    public void SpawnKeys()
    {
        for (int i=0; i<goldKeyStartCount; i++)
        {
            int rand = Random.Range(0, keyLocations.Count);
            GameObject key = Instantiate(goldKeyPrefab, keyLocations[rand]);
            keyLocations.Remove(keyLocations[rand]);
        }

        for (int i = 0; i < silverKeyStartCount; i++)
        {
            int rand = Random.Range(0, keyLocations.Count);
            GameObject key = Instantiate(silverKeyPrefab, keyLocations[rand]);
            keyLocations.Remove(keyLocations[rand]);
        }
    }
}
