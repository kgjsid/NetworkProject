using Photon.Pun;
using UnityEngine;

public class Item_Trap : MonoBehaviourPun
{
    [SerializeField] GameObject trapObj;
    [SerializeField] Transform trans;

    public void Use()
    {
        photonView.RPC("MakeTrap", RpcTarget.All, trans.position, trans.rotation);
    }
    [PunRPC]
    private void MakeTrap(Vector3 position, Quaternion rotation)
    {
        PhotonNetwork.InstantiateRoomObject(trapObj.name, position, rotation);
    }
}
