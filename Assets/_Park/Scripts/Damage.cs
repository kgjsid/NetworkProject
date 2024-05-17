using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Damage : MonoBehaviour
{
    // 플레이어의 자식오브젝트로 있는 DamageChecker에게 붙어있음
    [SerializeField] PhotonView owner;
    [SerializeField] LayerMask damageCheckLayer;
    [SerializeField] int damage;

    private void Start()
    {   // 주인이 누군지 파악하기 위해서 포톤뷰 캐싱하기
        owner = GetComponentInParent<PhotonView>();    
    }

    private void OnTriggerEnter(Collider collision)
    {   // 실제 데미지처리
        if ((damageCheckLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            IDamageable damagable = collision.GetComponent<IDamageable>();
            damagable?.TakeDamage(damage);
            Debug.Log($"{owner.name}의 공격");
        }
    }    
}
