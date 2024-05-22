using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IUseable
{
    [SerializeField] ItemType curItemType;
    public ItemType CurItemType { 
        get { return curItemType; }
        set { curItemType = value; }
    }

    [SerializeField] Item_Trap trap;
    [SerializeField] Item_Transparency transparency;

    PlayerController user;

    public void SetUser(PlayerController user)
    {
        this.user = user;
    }

    public void GetItem(ItemType type)
    {
        if (CurItemType == ItemType.None)
            return;

        CurItemType = type;

        // Item 이미지 반영??
    }

    public void Use(PlayerController user)
    {
        if (CurItemType == ItemType.None)
            return;

        switch(curItemType)
        {
            case ItemType.Trop:
                CaseTrop();
                break;
            case ItemType.Shield:
                CaseShield();
                break;
            case ItemType.Transparent:
                CaseTransparent();
                break;
        }

        curItemType = ItemType.None;
    }

    private void CaseTrop()
    {
        // 트랩을 생성하고
        // 트랩의 주인을 설정하면 트랩은 알아서 동작할 수 있도록 스크립트를 활용???
    }

    private void CaseShield()
    {

    }

    private void CaseTransparent()
    {
        transparency.SetUser(user);

    }

}

public enum ItemType
{
    None,
    Trop,
    Shield,
    Transparent 
}
