using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemAIMove : MonoBehaviour
{
    [SerializeField] AIController controller;

    [SerializeField] NavMeshAgent agent;
    public NavMeshAgent Agent { get { return agent; } }

    [SerializeField] Transform endPos; // 이동좌표


    float randomTime; //랜덤 이동 쿨타임
    float randomRange; //랜덤 이동 거리
    Vector3 randomPos; //랜덤 방향

    private void Awake()
    {
        controller = GetComponent<AIController>();
        agent = transform.GetComponent<NavMeshAgent>();
        endPos = transform.GetChild(1);
    }

    private void Start()
    {
        //ai 랜덤한 위치로 이동
        StartCoroutine(AIRandomPos());
    }

    private void Update()
    {
        if (agent.velocity.sqrMagnitude > 1f)
        {
            controller.Animator.SetFloat("MoveSpeed", agent.speed);
        }
        else
        {
            controller.Animator.SetFloat("MoveSpeed", 0);
        }
    }

    IEnumerator AIRandomPos()
    {
        while (true)
        {

            randomTime = Random.Range(0, 5);
            randomRange = Random.Range(0, 20);
            randomPos = Random.insideUnitSphere;

            endPos.position = transform.position + (new Vector3(randomPos.x, 0, randomPos.z).normalized * randomRange);
            agent.destination = endPos.position;


            yield return new WaitForSeconds(randomTime);
        }
    }
}
