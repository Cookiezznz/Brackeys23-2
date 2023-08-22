using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomManager : MonoBehaviour
{
    public Transform buildingTransform;
    public GameObject roomPrefab;
    public List<Room> roomsList;
    public List<Room> presetRooms;
    public float roomHeight;
    public int roomsToGenerate = 50;
    public int roofNumber = 30;
    private float roofHeight;

    public static event Action OnRoomsGenerated;

    private void Start()
    {
        GenerateRooms();
    }

    private void GenerateRooms()
    {
        roofHeight = presetRooms[0].transform.position.y;
        //Add preset rooms to roomsList
        for (int roomNumber = 0; roomNumber < presetRooms.Count; roomNumber++)
        {
            Room presetRoom = presetRooms[roomNumber];
            roomsList.Add(presetRoom);

            presetRoom.PopulateRoom(roofNumber - roomNumber);
        }

        //Generate up to roomsToGenerate number of rooms
        for (int roomNumber = roomsList.Count; roomNumber < roomsToGenerate; roomNumber++)
        {
            //Create new room
            Room newRoom = Instantiate(roomPrefab, buildingTransform).GetComponent<Room>();

            //Calculate & Set the height of the room
            float yHeight = roofHeight - roomHeight * roomsList.Count;
            newRoom.transform.position = new Vector3(0, yHeight, 0);

            //Save reference
            roomsList.Add(newRoom);

            newRoom.PopulateRoom(roofNumber - roomNumber);
        }

        OnRoomsGenerated?.Invoke();
    }
}
