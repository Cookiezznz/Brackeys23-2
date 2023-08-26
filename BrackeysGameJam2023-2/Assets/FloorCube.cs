using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCube : MonoBehaviour
{
    public float despawnDuration;
    public float breakForce;

    public void Break()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.AddForce(Vector3.down * breakForce);
        Destroy(gameObject, despawnDuration);
    }


}
