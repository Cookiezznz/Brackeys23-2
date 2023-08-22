using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody Rigidbody;
    [Header("Properties")]
    public bool canMove = false;
    public float movementSpeed;
    public AnimationCurve movementCurve;
    public float moveAccellerationDuration;
    public float moveAccelleration;
    Vector3 moveDirection;
    public Vector2 xConstraints;
    public Vector2 zConstraints;


    private void OnEnable()
    {
        InputManager.onMove += UpdateMoveDirection;
    }

    private void OnDisable()
    {
        
    }
    // Update is called once per frame
    public void MovementUpdate()
    {
        //Movement viability checks
        if (!canMove) return;
        if (moveDirection == Vector3.zero)
        {
            //Reset accelleration if player has stopped moving
            if (moveAccelleration > 0) moveAccelleration = 0;
            return;
        }

        //Increment moveAccelleration if needed
        if (moveAccelleration < moveAccellerationDuration)
            moveAccelleration = Mathf.Min(moveAccelleration += Time.deltaTime, moveAccellerationDuration);

        //Calculate the next position
        Vector3 nextPos = Time.deltaTime * movementSpeed * movementCurve.Evaluate(moveAccelleration / moveAccellerationDuration) * moveDirection;
        nextPos += transform.position;

        //X Constraints
        if (nextPos.x < xConstraints.x) nextPos.x = xConstraints.x;
        if (nextPos.x > xConstraints.y) nextPos.x = xConstraints.y;
        // Y Constraints
        if (nextPos.z < zConstraints.x) nextPos.z = zConstraints.x;
        if (nextPos.z > zConstraints.y) nextPos.z = zConstraints.y;

        Rigidbody.MovePosition(nextPos);
    }

    public void UpdateMoveDirection(Vector2 movementDir)
    {
        moveDirection = new Vector3(movementDir.x, 0, movementDir.y);
    }

}
