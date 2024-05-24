using Photon.Pun;
using UnityEngine;

public class Item_Trap : MonoBehaviourPun
{
    [SerializeField] GameObject trapObj;
    [SerializeField] Transform trans;

    public void Use()
    {
        photonView.RPC("MakeTrap", RpcTarget.MasterClient);
    }
    [PunRPC]
    private void MakeTrap()
    {
        PhotonNetwork.Instantiate(trapObj.name, trans.position, trans.rotation);
    }
}
