using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemBox_ : MonoBehaviour
{
    // 아이템 박스. 충돌하였을 때 상대방에게 아이템을 주고, 자신은 숨길 수 있도록
    [SerializeField] ItemBoxController controller;
    [SerializeField] int indexNumber;

    public void SetController(ItemBoxController controller, int index)
    {
        this.controller = controller;
        indexNumber = index;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerItemController target = other.GetComponent<PlayerItemController>();

        if (target != null)
        {
            if (target.Controller.IsDead)
                return;


            ItemType randomItem = (ItemType)Random.Range(1, (int)ItemType.Size);
            target.PlayerItem.GetItem(randomItem);
        }
        Debug.Log($"충돌 : {indexNumber}");
        controller.StartHideRoutine(indexNumber);
    }
}
