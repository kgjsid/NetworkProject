using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour, IUseable
{
    // 실제 플레이어의 아이템 curItemType에 들고 있는 아이템을 사용할 수 있도록 설정
    // ItemType.None -> 아이템을 들고 있지 않다
    [SerializeField] ItemType curItemType;
    public ItemType CurItemType { 
        get { return curItemType; }
        set { curItemType = value;
            switch(curItemType)
            {
                case ItemType.None:
                    itemBox_Image.sprite = null;
                    itemBox_Image.gameObject.SetActive(false);
                    break;
                default:
                    itemBox_Image.sprite = itemIcon[(int)CurItemType - 1];
                    itemBox_Image.gameObject.SetActive(true);
                    break;
            }
        }
    }

    [SerializeField] List<Sprite> itemIcon;
    [SerializeField] Image itemBox_Image;
    [Header("Item_Trap")]
    [SerializeField] Item_Trap trap;
    [SerializeField] Item_Shield shield;
    [SerializeField] Item_Transparency transparency;
    [SerializeField] Item_SmokeBomb smokebomb;

    PlayerItemController user;

    public void SetUser(PlayerItemController user)
    {
        this.user = user;
        transparency.SetUser(user);
        shield.SetUser(user);
    }

    public void GetItem(ItemType type)
    {
        // if (CurItemType != ItemType.None)    // 만약 아이템이 있는 상황에서 더 먹길 원하지 않는다면
        //    return;                           //  주석을 풀어주시면 됩니다.
        CurItemType = type;
        itemBox_Image.sprite = itemIcon[(int)CurItemType - 1];
    }

    public void Use(PlayerItemController user)
    {
        // 아이템이 있어야만 사용 가능
        if (CurItemType == ItemType.None)
        {
            return;
        }

        switch(CurItemType)
        {
            case ItemType.Trap:
                CaseTrop(user);
                break;
            case ItemType.Shield:
                CaseShield(user);
                break;
            case ItemType.Transparent:
                CaseTransparent(user);
                break;
            case ItemType.SmokeBomb:
                SmokeBomb(user);
                break;
        }

        CurItemType = ItemType.None;
    }

    private void CaseTrop(PlayerItemController user)
    {
        // 트랩을 생성하고
        // 트랩의 주인을 설정하면 트랩은 알아서 동작할 수 있도록 스크립트를 활용???
        trap.Use();
    }

    private void CaseShield(PlayerItemController user)
    {
        shield.Use();
    }

    private void CaseTransparent(PlayerItemController user)
    {
        transparency.Use();
    }

    private void SmokeBomb(PlayerItemController user)
    {
        smokebomb.Use();
    }

}

public enum ItemType
{   // 아이템을 표현할 열거형
    None,
    Trap,
    Shield,
    Transparent,
    SmokeBomb,
    Size
}
