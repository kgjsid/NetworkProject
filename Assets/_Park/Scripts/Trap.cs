using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviourPun
{
    [SerializeField] LayerMask PlayerCheckLayer;
    [SerializeField] AudioClip boomSFX;
    [SerializeField] int count;
    [SerializeField] int safeTime;
    [SerializeField] Collider collider;
    [SerializeField] KillLogUI killLogUI;
    private void Awake()
    {
        killLogUI = FindObjectOfType<KillLogUI>();
        TrapTimer();
    }

    private void TrapTimer() // 트랩 자동 삭제
    {
        StartCoroutine(SafeTime());
        StartCoroutine(TrapCount());
    }

    private IEnumerator TrapCount()
    {
        yield return new WaitForSeconds(count);
        PhotonNetwork.Destroy(gameObject);
    }

    private IEnumerator SafeTime()
    {
        collider.enabled = false;
        yield return new WaitForSeconds(safeTime);
        collider.enabled = true;
    }

    private void OnTriggerEnter( Collider collision )
    {
        /*IShieldable targetShield = collision.GetComponent<IShieldable>();

        if (targetShield != null)
        {   // 쉴드가 있었다면
            targetShield.Shielding(collision.GetComponent<PlayerItemController>());
            // 밑은 진행하지 않음.
            Debug.Log("쉴드가 막음");
        }*/
        if ( ( PlayerCheckLayer.value & 1 << collision.gameObject.layer ) != 0 )
        {
            PhotonView targetView = collision.GetComponent<PhotonView>();
            PhotonNetwork.Instantiate("FX_Trab_Boom", transform.position, Quaternion.identity);
            Manager.Sound.PlaySFX(boomSFX);
            PhotonNetwork.Destroy(gameObject);
            photonView.RPC("TrapKillLog", RpcTarget.All, targetView.name);
        }
    }


    [PunRPC]
    private void TrapKillLog( string targetName )
    {
        killLogUI.KillLog("지뢰", targetName);

    }
}

