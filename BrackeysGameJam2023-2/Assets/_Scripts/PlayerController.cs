using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public PlayerMovement movement;
    public PlayerRage rage;
    public PlayerAttacks attacks;
    

    public void PlayerUpdate()
    {
        movement.MovementUpdate();
        attacks.AttackUpdate();
    }
    
}
