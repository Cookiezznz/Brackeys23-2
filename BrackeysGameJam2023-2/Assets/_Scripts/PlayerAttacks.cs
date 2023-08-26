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

    public LayerMask smashableLayer;
    public PlayerMovement direction;

    public Vector3 tempDirection;

    public Quaternion orientation = new Quaternion(1f, 1f, 1f, 1f);

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
                if (attackHoldDuration > holdDurationToShowIndicator && !attackIndicator.activeSelf) attackIndicator.SetActive(true);
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

    private void StartAttack()
    {
        //If on cooldown, do nothing...
        if (Time.time < lastAttackTime + attackCooldown) return;

        //Can only execute once
        if (attackActive) return;
        attackActive = true;

        //Reset Attack Indicator
        attackIndicatorFill.fillAmount = 0;
    }

    private void EndAttack()
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
    private void Slam()
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
    private void Smash()
    {
        // Need Validation
        Debug.DrawRay(transform.position + Vector3.up * 0.2f, direction.moveDirection, Color.yellow);
        RaycastHit[] hits = Physics.BoxCastAll(transform.position, Vector3.forward, direction.moveDirection, orientation);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Smashable"))
            {
                hit.collider.gameObject.SetActive(false);
                Debug.Log("Smash!");
                playerController.rage.AddRage(10f);
            }
        }
        SaveDirection();

        /*
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, Vector3.forward, direction.moveDirection, out hit, transform.rotation, 1.0f, smashableLayer))
        {
            GameObject smashableObject = hit.collider.gameObject;
            if (smashableObject.CompareTag("Smashable"))
            {
                Debug.Log("Smashable object detected: " + smashableObject.name);
                smashableObject.SetActive(false);
                playerController.rage.AddRage(10f);
            }
        }
        */
    }

    private void SaveDirection()
    {
        Vector3 zero = Vector3.zero;
        if (direction.moveDirection != zero)
        {
            tempDirection = direction.moveDirection;
        }
        if (direction.moveDirection == zero)
        {
            tempDirection = direction.moveDirection;
        }
    }
}