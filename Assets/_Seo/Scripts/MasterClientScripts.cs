using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class MasterClientScripts : MonoBehaviourPun
{
    // AI 동기화 작업
    // 이동은 되나 랜덤 포인트가 로컬마다 다르게 찍히니
    // AI쪽에서 요청하고 결과를 모든 클라에게 뿌려야 함

    // 1. AI입장에서 마스터에게 요청하기(랜덤으로 이동할 예정)
    // 2. 마스터가 랜덤 포인트 하나 찍기
    // 3. 해당 포인트를 요청한 AI에게 전달하기
    private void Start()
    {
        Debug.Log($"{gameObject.name} : {PhotonNetwork.IsMasterClient}");
    }
    /*
    [PunRPC]
    public void RequestRandPos(int viewID, Vector3 target)
    {
        Vector3 randPos = GetRandomPointOnNavMesh(target, 20f, NavMesh.AllAreas);

        photonView.RPC("ResultRandomPos", RpcTarget.All, viewID, randPos);
    }

    private Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance, int areaMask)
    {   // 랜덤한 내비메시 포인트 찍기
        Vector3 RandomPos = Random.insideUnitSphere * distance + center;

        NavMeshHit hit;

        NavMesh.SamplePosition(RandomPos, out hit, distance, areaMask);

        return hit.position;
    }
    */
}
