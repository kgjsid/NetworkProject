using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Damage : MonoBehaviourPun
{
    [SerializeField] LayerMask damageCheckLayer;
    [SerializeField] int damage;
    [SerializeField] PlayerController ownerController;
    public PhotonView targetView;
    private int targetID;
    public int TargetID {  get { return targetID; } }
    private void OnTriggerEnter( Collider collision )
    {
        if ( ( damageCheckLayer.value & ( 1 << collision.gameObject.layer ) ) != 0 )
        {
            targetView = collision.GetComponent<PhotonView>();
            if ( targetView != null )
            {
                targetID = targetView.ViewID;
                IDamageable damagable = collision.GetComponent<IDamageable>();
                damagable?.TakeDamage(damage);
                ownerController.KillLog(targetID);
            }
        }
    }
    public void TargetIDReset()
    {
        targetID = -12;
    }
}
