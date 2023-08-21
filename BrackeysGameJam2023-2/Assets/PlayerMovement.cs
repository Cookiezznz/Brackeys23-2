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
    private float moveAccelleration;
    Vector3 moveDirection;
    public Vector2 xConstraints;
    public Vector2 zConstraints;


    private void OnEnable()
    {
        InputManager.OnMoveUpdated += Move;
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
        if(moveAccelleration < moveAccellerationDuration)
            moveAccelleration += Time.deltaTime;

        //Calculate the next position
        Vector3 nextPos = transform.position + Time.deltaTime * movementSpeed * moveDirection * movementCurve.Evaluate(moveAccelleration / moveAccellerationDuration);

        //X Constraints
        if (nextPos.x < xConstraints.x) nextPos.x = xConstraints.x;
        if (nextPos.x > xConstraints.y) nextPos.x = xConstraints.y;
        // Y Constraints
        if (nextPos.z < zConstraints.x) nextPos.z = zConstraints.x;
        if (nextPos.z > zConstraints.y) nextPos.z = zConstraints.y;

        Rigidbody.MovePosition(nextPos);
    }

    public void Move(Vector2 movementDir)
    {
        moveDirection = new Vector3(movementDir.x, 0, movementDir.y);
    }

}
