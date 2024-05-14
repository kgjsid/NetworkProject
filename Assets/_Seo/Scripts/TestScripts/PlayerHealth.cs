using Photon.Pun;
using UnityEngine;

public class PlayerHealth : MonoBehaviourPun
{
    [SerializeField] LayerMask damageLayer;
    [SerializeField] int hp;

    private void OnTriggerEnter(Collider collision)
    {
        if (damageLayer.Contain(collision.gameObject.layer))
        {
            int targetID = collision.GetComponent<PhotonView>().ViewID;

            photonView.RPC("RequestAttack", RpcTarget.MasterClient, targetID);
        }
    }

    [PunRPC]
    private void RequestAttack(int targetViewID)
    {
        IDamageable target = PhotonView.Find(targetViewID).GetComponent<IDamageable>();

        target?.TakeDamage(2);
    }
}
