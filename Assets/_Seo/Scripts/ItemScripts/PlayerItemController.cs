using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerItemController : MonoBehaviour
{   // 플레이어의 아이템 관련 컨트롤러
    [SerializeField] PlayerController controller;
    [SerializeField] ItemObject itemObject;

    public PlayerController Controller { get { return controller; } }

    public ItemObject PlayerItem { get { return itemObject; } }

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        itemObject = GetComponentInChildren<ItemObject>();
        itemObject.SetUser(this);
    }

    private void OnUsingItem(InputValue value)
    {
        if (itemObject.CurItemType == ItemType.None)
            return;

        itemObject.Use(this);
    }
}
