using System;
using UnityEngine;

using UnityEngine.UI;

public class PlayerAttacks : MonoBehaviour
{
    private float attackHoldDuration;
    [Tooltip("If attackHoldDuration reaches maxAttackHoldDuration then on release, the player will slam the floor and enter the next floor.")]
    public float maxAttackHoldDuration;
    
    public float attackCooldown = 0.5f;
    private float lastAttackTime;
    public bool attackActive;
    public bool attackFullyCharged;

    public GameObject attackIndicator;
    public Image attackIndicatorFill;

    public PlayerController playerController;
    private void OnEnable()
    {
        InputManager.onPrimaryDown += StartAttack;
        InputManager.onPrimaryUp += EndAttack;
    }
    
    private void OnDisable()
    {
        InputManager.onPrimaryDown -= StartAttack;
        InputManager.onPrimaryUp -= EndAttack;
    }

    public void AttackUpdate()
    {
        //If currently attacking and not fully charged
        if (attackActive && !attackFullyCharged)
        {
            //If still charging...
            if (attackHoldDuration < maxAttackHoldDuration)
            {
                //Increment hold duration
                attackHoldDuration += Time.deltaTime;
            }
            else //Is fully charged
            {
                attackHoldDuration = maxAttackHoldDuration;
                attackFullyCharged = true;
            }
            //Update attack indicator
            float attackDurationNormalized = attackHoldDuration / maxAttackHoldDuration;
            attackIndicatorFill.fillAmount = attackDurationNormalized;
        }
        
    }
    
    void StartAttack()
    {
        //If on cooldown, do nothing...
        if (Time.time < lastAttackTime + attackCooldown) return;
        
        //Can only execute once
        if (attackActive) return;
        attackActive = true;
        
        //Show Attack Indicator
        attackIndicator.SetActive(true);
        attackIndicatorFill.fillAmount = 0;
    }
    
    void EndAttack()
    {
        //Can only execute once
        if (!attackActive) return;
        attackActive = false;
        
        //Clear hold duration
        attackHoldDuration = 0;
        //Log the last attack time for cooldown tracking.
        lastAttackTime = Time.time;

        
        //Hide Attack Indicator
        attackIndicator.SetActive(false);

        if (attackFullyCharged)
        {
            attackFullyCharged = false;
            //Only slam if fully enraged
            if (playerController.rage.isEnraged)
            {
                //TODO Implement Slam
                Slam();
            }
            else //Else Smash
            {
                Smash();
            }
        }
        else
        {
            //TODO Implement Standard Attack
            Smash();
        }
    }

    //Attack for progressing levels
    void Slam()
    {
        Debug.Log("SLAM!");
    }

    //Standard Attack Implementation: Breaks Smashables
    void Smash()
    {
        Debug.Log("Smash!");
    }

    
}
