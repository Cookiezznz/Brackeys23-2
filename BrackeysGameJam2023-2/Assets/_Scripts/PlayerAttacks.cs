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
    public float slamRadius;

    public GameObject attackIndicator;
    public float holdDurationToShowIndicator;
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
                if(attackHoldDuration > holdDurationToShowIndicator && !attackIndicator.activeSelf) attackIndicator.SetActive(true);
            }
            else //Is fully charged
            {
                attackHoldDuration = maxAttackHoldDuration;
                attackFullyCharged = true;
            }

            //0 - 1 Value of attack power
            float attackDurationNormalized = attackHoldDuration / maxAttackHoldDuration;

            //Update attack indicator
            float attackIndicatorFillAmount = (attackHoldDuration - holdDurationToShowIndicator) / (maxAttackHoldDuration - holdDurationToShowIndicator);
            attackIndicatorFill.fillAmount = attackIndicatorFillAmount;
        }
        
    }
    
    void StartAttack()
    {
        //If on cooldown, do nothing...
        if (Time.time < lastAttackTime + attackCooldown) return;
        
        //Can only execute once
        if (attackActive) return;
        attackActive = true;
        
        //Reset Attack Indicator
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
        Debug.DrawRay(transform.position + Vector3.up * 0.2f, Vector3.down, Color.red, 5);
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, slamRadius, Vector3.down, 2);
        foreach(RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Floor"))
            {
                hit.collider.enabled = false;
            }

            if (hit.collider.gameObject.CompareTag("FloorCube"))
            {
                hit.collider.gameObject.GetComponent<FloorCube>().Break();
            }
        }
        playerController.rage.ClearRage();
    }

    //Standard Attack Implementation: Breaks Smashables
    void Smash()
    {
        Debug.Log("Smash!");
    }

    
}
