using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIMove : MonoBehaviourPun, IPunObservable
{
    [SerializeField] AIController controller;

    [SerializeField] NavMeshAgent agent;
    public NavMeshAgent Agent { get { return agent; } }

    [SerializeField] Vector3 endPos; // 이동좌표
    

    float randomTime; //랜덤 이동 쿨타임
    float randomRange; //랜덤 이동 거리
    Vector3 randomPos; //랜덤 방향
    float speed;

    private void Awake()
    {
        controller = GetComponent<AIController>();
        agent = transform.GetComponent<NavMeshAgent>();
        endPos = transform.GetChild(1).position;
    }

    private void Start()
    {
        //마스터가 랜덤한 위치좌표 지정 후 OnPhotonSerializeView함수로 동기화
        if (photonView.IsMine)
            StartCoroutine(AIRandomPos());

        //좌표로 이동
        StartCoroutine(AIMoveCoroutine());
    }


    private void Update()
    {
        /*if(agent.velocity.sqrMagnitude > 1f )
        {
            controller.Animator.SetFloat("MoveSpeed", agent.speed);
        }
        else
        {
            controller.Animator.SetFloat("MoveSpeed", 0);
        }*/

        //agent.destination = endPos;
    }

    //마스터만실행
    IEnumerator AIRandomPos()
    {
        while (true)
        {
            randomTime = Random.Range(0, 5);
            randomRange = Random.Range(0, 20);
            randomPos = Random.insideUnitSphere;

            endPos = transform.position + (new Vector3(randomPos.x, 0, randomPos.z).normalized * randomRange);

            

            yield return new WaitForSeconds(randomTime);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // photonView.IsMine 내가 로컬일때
        {
            // 네트워크로 보내기
            stream.SendNext(endPos);
        }
        else // steam.IsReading , !photonView.IsMine 내가 리모트일때
        {
            // 네크워크에서 받기 , 보낸 순서대로 받아야한다.
            endPos = (Vector3)stream.ReceiveNext();
        }
    }

    IEnumerator AIMoveCoroutine()
    {
        while (true)
        {
            agent.destination = endPos;

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
