using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smashable : MonoBehaviour
{
    public float rageOnSmash;

    public static event Action<float> OnSmash;

    public bool smash;

    public void Smash()
    {
        smash = true;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (smash && collision.transform.CompareTag("FloorCube"))
        {
            OnSmash.Invoke(rageOnSmash);
            Destroy(gameObject);
        }
    }

    //private IEnumerator AnimationHalt()
    //{
    //    yield return new WaitForSeconds(0.5f);

    //    yield return null;
    //}
}