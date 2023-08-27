using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public PlayerMovement movement;

    public PlayerRage rage;
    public PlayerAttacks attacks;
    public PlayerArrest arrest;

    public void PlayerUpdate()
    {
        if (arrest.isArrested) return;
        movement.MovementUpdate();
        attacks.AttackUpdate();
        arrest.UpdateArrest();
    }






}
