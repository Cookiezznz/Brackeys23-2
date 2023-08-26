using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HostileController : MonoBehaviour
{
    [Header("Player")]
    PlayerController player;

    [Header("Components")]
    NavMeshAgent agent;
    HostileManager hostileManager;

    [Header("Room")]
    public Room room;

    [Header("Stats")]
    public float effectRadius = 0.3f;

    public bool attacking = false;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerController>();
        hostileManager = FindAnyObjectByType<HostileManager>();
        
    }

    // Update is called once per frame
    public void HostileUpdate()
    {
        //if the current room is not active... dont update
        if (!room.isActive) return;
        
        //Move to player
        agent.SetDestination(player.transform.position);
        //Always face forwards
        transform.LookAt(Camera.main.transform.position, Vector3.up);

        CheckPlayerDistance();
    }

    void CheckPlayerDistance()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < effectRadius)
        {
            player.AddNearbyEnemy(this);
        }
        else
        {
            player.RemoveNearbyEnemy(this);
        }

    }

}
