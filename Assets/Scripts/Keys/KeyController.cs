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

    public int goldKeyHeldCount;
    public int silverKeyHeldCount;

    public int goldKeyUsedCount;
    public int silverKeyUsedCount;

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


    public int HeldSilverKeys
    {
        get { return silverKeyHeldCount; }
        set { silverKeyHeldCount = value; }
    }

    public int HeldGoldKeys
    {
        get { return goldKeyHeldCount; }
        set { goldKeyHeldCount = value; }
    }


    public int UsedSilverKeys
    {
        get { return silverKeyUsedCount; }
        set { silverKeyUsedCount = value; }
    }

    public int UsedGoldKeys
    {
        get { return goldKeyUsedCount; }
        set { goldKeyUsedCount = value; }
    }


    public bool KeysFound()
    {
        if (goldKeyUsedCount == goldKeyStartCount && silverKeyUsedCount == silverKeyStartCount)
        {
            return true;
        }
        else
        {
            return false;
        }
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
