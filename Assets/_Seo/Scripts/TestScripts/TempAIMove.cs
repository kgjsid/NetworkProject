using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using System.Runtime.CompilerServices;

public class TempAIMove : MonoBehaviourPun, IDamageable
{
    // AI 테스트 이동 코드
    Coroutine moveRoutine;

    [SerializeField] float choiceInterval;
    [SerializeField] AIState curState;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] Collider collider;

    [SerializeField] LayerMask damageLayer;

    Vector3 randPos = Vector3.zero;

    private void Start()
    {
        BaseGameScene.Instance.masterChangeEvent.AddListener(StartMoveRoutine);
    }

    private void StartMoveRoutine()
    {
        if (PhotonNetwork.IsMasterClient)
            moveRoutine = StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        yield return new WaitForSeconds(1f);
        while(true)
        {
            curState = (AIState)Random.Range(0, 2);

            switch(curState)
            {
                case AIState.Walking:
                    animator.SetFloat("MoveSpeed", 8f);
                    yield return GoRandomPos();
                    break;
                case AIState.Stopping:
                    animator.SetFloat("MoveSpeed", 0f);
                    yield return new WaitForSeconds(Random.Range(1, 3));
                    break;
            }

            yield return new WaitForSeconds(choiceInterval);
        }
    }

    IEnumerator GoRandomPos()
    {
        int time = 0;

        // 마스터에게 요청
        RequestRPC();
        // Vector3 randPos = GetRandomPointOnNavMesh(transform.position, 20f, NavMesh.AllAreas);
        agent.SetDestination(randPos);

        yield return new WaitUntil(() => ((Vector3.SqrMagnitude(randPos) < 50f) || time++ > 7));
        yield return null;
    }

    private void RequestRPC()
    {
        photonView.RPC("RequestRandPos", RpcTarget.MasterClient, photonView.ViewID, transform.position);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    [PunRPC]
    public void RequestRandPos(int viewID, Vector3 target)
    {
        Vector3 randPos = GetRandomPointOnNavMesh(target, 20f, NavMesh.AllAreas);

        photonView.RPC("ResultRandomPos", RpcTarget.All, viewID, randPos);
    }
    [PunRPC]
    private void ResultRandomPos(int viewID, Vector3 target)
    {
        if (viewID != photonView.ViewID)
            return;

        randPos = target;
    }

    private Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance, int areaMask)
    {   // 랜덤한 내비메시 포인트 찍기
        Vector3 RandomPos = Random.insideUnitSphere * distance + center;

        NavMeshHit hit;

        NavMesh.SamplePosition(RandomPos, out hit, distance, areaMask);

        return hit.position;
    }

    private void Die()
    {
        animator.SetTrigger("Die");
        collider.enabled = false;
        agent.speed = 0f;
        agent.isStopped = false;
        agent.enabled = false;
    }

    public void TakeDamage(int damage)
    {
        Die();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (damageLayer.Contain(collision.gameObject.layer))
        {
            TakeDamage(2);
        }
    }
}

public enum AIState
{
    Walking,
    Stopping
}