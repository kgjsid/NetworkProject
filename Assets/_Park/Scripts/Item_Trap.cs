using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Trap : MonoBehaviour
{
    [SerializeField] GameObject boom;
    [SerializeField] LayerMask PlayerCheckLayer;
    [SerializeField] int count;

    private void Start()
    {
        Trap();
    }

    private void Trap() // 트랩 자동 삭제
    {
        StartCoroutine(TrapCount());        
    }

    private IEnumerator TrapCount()
    {
        yield return new WaitForSeconds(count);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if ((PlayerCheckLayer.value & 1 << collision.gameObject.layer) != 0)
        {
            GameObject deathEffect = Instantiate(boom, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

