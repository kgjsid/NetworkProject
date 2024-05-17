using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    [SerializeField] private LayerMask playerCheck; 
    [SerializeField] private GameObject box; 
    [SerializeField] private int count; 

    private IEnumerator ReCreate()
    {        
        yield return new WaitForSeconds(count); 
        box.SetActive(true); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((playerCheck.value & 1 << other.gameObject.layer) != 0)
        {
            box.SetActive(false);
            StartCoroutine(ReCreate()); 
        }
    }
}
