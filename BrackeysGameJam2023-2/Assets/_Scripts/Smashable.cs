using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Smashable : MonoBehaviour
{
    public float rageOnSmash;

    public static event Action<float> OnSmash;

    public float despawnTime;
    public AnimationCurve despawnCurve;

    public float costValue;

    public string soundOnSmash;

    public void Smash()
    {
        OnSmash.Invoke(rageOnSmash);
        StartCoroutine(Despawn());
        GameStateManager.Instance.UpdateGameState(1, 0, costValue, 0);
        AudioManager.Instance.PlaySound(soundOnSmash);
    }

    IEnumerator Despawn()
    {
        float t = despawnTime;
        Vector3 localScale = transform.localScale;
        while (t > 0)
        {
            t -= Time.deltaTime;
            float multiplier = despawnCurve.Evaluate(t);
            Vector3 scale = localScale * multiplier;
            transform.localScale = scale;
            yield return null;
        }
        Destroy(gameObject);
        yield return null;
    }

}