using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ItemAIMove : MonoBehaviourPun
{
    [SerializeField] AIController controller;

    [SerializeField] NavMeshAgent agent;
    public NavMeshAgent Agent { get { return agent; } }

    //이동할 위치를 동기화하여 각자 이동 구현
    [SerializeField] Vector3 endPos; // 이동좌표
    NavMeshHit hit; // 네비위로 위치 조정용
    float randomTime; //랜덤 이동 쿨타임
    float randomRange; //랜덤 이동 거리
    Vector3 randomPos; //랜덤 방향
    int agentSpeed;
    int motion;

    [SerializeField] AnimationClip attackClip;


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
    }

    private void Update()
    {
        //목표위치 도착하면 이동애니메이션 종료
        if((transform.position - endPos).sqrMagnitude < 0.25f)
        {
            controller.Animator.SetFloat("MoveSpeed", 0);
        }

        //죽으면 코르틴 종료
        if(controller.Hp == 0)
        {
            StopAllCoroutines();
        }
    }

    //마스터만실행
    IEnumerator AIRandomPos()
    {
        yield return new WaitForSeconds(Random.Range(0, 3));
        while (true)
        {
            //랜덤 상태
            controller.State = GetRandomState();
            switch (controller.State)
            {
                case AIController.AIstate.Idle:
                    IdleState();
                    break;
                case AIController.AIstate.Walk:
                    WalkState();
                    break;
                case AIController.AIstate.Run:
                    RunState();
                    break;
                /*case AIController.AIstate.Attack:
                    AttackState();
                    break;
                case AIController.AIstate.Emote:
                    EmoteState();
                    break;*/
            }

            //지정된 좌표를 서버를 통해 준다
            

            /*//공격이나,이모트 애니메이션이 끝날떄까지 대기
            if(controller.State == AIController.AIstate.Attack)
                yield return StartCoroutine(WaitForAnimationToEnd());*/

            //다음 행동 시간
            randomTime = Random.Range(1, 5);
            yield return new WaitForSeconds(randomTime);

            controller.Animator.SetBool($"Emote0{motion}", false);
        }
    }

    [PunRPC]
    private void ResultRandomPos(Vector3 pos, int agentSpeed)
    {
        //받은 좌료로 네비에이전트 이동
        endPos = pos;
        agent.destination = endPos;
        agent.speed = agentSpeed;
        controller.Animator.SetFloat("MoveSpeed", agentSpeed);
    }


    [PunRPC]
    private void ResultAttack()
    {
        endPos = transform.position;
        agent.destination = endPos;
        agentSpeed = 0;
        controller.Animator.SetTrigger("Attack");
    }

    private void IdleState()
    {
        endPos = transform.position;
        agentSpeed = 0;
        //controller.Animator.SetFloat("MoveSpeed", 0);

        photonView.RPC("ResultRandomPos", RpcTarget.AllViaServer, endPos, agentSpeed);
    }

    private void WalkState()
    {
        randomRange = Random.Range(1, 20); //
        randomPos = Random.insideUnitSphere;
        endPos = transform.position + (new Vector3(randomPos.x, 0, randomPos.z).normalized * randomRange);

        //베이크 위에만 찍히게 하기
        if (NavMesh.SamplePosition(endPos, out hit, randomRange, NavMesh.AllAreas))
        {
            endPos = hit.position;
        }

        agentSpeed = 8;
        //controller.Animator.SetFloat("MoveSpeed", 8);

        photonView.RPC("ResultRandomPos", RpcTarget.AllViaServer, endPos, agentSpeed);
    }

    private void RunState()
    {
        randomRange = Random.Range(1, 50); //
        randomPos = Random.insideUnitSphere;
        endPos = transform.position + (new Vector3(randomPos.x, 0, randomPos.z).normalized * randomRange);

        //베이크 위에만 찍히게 하기
        if (NavMesh.SamplePosition(endPos, out hit, randomRange, NavMesh.AllAreas))
        {
            endPos = hit.position;
        }

        agentSpeed = 12;
        //controller.Animator.SetFloat("MoveSpeed", 12);

        photonView.RPC("ResultRandomPos", RpcTarget.AllViaServer, endPos, agentSpeed);
    }

    /*private void AttackState()
    {
        endPos = transform.position;

        agentSpeed = 0;
        controller.Animator.SetTrigger("Attack");
    }

    private void EmoteState()
    {
        endPos = transform.position;

        agentSpeed = 0;
        //controller.Animator.SetFloat("MoveSpeed", 0);
        motion = Random.Range(0, 5);
        controller.Animator.SetBool($"Emote0{motion}", true);
    }*/


    //행동확률
    private AIController.AIstate GetRandomState()
    {
        float randomValue = Random.Range(0f, 1f);
        if (randomValue < 0.2f)
            return AIController.AIstate.Idle;
        else if (randomValue < 0.8f)
            return AIController.AIstate.Walk;
        else if (randomValue < 0.9f)
            return AIController.AIstate.Run;
        else if (randomValue < 0.95f)
            return AIController.AIstate.Attack;
        else
            return AIController.AIstate.Emote;
    }

    /*private IEnumerator WaitForAnimationToEnd()
    {
        float speed = agent.speed;
        agent.speed = 0;

        //AnimatorClipInfo[] clipInfo = controller.Animator.GetCurrentAnimatorClipInfo(0);
        float clipLength = attackClip.length;

        yield return new WaitForSeconds(clipLength);

        //controller.Animator.SetTrigger("Attack");
        agent.speed = speed;
    }*/
}
