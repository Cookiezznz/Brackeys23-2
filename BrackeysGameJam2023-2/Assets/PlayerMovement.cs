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
            if (moveAccelleration > 0) moveAccelleration = 0;
            return;
        }

        //Increment moveAccelleration if needed
        if(moveAccelleration < moveAccellerationDuration)
            moveAccelleration += Time.deltaTime;

        Rigidbody.MovePosition(transform.position + Time.deltaTime * movementSpeed * moveDirection * movementCurve.Evaluate(moveAccelleration / moveAccellerationDuration));
    }

    public void Move(Vector2 movementDir)
    {
        moveDirection = new Vector3(movementDir.x, 0, movementDir.y);
    }

}
