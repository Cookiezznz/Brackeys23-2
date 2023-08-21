using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRage : MonoBehaviour
{
    public float rageMax;
    public float currentRage;
    public Slider rageMeter;
    public GameObject rageText;
    bool isEnraged;

    private void OnEnable()
    {
        Smashable.OnSmash += AddRage;
    }

    public void AddRage(float rageToAdd)
    {
        if (isEnraged) return;

        currentRage += rageToAdd;
        if(currentRage >= rageMax)
        {
            //Rage has just reached max
            currentRage = rageMax;
            Enrage();
        }
        rageMeter.value = currentRage / rageMax;
    }

    void Enrage()
    {
        isEnraged = true;
        rageText.gameObject.SetActive(true);
    }
}
