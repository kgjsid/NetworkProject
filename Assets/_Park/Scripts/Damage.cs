using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] LayerMask damageCheckLayer;
    [SerializeField] int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((damageCheckLayer.value & 1 << collision.gameObject.layer) != 0)
        {
            IDamageable damagable = collision.GetComponent<IDamageable>();
            damagable?.TakeDamage(damage);
        }
    }    
}
