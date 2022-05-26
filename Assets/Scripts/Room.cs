using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private RoomController.Room room;
    [SerializeField] private int hidingSpots;
    [SerializeField] private List<Light> lights = new List<Light>();

    private RoomController controller;

    private void Start()
    {
        controller = GetComponentInParent<RoomController>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            controller.SetRoom(room);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            controller.SetRoom(RoomController.Room.None);
        }
    }


}
