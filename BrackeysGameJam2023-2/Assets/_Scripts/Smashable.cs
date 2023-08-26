using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smashable : MonoBehaviour
{
    public float rageOnSmash;

    public static event Action<float> OnSmash;

    public void Smash()
    {
        OnSmash.Invoke(rageOnSmash);
        Destroy(gameObject);
    }

    /*
    //TODO Replace collision with smashing of objects
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Smash();
        }
    }
    */
}