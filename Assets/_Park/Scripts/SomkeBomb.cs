using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBomb : MonoBehaviour
{
    [SerializeField] int count;

    private void Awake()
    {
        SmokeTimer();
    }

    private void SmokeTimer() // 트랩 자동 삭제
    {        
        StartCoroutine(SmokeCount());
    }

    private IEnumerator SmokeCount()
    {
        yield return new WaitForSeconds(count);
        Destroy(gameObject);
    }
}
