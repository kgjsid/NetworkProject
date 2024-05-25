using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Unity.VisualScripting;

public class Player_GetItem : MonoBehaviourPun
{
    [SerializeField] Image itemBox;
    [SerializeField] List<Sprite> itemIcon;
    [SerializeField] LayerMask itemBoxCheck;

    [Header("Item")]
    [SerializeField] GameObject Trap;

    private int itemCode;
    private bool itemCheack = false;

    private ItemBox itemBoxScript;

    private void Start()
    {
        itemBoxScript = GetComponent<ItemBox>();
    }

    public void GetItem()
    {
        itemCode = Random.Range(0, 2);  //아이템 랜덤
        itemCheack = true;              //아이템 소유여부
        itemBox.gameObject.SetActive(true);//아이템 아이콘 on
        switch (itemCode)
        {
            case 0:
                itemBox.sprite = itemIcon[0];
                break;
            case 1:
                itemBox.sprite = itemIcon[1];
                break;
        }
    }

    [PunRPC]
    public void Using_Item()
    {
        if (itemCheack != false)
        {
            switch (itemCode)
            {
                case 0:
                    
                    break;
                case 1:               
                    break;
            }
            itemCheack = false;
            itemBox.gameObject.SetActive(false);
        }
    }

    private void OnUsingItem()
    {
        photonView.RPC("Using_Item", RpcTarget.All);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((itemBoxCheck.value & 1 << other.gameObject.layer) != 0)
        {
            GetItem();
            other.gameObject.GetComponent<ItemBox>().GetTwoItem();
            if (itemBoxScript != null && itemBoxScript.boxStatus == true)
            {
            }
        }
    }
}
