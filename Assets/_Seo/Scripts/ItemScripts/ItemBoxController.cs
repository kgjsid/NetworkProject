using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoxController : MonoBehaviourPun
{
    // 아이템 박스들을 컨트롤할 컨트롤러. 현재는 아이템 획득하면 박스가 사라지고, 다시 생성할 수 있도록
    [SerializeField] List<ItemBox_> itemBoxes;
    [SerializeField] float SpawnTime;

    private void Start()
    {
        for(int i = 0; i < itemBoxes.Count; i++)
        {
            itemBoxes[i].SetController(this, i);
        }
    }

    public void StartHideRoutine(int indexNumber)
    {
        // HideRoutine(itemBox);
        photonView.RPC("HideBox", RpcTarget.All, indexNumber);
    }

    [PunRPC]
    public void HideBox(int indexNumber)
    {
        StartCoroutine(HideRoutine(indexNumber));
    }


    IEnumerator HideRoutine(int indexNumber)
    {
        itemBoxes[indexNumber].gameObject.SetActive(false);
        yield return new WaitForSeconds(SpawnTime);
        itemBoxes[indexNumber].gameObject.SetActive(true);
    }
}
