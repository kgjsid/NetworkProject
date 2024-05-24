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
        IShieldable targetShield = collision.GetComponent<IShieldable>();

        if(targetShield != null)
        {   // 쉴드가 있었다면
            targetShield.Shielding();
            // 밑은 진행하지 않음.
            return;
        }

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
