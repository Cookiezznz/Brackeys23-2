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

    [Header("Arrest Information")] 
    public List<HostileController> nearbyHostiles = new List<HostileController>();

    public Slider arrestSlider;
    public float arrest;
    public float arrestThreshhold;
    public float arrestPerSecondPerHostile;
    public float arrestReductionSpeedMultiplier;
    public bool isArrested;
    public event Action onArrest;

    public void PlayerUpdate()
    {
        if (isArrested) return;
        movement.MovementUpdate();
        attacks.AttackUpdate();

        UpdateArrest();
    }

    public void AddNearbyEnemy(HostileController hostile)
    {
        if (!nearbyHostiles.Contains(hostile))
            nearbyHostiles.Add(hostile);
    }

    public void RemoveNearbyEnemy(HostileController hostile)
    {
        if(nearbyHostiles.Contains(hostile))
            nearbyHostiles.Remove(hostile);
    }

    public void UpdateArrest()
    {
        

        arrest += (nearbyHostiles.Count > 0 ? arrestPerSecondPerHostile * nearbyHostiles.Count : -arrestPerSecondPerHostile * arrestReductionSpeedMultiplier) * Time.deltaTime;
        arrest = MathF.Max(0, arrest);

        //Activate slider
        if (arrest > 0 && !arrestSlider.gameObject.activeSelf)
        {
            arrestSlider.gameObject.SetActive(true);
        }
        //Deactivate Slider
        else if (arrest <= 0 && arrestSlider.gameObject.activeSelf)
        {
            arrestSlider.gameObject.SetActive(false);
        }

        if(arrest > 0)
            arrestSlider.value = arrest / arrestThreshhold;

        if (arrest > arrestThreshhold)
        {
            Arrest();
        }
    }

    void Arrest()
    {
        if (isArrested) return;
        isArrested = true;
        onArrest?.Invoke();
    }





}
