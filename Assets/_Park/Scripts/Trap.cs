using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] GameObject boom;
    [SerializeField] LayerMask PlayerCheckLayer;
    [SerializeField] AudioClip boomSFX;
    [SerializeField] int count;
    [SerializeField] int safeTime;
    [SerializeField] Collider collider;

    private void Awake()
    {
        TrapTimer();
    }

    private void TrapTimer() // 트랩 자동 삭제
    {
        StartCoroutine(SafeTime());
        StartCoroutine(TrapCount());
    }

    private IEnumerator TrapCount()
    {
        yield return new WaitForSeconds(count);
        Destroy(gameObject);
    }

    private IEnumerator SafeTime()
    {
        collider.enabled = false;
        yield return new WaitForSeconds(safeTime);
        collider.enabled = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if ((PlayerCheckLayer.value & 1 << collision.gameObject.layer) != 0)
        {
            GameObject deathEffect = Instantiate(boom, transform.position, Quaternion.identity);
            Manager.Sound.PlaySFX(boomSFX);
            Destroy(gameObject);
        }
    }
}

