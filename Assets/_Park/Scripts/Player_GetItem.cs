using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Player_GetItem : MonoBehaviour
{
    [SerializeField] Image itemBox;    
    [SerializeField] List<Sprite> itemIcon; 
    [SerializeField] LayerMask itemBoxCheck;

    [Header("Item")]
    [SerializeField] GameObject Trap;

    private int itemCode;
    private bool itemCheack = false;

    public void GetItem()
    {
        itemBox.gameObject.SetActive(true);
        
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

    public void Using_Item()
    {
        switch (itemCode)
        {
            case 0:
                Debug.Log("트랩 아이템 사용");
                break;
            case 1:
                Debug.Log("쉴드 아이템 사용");
                break;
        }
        itemCheack = false;
        itemBox.gameObject.SetActive(false);
    }

    private void OnUsingItem()
    {
        Using_Item();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((itemBoxCheck.value & 1 << other.gameObject.layer) != 0)
        {            
            itemCode = Random.Range(0, 2);
            itemCheack = true;
            GetItem();            
        }        
    }
}
