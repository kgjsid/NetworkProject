using Photon.Pun;
using UnityEngine;

public class PlayerHealth : MonoBehaviourPun
{
    [SerializeField] LayerMask damageLayer;
    [SerializeField] int hp;
    public int dieViewId;
    [SerializeField] KillLogUI killLogUI; // 삭제가능

    [SerializeField] PhotonView targetView;
    private void Awake()
    {
        // 삭제가능
        killLogUI = FindObjectOfType<KillLogUI>();
    }
    private void OnTriggerEnter( Collider collision )
    {
        // 어택을 받을 때 collision이 damageChecker가 되어버림
        // 자식 오브젝트
        targetView = collision.GetComponentInParent<PhotonView>();
        Debug.Log($"{targetView.ViewID}");
        if ( targetView != null )
        {
            int targetID = targetView.ViewID;
            Debug.Log("마스터한테 요청");
            photonView.RPC("RequestAttack", RpcTarget.MasterClient, targetID);
        }
    }

    [PunRPC]
    private void RequestAttack( int targetViewID )
    {
        Debug.Log("RPC 호출");
        PhotonView targetView = PhotonNetwork.GetPhotonView(targetViewID);
        string die = targetView.name;
        IDamageable target = PhotonView.Find(targetViewID).GetComponent<IDamageable>();
        dieViewId = targetViewID;
        target?.TakeDamage(2);
    }
    // 여기서 킬로그 함수 만들어줘서 붙여주면될듯?
    // 그럼 킬로그패널에서 해야될거는?
    // 생성될때 죽인캐릭 닉네임, 죽은캐릭 닉네임 수정
    // 플레이어는 무조건 1000번부터 시작
    // 고로 1000번밑으로는 Ai 만약 dieViewId가 1000번밑이면 걍 AI라고 써주면될듯

}
