using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{

    public static KeyController instance;

    [SerializeField] private List<GameObject> keyPrefabs = new List<GameObject>();
    private List<GameObject> keys = new List<GameObject>();

    public int keyCount; //TODO: make private

    public List<int> grabbedKeys = new List<int>(); //TODO: Make private

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

    public int GetKeyCount()
    {
        return keyCount;
    }


    public void AddGrabbedKey(GameObject key)
    {
        Debug.Log(key.name);
        for(int i=0; i<keyCount; i++)
        {
            if (keys[i] == key)
            {
                if (grabbedKeys.Contains(i)) return;
                grabbedKeys.Add(i);
            }
        }
    }

    #endregion





    // Update is called once per frame
    void Update()
    {
        
    }


    public void SpawnKeys()
    {
        for (int i=0; i<keyCount; i++)
        {
            //TODO: Will need to spawn randomly on a navmesh

            GameObject key = Instantiate(keyPrefabs[i], new Vector3(0 + Random.Range(0, 20), 0, Random.Range(-10, 10)), Quaternion.identity);
            keys.Add(key);
        }
    }
}
