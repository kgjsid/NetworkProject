using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiMove : MonoBehaviour
{
    [SerializeField] Transform trans;
    [SerializeField] Animator aiAni;
    [SerializeField] int moveSpeed;
    private Vector3 moveDir;

    private void Update()
    {
        AiMoveing();
    }

    private void AiMoveing()
    {
        int rotate = Random.Range(0, 360);
        int moveDistance = Random.Range(0, 10);

        moveDir = Quaternion.Euler(0, rotate, 0) * Vector3.forward * moveDistance * moveSpeed*Time.deltaTime;

        StartCoroutine(AiMoveCoolTime());
    }

    private IEnumerator AiMoveCoolTime()
    {
        int randomTime = Random.Range(0, 5);
        yield return new WaitForSeconds(randomTime);
    }
}
