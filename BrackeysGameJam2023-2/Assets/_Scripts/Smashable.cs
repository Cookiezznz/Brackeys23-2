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
        StartCoroutine(AnimationHalt());
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

    private IEnumerator AnimationHalt()
    {
        yield return new WaitForSeconds(2f);
        OnSmash.Invoke(rageOnSmash);
        Destroy(gameObject);
        yield return null;
    }
}