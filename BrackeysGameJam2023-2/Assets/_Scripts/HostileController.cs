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
    Rigidbody rigidbody;

    [Header("Room")]
    public Room room;

    [Header("Stats")]
    public float arrestRadius = 0.3f;
    public float playerSlamExplosionForce;
    public float playerSlamExplosionRadius;
    public float explosionUpwardsModifier;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerController>();
        hostileManager = FindObjectOfType<HostileManager>();
        rigidbody = FindObjectOfType<Rigidbody>();
        
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

        if (distance < arrestRadius)
        {
            player.AddNearbyEnemy(this);
        }
        else
        {
            player.RemoveNearbyEnemy(this);
        }

    }

    public void Deactivate()
    {
        agent.enabled = false;
        player.RemoveNearbyEnemy(this);
        
        //rigidbody.useGravity = false;
        //rigidbody.AddExplosionForce(playerSlamExplosionForce, player.transform.position - 0.1f, playerSlamExplosionRadius, explosionUpwardsModifier, ForceMode.Impulse);
    }

}
