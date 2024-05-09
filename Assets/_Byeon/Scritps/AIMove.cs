using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMove : MonoBehaviour
{ 
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform endPos; // 이동좌표


    float randomTime; //랜덤 이동 쿨타임
    float randomRange; //랜덤 이동 거리
    Vector3 randomPos;

    private void Awake()
    {
        agent = transform.GetComponent<NavMeshAgent>();
        endPos = transform.Find("endPos");
    }

    private void Start()
    {
        StartCoroutine(AIRandomPos());
    }

    IEnumerator AIRandomPos()
    {
        while (true)
        {
            randomTime = Random.Range(0, 5);
            randomRange = Random.Range(0, 50);

            randomPos = Random.insideUnitSphere;
            endPos.position = transform.position + (new Vector3(randomPos.x, 0, randomPos.z).normalized * randomRange);
            agent.destination = endPos.position;

            yield return new WaitForSeconds(randomTime);
        }
    }
}
