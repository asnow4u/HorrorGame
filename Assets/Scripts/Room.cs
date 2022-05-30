using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private RoomController.RoomType room;
    [SerializeField] private int hidingSpots;
    [SerializeField] private List<Light> lights = new List<Light>();

    private RoomController controller;

    private void Start()
    {
        controller = GetComponentInParent<RoomController>();
    }


    public RoomController.RoomType GetRoom()
    {
        return room;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            controller.PlayerRoom = room;
        }

        if (other.gameObject.tag == "Skeleman")
        {
            controller.SkeletonRoom = room;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            controller.PlayerRoom = RoomController.RoomType.None;
        }

        if (other.gameObject.tag == "Skeleman")
        {   
            controller.SkeletonRoom = RoomController.RoomType.None;
        }
    }
}
