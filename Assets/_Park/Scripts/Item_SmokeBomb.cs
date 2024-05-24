using Photon.Pun;
using UnityEngine;

public class Item_SmokeBomb : MonoBehaviourPun
{
    [SerializeField] GameObject trapObj;
    [SerializeField] Transform trans;

    public void Use()
    {
        photonView.RPC("MakeSmokeBomb", RpcTarget.All);
    }
    [PunRPC]
    private void MakeSmokeBomb()
    {
        PhotonNetwork.Instantiate(trapObj.name, trans.position, trans.rotation);
    }
}
