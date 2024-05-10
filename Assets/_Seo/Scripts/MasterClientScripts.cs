using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class MasterClientScripts : MonoBehaviourPun
{
    // 마스터 클라이언트만 해당 스크립트를 켜기??
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
