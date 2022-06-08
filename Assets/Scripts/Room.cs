using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private RoomController.RoomType type;
    [SerializeField] private List<Light> lights;
    [SerializeField] private List<GameObject> hidingSpots;
    [SerializeField] private Transform key;
    [SerializeField] private Transform dormant;
    [SerializeField] private Transform observe;
    [SerializeField] private Transform wander;

    private List<Transform> keySpots;
    private List<Transform> dormantSpots;
    private List<Transform> observeSpots;
    private List<Transform> wanderSpots;

    private RoomController controller;

    private void Awake()
    {
        controller = GetComponentInParent<RoomController>();

        keySpots = new List<Transform>();
        dormantSpots = new List<Transform>();
        observeSpots = new List<Transform>();
        wanderSpots = new List<Transform>();

        foreach (Transform transform in key)
        {
            keySpots.Add(transform);
        }

        foreach (Transform transform in dormant)
        {
            dormantSpots.Add(transform);
        }

        foreach (Transform transform in observe)
        {
            observeSpots.Add(transform);
        }

        foreach (Transform transform in wander)
        {
            wanderSpots.Add(transform);
        }
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

    public List<GameObject> HidingSpots
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
        get { return WanderSpots; }
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
