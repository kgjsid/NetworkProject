using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static AIController;

public class ItemAIMove : MonoBehaviourPun
{
    [SerializeField] AIController controller;

    [SerializeField] NavMeshAgent agent;
    public NavMeshAgent Agent { get { return agent; } }

    //이동할 위치를 동기화하여 각자 이동 구현
    Vector3 endPos; // 이동좌표

    float randomTime; //랜덤 이동 쿨타임
    float randomRange; //랜덤 이동 거리
    Vector3 randomPos; //랜덤 방향

    int randomState;


    private void Awake()
    {
        //드래그앤드랍 참조 가능
        if(controller == null)
            controller = GetComponent<AIController>();
        if(agent == null)
            agent = transform.GetComponent<NavMeshAgent>();
        if(endPos == null)
            endPos = transform.GetChild(1).position;
    }

    private void Start()
    {
        if (photonView.IsMine)
            StartCoroutine(AIRandomPos());

        //좌표로 이동
        //StartCoroutine(AIMoveCoroutine());
    }

    private void Update()
    {
        if((transform.position - endPos).sqrMagnitude <1)
            controller.Animator.SetFloat("MoveSpeed", 0);
    }

    //마스터만실행
    IEnumerator AIRandomPos()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        while (true)
        {
            controller.State = (AIstate)Random.Range(0, 5);
            switch (controller.State)
            {
                case AIstate.Idle:
                    IdleState();
                    break;
                case AIstate.Walk:
                    WalkState();
                    break;
                case AIstate.Run:
                    RunState();
                    break;
                /*case AIstate.Attack:
                    AttackState();
                    break;
                case AIstate.Emote:
                    EmoteState();
                    break;*/
            }

            //지정된 좌표를 서버를 통해 준다
            photonView.RPC("ResultRandomPos", RpcTarget.AllViaServer, endPos);


            randomTime = Random.Range(0, 5); // 행동 변경 시간
            yield return new WaitForSeconds(randomTime);

            
            //controller.Animator.SetBool("Emote01", false);
            //controller.Animator.SetTrigger("Attack");
        }
    }

    [PunRPC]
    private void ResultRandomPos(Vector3 pos)
    {
        //받은 좌료로 네비에이전트 이동
        endPos = pos;
        agent.destination = endPos;
        //controller.Animator.SetFloat("MoveSpeed", agent.speed);
    }

    private void IdleState()
    {
        endPos = transform.position;

        controller.Animator.SetFloat("MoveSpeed", 0);
    }

    private void WalkState()
    {
        randomRange = Random.Range(1, 20); //
        randomPos = Random.insideUnitSphere;
        endPos = transform.position + (new Vector3(randomPos.x, 0, randomPos.z).normalized * randomRange);

        controller.Animator.SetFloat("MoveSpeed", 8);
    }

    private void RunState()
    {
        randomRange = Random.Range(1, 50); //
        randomPos = Random.insideUnitSphere;
        endPos = transform.position + (new Vector3(randomPos.x, 0, randomPos.z).normalized * randomRange);

        controller.Animator.SetFloat("MoveSpeed", 12);
    }

    /*private void AttackState()
    {
        endPos = transform.position;

        controller.Animator.SetFloat("MoveSpeed", 0);
        controller.Animator.SetTrigger("Attack");
    }

    private void EmoteState()
    {
        endPos = transform.position;

        controller.Animator.SetFloat("MoveSpeed", 0);
        controller.Animator.SetBool("Emote01", true);
    }*/
}
