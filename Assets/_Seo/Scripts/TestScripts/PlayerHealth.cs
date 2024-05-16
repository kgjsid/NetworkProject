using Photon.Pun;
using UnityEngine;

public class PlayerHealth : MonoBehaviourPun
{
    [SerializeField] LayerMask damageLayer;
    [SerializeField] int hp;
    public int dieViewId;
    [SerializeField] KillLogUI killLogUI;
    private void Awake()
    {
        killLogUI = FindObjectOfType<KillLogUI>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (damageLayer.Contain(collision.gameObject.layer))
        {
            int targetID = collision.GetComponent<PhotonView>().ViewID;
            Debug.Log("마스터한테 요청");
            photonView.RPC("RequestAttack", RpcTarget.MasterClient, targetID);
        }
    }

    [PunRPC]
    private void RequestAttack(int targetViewID)
    {
        Debug.Log("RPC 호출");
        string die = PhotonNetwork.GetPhotonView(targetViewID).name;
        IDamageable target = PhotonView.Find(targetViewID).GetComponent<IDamageable>();
        dieViewId = targetViewID;
        target?.TakeDamage(2);
        killLogUI.KillLog(PhotonNetwork.LocalPlayer.NickName, die );
    }
    // 여기서 킬로그 함수 만들어줘서 붙여주면될듯?
    // 그럼 킬로그패널에서 해야될거는?
    // 생성될때 죽인캐릭 닉네임, 죽은캐릭 닉네임 수정
    // 플레이어는 무조건 1000번부터 시작
    // 고로 1000번밑으로는 Ai 만약 dieViewId가 1000번밑이면 걍 AI라고 써주면될듯

}
