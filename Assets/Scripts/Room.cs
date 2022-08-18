using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private RoomController.RoomType type;
    [SerializeField] private List<Light> lights;
    [SerializeField] private Transform key;
    [SerializeField] private Transform hiding;
    [SerializeField] private Transform dormant;
    [SerializeField] private Transform observe;
    [SerializeField] private Transform wander;

    private List<Transform> keySpots;
    private List<HidingSpot> hidingSpots;
    private List<Transform> dormantSpots;
    private List<Transform> observeSpots;
    private List<Transform> wanderSpots;

    RoomController controller;

    private void Awake()
    {

        keySpots = new List<Transform>();
        hidingSpots = new List<HidingSpot>();
        dormantSpots = new List<Transform>();
        observeSpots = new List<Transform>();
        wanderSpots = new List<Transform>();

        foreach (Transform child in key)
        {
            keySpots.Add(child);
        }
        
        foreach (Transform child in hiding)
        {
            HidingSpot hidingSpot;            
            if (child.TryGetComponent<HidingSpot>(out hidingSpot))
            {
                hidingSpots.Add(hidingSpot);
            }
        }

        foreach (Transform child in dormant)
        {
            dormantSpots.Add(child);
        }

        foreach (Transform child in observe)
        {
            observeSpots.Add(child);
        }

        foreach (Transform child in wander)
        {
            wanderSpots.Add(child);
        }
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

    public List<Transform> ObserveSpots
    {
        get { return observeSpots; }
    }

    public List<Transform> WanderSpots
    {
        get { return wanderSpots; }
    }

    #endregion


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
