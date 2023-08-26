using System;
using Unity.VisualScripting;
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
    public PlayerMovement playerMovement;

    public Vector3 attackDirection;
    public float maxAttackDistance;

    [Tooltip("No Hold = Min Curve, Max Hold = Max Curve")]
    public AnimationCurve holdAttackDistanceCurve;

    private void OnEnable()
    {
        InputManager.onPrimaryDown += StartAttack;
        InputManager.onPrimaryUp += EndAttack;
        InputManager.onMove += UpdateAttackDirection;
    }

    private void OnDisable()
    {
        InputManager.onPrimaryDown -= StartAttack;
        InputManager.onPrimaryUp -= EndAttack;
        InputManager.onMove -= UpdateAttackDirection;
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
                playerMovement.canMove = false;
                Debug.Log("stopMovement = true");
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
                playerMovement.canMove = true;
            }
            else //Else Smash
            {
                Smash();
            }
        }
        else
        {
            //Done Hopefully: Animations next
            Smash();
        }
        //Clear hold duration
        attackHoldDuration = 0;
    }

    //Attack for progressing levels
    private void Slam()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, slamRadius, Vector3.down, 2);
        foreach (RaycastHit hit in hits)
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
        float calculatedAttackDistance = maxAttackDistance * holdAttackDistanceCurve.Evaluate(attackHoldDuration / maxAttackHoldDuration);
        RaycastHit[] hits = Physics.BoxCastAll(transform.position, Vector3.one, attackDirection, Quaternion.identity, calculatedAttackDistance, LayerMask.GetMask("Smashable"));
        foreach (RaycastHit hit in hits)
        {
            GameObject hitGO = hit.collider.gameObject;
            if (hitGO.CompareTag("Smashable"))
            {
                hitGO.GetComponent<Smashable>().Smash();
            }
        }
    }

    private void UpdateAttackDirection(Vector2 dir)
    {
        if (dir != Vector2.zero)
        {
            attackDirection = new Vector3(dir.x, 0, dir.y);
        }
    }
}