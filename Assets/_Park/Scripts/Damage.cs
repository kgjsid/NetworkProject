using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Damage : MonoBehaviourPun
{
    [SerializeField] LayerMask damageCheckLayer;
    [SerializeField] LayerMask shieldLayer;
    [SerializeField] int damage;
    [SerializeField] PlayerAttack owner;
    public PhotonView targetView;
    private int targetID;
    public int TargetID {  get { return targetID; } }
    private void OnTriggerEnter( Collider collision )
    {
        /*if(((1 << collision.gameObject.layer) & shieldLayer) != 0)
        {   // 쉴드가 있었다면
            IShieldable targetShield = collision.GetComponent<IShieldable>();
            Debug.Log("쉴드가 있나?");
            targetShield.Shielding(collision.GetComponent<PlayerItemController>());
            Debug.Log("쉴드가 막음");
            // 밑은 진행하지 않음.
        }*/
        if ( ( damageCheckLayer.value & ( 1 << collision.gameObject.layer ) ) != 0 )
        {
            targetView = collision.GetComponent<PhotonView>();
            if ( targetView != null )
            {
                targetID = targetView.ViewID;
                IDamageable damagable = collision.GetComponent<IDamageable>();
                damagable?.TakeDamage(damage);
                owner.KillLog(targetID);
            }
        }
    }
    public void TargetIDReset()
    {
        targetID = -12;
    }

    
}
