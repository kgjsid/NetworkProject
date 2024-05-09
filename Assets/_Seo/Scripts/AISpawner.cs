using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class AISpawner : MonoBehaviour
{
    // AI를 스폰할 스포너 스크립트
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

            GameObject instance = PhotonNetwork.InstantiateRoomObject("TempAI", randPos, randRot);
        }
    }

    // 랜덤한 내비메시 포인트 찍기
    private void Update()
    {
        GetRandomPointOnNavMesh(transform.position, 1f, NavMesh.AllAreas);
    }

    public void GetRandomPointOnNavMesh(Vector3 center, float distance, int areaMask)
    {
        Vector3 RandomPos = Random.insideUnitSphere * distance + center;

        NavMeshHit hit;

        Debug.Log(NavMesh.SamplePosition(RandomPos, out hit, distance, areaMask));
    }

}
