using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public PlayerMovement movement;
    

    public void PlayerUpdate()
    {
        movement.MovementUpdate();

    }
}
