using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class HostileController : MonoBehaviour
{
    [Header("Player")]
    private PlayerController player;

    [Header("Components")]
    private NavMeshAgent agent;

    private HostileManager hostileManager;
    private Rigidbody rigidbody;
    public GameObject hostilePosition;
    public GameObject playerPosition;

    [Header("Room")]
    public Room room;

    [Header("Stats")]
    public float arrestRadius = 0.3f;

    public float playerSlamExplosionForce;
    public float playerSlamExplosionRadius;
    public float explosionUpwardsModifier;

    // Start is called before the first frame update
    private void Start()
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

        Vector3 normalizedCrossProduct = Vector3.Cross(playerPosition.transform.position, hostilePosition.transform.position).normalized;
        Debug.Log(playerPosition.transform.position.x - hostilePosition.transform.position.x);
        if (normalizedCrossProduct == Vector3.zero)
        {
            hostilePosition.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            hostilePosition.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        //Move to player
        agent.SetDestination(player.transform.position);
        //Always face forwards
        transform.LookAt(Camera.main.transform.position, Vector3.up);

        CheckPlayerDistance();
    }

    private void CheckPlayerDistance()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < arrestRadius)
        {
            player.arrest.AddNearbyEnemy(this);
        }
        else
        {
            player.arrest.RemoveNearbyEnemy(this);
        }
    }

    public void Deactivate()
    {
        agent.enabled = false;
        player.arrest.RemoveNearbyEnemy(this);
        
        //rbody.useGravity = false;
        //rbody.AddExplosionForce(playerSlamExplosionForce, player.transform.position - 0.1f, playerSlamExplosionRadius, explosionUpwardsModifier, ForceMode.Impulse);
    }
}