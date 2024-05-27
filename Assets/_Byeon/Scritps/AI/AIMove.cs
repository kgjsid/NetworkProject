using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;


public class AIMove : MonoBehaviourPun
{
    [SerializeField] AIController controller;

    [SerializeField] NavMeshAgent agent;
    public NavMeshAgent Agent { get { return agent; } }

    Vector3 endPos; // 이동좌표
    float randomTime; //랜덤 이동 쿨타임
    float randomRange; //랜덤 이동 거리
    Vector3 randomPos; //랜덤 방향

    private void Awake()
    {
        //드래그앤드랍 참조 가능
        if (controller == null)
            controller = GetComponent<AIController>();
        if (agent == null)
            agent = transform.GetComponent<NavMeshAgent>();
        if (endPos == null)
            endPos = transform.GetChild(1).position;
    }

    private void Start()
    {
        if (photonView.IsMine)
            StartCoroutine(AIRandomPos());

        //좌표로 이동
        StartCoroutine(AIMoveCoroutine());
    }


    //마스터만실행
    IEnumerator AIRandomPos()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        while (true)
        {
            //마스터가 좌표를 지정
            randomTime = Random.Range(0, 5);
            randomRange = Random.Range(0, 20);
            randomPos = Random.insideUnitSphere;
            endPos = transform.position + (new Vector3(randomPos.x, 0, randomPos.z).normalized * randomRange);
            //지정된 좌표를 서버를 통해 준다
            photonView.RPC("ResultRandomPos", RpcTarget.AllViaServer, endPos);

            yield return new WaitForSeconds(randomTime);
        }
    }

    [PunRPC]
    private void ResultRandomPos(Vector3 pos)
    {
        //받은 좌료로 네비에이전트 이동
        endPos = pos;
        agent.destination = endPos;
    }

    IEnumerator AIMoveCoroutine()
    {
        while (true)
        {
            if (agent.velocity.sqrMagnitude > 1f)
            {
                controller.Animator.SetFloat("MoveSpeed", agent.speed);
            }
            else
            {
                controller.Animator.SetFloat("MoveSpeed", 0);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}