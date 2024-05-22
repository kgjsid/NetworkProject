using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemBox_Create : MonoBehaviour
{
    private ItemBox itemBoxScript;
    [SerializeField] private int respawnTime;
    [SerializeField] GameObject box;
    public void Start()
    {
        itemBoxScript = GetComponent<ItemBox>();
    }    

    public void CreateItemBox()
    {
        if (itemBoxScript.boxStatus == false)
        {
            StartCoroutine(ReCreate());
        }
    }

    public IEnumerator ReCreate()
    {
        yield return new WaitForSeconds(respawnTime);
        if (box != null)
        {
            itemBoxScript.boxStatus = true;
            box.gameObject.SetActive(true);
        }
    }    
}
