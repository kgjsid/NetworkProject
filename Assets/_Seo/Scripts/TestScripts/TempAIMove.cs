using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class TempAIMove : MonoBehaviourPun
{
    // AI 테스트 이동 코드
    Coroutine moveRoutine;

    [SerializeField] float choiceInterval;
    [SerializeField] AIState curState;
    [SerializeField] NavMeshAgent agent;

    [SerializeField] TempAIMove obj;

    private void Start()
    {
        moveRoutine = StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        while(true)
        {
            curState = (AIState)Random.Range(0, 2);

            switch(curState)
            {
                case AIState.Walking:
                    yield return GoRandomPos();
                    break;
                case AIState.Stopping:
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
        photonView.RPC("RequestRandPos", RpcTarget.MasterClient, obj);
        // Vector3 randPos = GetRandomPointOnNavMesh(transform.position, 20f, NavMesh.AllAreas);
        // agent.SetDestination(randPos);

        // yield return new WaitUntil(() => ((Vector3.SqrMagnitude(randPos) < 50f) || time++ > 7));
        yield return null;
    }

    public void SetRandPos()
    {

    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}

public enum AIState
{
    Walking,
    Stopping
}