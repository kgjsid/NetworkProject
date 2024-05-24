using System.Collections;
using UnityEngine;
using Photon.Pun;

public class ItemBox : MonoBehaviourPun
{

    [SerializeField] private LayerMask playerCheck;
    [SerializeField] GameObject box;    
    [SerializeField] private int respawnTime;

    public bool boxStatus = true;

    private IEnumerator ReCreate()
    {
        yield return new WaitForSeconds(respawnTime);
        if (box != null)
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
            boxStatus = true;
            box.gameObject.SetActive(true);
        }
    }

/*    private void OnTriggerEnter(Collider other)
    {
        if ((playerCheck.value & 1 << other.gameObject.layer) != 0)
        {
        }
    }
*/
    public void GetTwoItem()
    {
        photonView.RPC("GetItemBox", RpcTarget.All);

    }
    [PunRPC]
    private void GetItemBox()
    {
        if (box != null)
        {            
            boxStatus = false;
            box.gameObject.SetActive(false);     
            gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(ReCreate());
        }
    }
}
