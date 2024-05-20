using System.Collections;
using UnityEngine;
using Photon.Pun;

public class ItemBox : MonoBehaviourPun
{
    [SerializeField] private LayerMask playerCheck;
    //[SerializeField] private GameObject box;
    [SerializeField] private int respawnTime;

    private IEnumerator ReCreate()
    {
        yield return new WaitForSeconds(respawnTime);
        gameObject.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((playerCheck.value & 1 << other.gameObject.layer) != 0)
        {
            if (PhotonNetwork.IsMasterClient)
                photonView.RPC("GetItemBox", RpcTarget.All);
        }
    }
    [PunRPC]
    private void GetItemBox()
    {
        gameObject.SetActive(false);
        StartCoroutine(ReCreate());
    }    
}
