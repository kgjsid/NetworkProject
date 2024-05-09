using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] SphereCollider grondCollider;
    [SerializeField] LayerMask groundLayer;
    public static bool groundCheak;

    //private int groundCount;
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Enter");
        Debug.Log($"{collision.gameObject.layer}");
        if (groundLayer.Contain(collision.gameObject.layer))
        {            
            groundCheak = true;
            Debug.Log("바닥닿음");
            //groundCount++;
            //groundCheak = groundCount > 0;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        Debug.Log("Exit");
        Debug.Log($"{collision.gameObject.layer}");
        if (groundLayer.Contain(collision.gameObject.layer))
        {
            groundCheak = false;
            Debug.Log("바닥떨어짐");
            //groundCount--;
            //groundCheak = groundCount > 0;
        }
    }
}
