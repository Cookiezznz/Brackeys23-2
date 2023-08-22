using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Room : MonoBehaviour
{
    public TextMeshPro roomNumber;
    public GameObject frontWallFacade;
    public float roomRevealFadeDuration;
    public AnimationCurve roomRevealFadeCurve;
    public bool isActive;

    private void Update()
    {
        if(isActive)
        {
            if(frontWallFacade.activeSelf) ActivateRoom(true);
        }
        
    }
    public void ActivateRoom(bool forceActivate = false)
    {
        if(!forceActivate)
            if (isActive) return;

        isActive = true;
        StartCoroutine(RevealRoom());
    }
    public void SetRoomNumber(int num)
    {
        roomNumber.text = num.ToString();
    }

    IEnumerator RevealRoom()
    {
        float timeDuration = 0;
        MeshRenderer renderer = frontWallFacade.GetComponent<MeshRenderer>();
        while (timeDuration < roomRevealFadeDuration)
        {
            timeDuration += Time.deltaTime;
            Material mat = renderer.material;
            float alpha = 1 - roomRevealFadeCurve.Evaluate(timeDuration / roomRevealFadeDuration);
            Color colour = mat.color;
            colour.a = alpha;
            mat.SetColor("_BaseColor", colour);
            renderer.material = mat;
            yield return null;
        }
        frontWallFacade.SetActive(false);
        yield return null;

    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            ActivateRoom();
        }
    }

}
