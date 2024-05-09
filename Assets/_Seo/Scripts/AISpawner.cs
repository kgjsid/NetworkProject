using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    // AI를 스폰할 스포너 스크립트

    // TODO... AI 스크립트 or 인터페이스 지정하기
    [SerializeField] GameObject aiPrefab;   // 스폰할 프리팹

    [SerializeField] int spawnCount;        // 스폰 카운트
    [SerializeField] int spawnMaxCount;     // 스폰 최대 수
    [SerializeField] int spawnMinCount;     // 스폰 최소 수

    public void Spawn()
    {   // TODO AI 스폰하기 + NavMesh 위에서만 스폰 가능하도록 설정해야 함
        spawnCount = Random.Range(spawnMinCount, spawnMaxCount + 1);

        for(int i = 0; i < spawnCount; i++)
        {
            Vector3 randPos = transform.position + Random.insideUnitSphere * 5f;
            randPos.y = 2f;

            Quaternion randRot = Random.rotation;
            randRot.x = 0f; randRot.z = 0f;

            GameObject instance = Instantiate(aiPrefab, randPos, randRot);
        }
    }

}
