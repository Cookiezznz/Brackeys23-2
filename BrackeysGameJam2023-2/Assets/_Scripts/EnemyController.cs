using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    GameObject playerObject;
    PlayerMovement playerMovement;
    float playerYPosition;

    NavMeshAgent agent;
    EnemyManager enemyManager;

    public float effectRadius = 0.3f;

    public bool attacking = false;

    float normalPlayerSpeed;

    public float roomYPosition; 



    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerObject = FindObjectOfType<PlayerController>().gameObject;
        playerMovement = playerObject.GetComponent<PlayerMovement>();
        enemyManager = FindAnyObjectByType<EnemyManager>();

        normalPlayerSpeed = playerMovement.movementSpeed;

        roomYPosition = transform.parent.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        playerYPosition = playerObject.transform.position.y;
        //Debug.Log("player position = " + playerYPosition +  " || " + );
        
        //check if the player is on the same floor 
        if ((int)playerYPosition == (int)roomYPosition)
        {
            
            agent.SetDestination(playerObject.transform.position);
            transform.LookAt(Camera.main.transform.position, Vector3.up);


            PlayerDistanceCheck();
        } 
    }

    void PlayerDistanceCheck()
    {
        float distance = Vector3.Distance(transform.position, playerObject.transform.position);

        //Debug.Log("checking distance " + distance + " : " + effectRadius);
        if (distance < effectRadius == !attacking)
        {
            //add to attacking list
            enemyManager.AddToAttack();
            attacking = true;
        }
        else if(attacking && distance > effectRadius)
        {
            //remove from attacking list 
            enemyManager.RemoveAttack();
            attacking = false;
        }
    }
}
