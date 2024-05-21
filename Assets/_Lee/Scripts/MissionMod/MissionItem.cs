using System.Collections;
using UnityEngine;

public class MissionItem : MonoBehaviour
{
    [SerializeField] private LayerMask playerCheck;
    //[SerializeField] private GameObject box;

    private void OnTriggerEnter( Collider other )
    {
        if ( ( playerCheck.value & 1 << other.gameObject.layer ) != 0 )
        {
            Destroy(gameObject);
        }
    }

}
