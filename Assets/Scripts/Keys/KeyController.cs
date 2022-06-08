using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    public static KeyController instance;

    [SerializeField] private GameObject goldKeyPrefab;
    [SerializeField] private GameObject silverKeyPrefab;

    public int goldKeyStartCount; 
    public int silverKeyStartCount;

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
        SpawnKeys();
    }


    #region Getter / Setter

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

    #endregion

    public void SpawnKeys()
    {
        List<Transform> keyLocations = new List<Transform>();
        foreach (Room room in RoomController.instance.Rooms)
        {
            keyLocations.AddRange(room.Keys);
        }

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
