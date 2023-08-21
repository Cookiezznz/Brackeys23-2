using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Room : MonoBehaviour
{
    public TextMeshPro roomNumber;


    public void SetRoomNumber(int num)
    {
        roomNumber.text = num.ToString();
    }
}
