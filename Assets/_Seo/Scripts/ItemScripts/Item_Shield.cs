using Photon.Pun;
using UnityEngine;

public class Item_Shield : MonoBehaviourPun
{   // 쉴드 아이템
    [SerializeField] PlayerItemController user;

    [SerializeField] Shield visiableShield;
    [SerializeField] Shield unVisiableShield;
    // 쉴드 아이템
    // 1. 사용한 유저에게만 보여야 함
    // -> 아이템을 사용하였을 때 자기 자신만 오브젝트를 켜고, 상대방에게는 켯다는 정보만, 보여지지는 않아야 함
    // 2. 상대방이 공격을 진행하면, 1회 방어
    // -> 어택을 진행할 때 쉴드에 닿았는지 그걸 체크하는 용도가 필요함

    public void SetUser(PlayerItemController user)
    {
        this.user = user;
    }

    public void Use()
    {
        OwnerUse();
        photonView.RPC("OtherUse", RpcTarget.Others);
    }

    private void OwnerUse()
    {
        visiableShield.gameObject.SetActive(true);
    }

    [PunRPC]
    private void OtherUse()
    {
        unVisiableShield.gameObject.SetActive(true);
    }
}
