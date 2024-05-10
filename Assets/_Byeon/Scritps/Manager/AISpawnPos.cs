using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISpawnPos : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPos;
    public Transform[] SpawnPos { get { return spawnPos; } }

    private void Awake()
    {
        //SpawnManager의 자식들의 Transform을 배열에 담기
        spawnPos = new Transform[transform.childCount];

        for (int i = 0; i < spawnPos.Length; i++)
        {
            spawnPos[i] = transform.GetChild(i);
        }
    }

    private void RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        // 무작위 방향과 거리에서 포인트를 생성
        Vector3 randomDir = Random.insideUnitSphere;
        float randomRange = Random.Range(0, range);
        Vector3 randomPoint = center + new Vector3(randomDir.x, 0, randomDir.z).normalized * randomRange;
        NavMeshHit hit;
        // 최대 5미터 반경 내에서 네비메시 위의 가장 가까운 점을 찾습니다.
        if (NavMesh.SamplePosition(randomPoint, out hit, 1, NavMesh.AllAreas))
        {
            result = hit.position;
        }
        result = Vector3.zero;
       
    }
}
