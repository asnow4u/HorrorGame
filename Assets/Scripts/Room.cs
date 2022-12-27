using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private RoomController.RoomType type;
    [SerializeField] private List<Light> lights;

    private List<Transform> keySpots;
    private List<HidingSpot> hidingSpots;
    private List<Transform> dormantSpots;
    private List<Transform> wanderSpots;

    RoomController controller;

    private void Awake()
    {

        keySpots = new List<Transform>();
        dormantSpots = new List<Transform>();
        wanderSpots = new List<Transform>();

        //Get all key locations
        Transform key = transform.Find("Keys");
        foreach (Transform child in key)
        {
            keySpots.Add(child);
        }

        Transform loc = transform.Find("Locations");

        //Get all skeleton dormant locations
        Transform dormant = loc.Find("DormantLoc");
        foreach (Transform child in dormant)
        {
            dormantSpots.Add(child);
        }

        //Get all skeleton wander locations
        Transform wander = loc.Find("WanderLoc");
        foreach (Transform child in wander)
        {
            wanderSpots.Add(child);
        }


        hidingSpots = new List<HidingSpot>();
        hidingSpots = GetComponentsInChildren<HidingSpot>().ToList();
    }

    private void Start()
    {
        controller = RoomController.instance;
        controller.Rooms.Add(this);
    }


    #region Getter / Setter

    public RoomController.RoomType Type
    {
        get { return type; }
    }

    public List<Light> Lights
    {
        get { return lights; }
    }

    public BoxCollider Collider
    {
        get { return GetComponent<BoxCollider>(); }
    }

    public List<HidingSpot> HidingSpots
    {
        get { return hidingSpots; }
    }

    public List<Transform> Keys
    {
        get { return keySpots; }
    }

    public List<Transform> DormantSpots
    {
        get { return dormantSpots; }
    }

    public List<Transform> WanderSpots
    {
        get { return wanderSpots; }
    }

    #endregion

    /// <summary>
    /// Return Random Hiding Spot
    /// </summary>
    public HidingSpot GetRandomHidingSpotWithOutPlayer()
    {
        //No hiding spots
        if (hidingSpots.Count == 0)
        {
            return null;
        }

        //One Hiding Spot
        if (hidingSpots.Count == 1)
        {
            return hidingSpots[0];
        }

        //Randomize Hiding Spot
        List<HidingSpot> availableSpots = new List<HidingSpot>();
        foreach (HidingSpot hidingSpot in hidingSpots)
        {
            if (hidingSpot.IsPlayerHiding) continue;
            availableSpots.Add(hidingSpot);
        }

        int randSpot = Random.Range(0, availableSpots.Count);

        Debug.Log("Skeleton: Hidingspot selected " + availableSpots[randSpot].transform.parent.name);

        return availableSpots[randSpot];
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            controller.PlayerRoom = this;
        }

        if (other.gameObject.tag == "Skeleman")
        {
            controller.SkeletonRoom = this;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            controller.PlayerRoom = null;
        }

        if (other.gameObject.tag == "Skeleman")
        {   
            controller.SkeletonRoom = null;
        }
    }
}
