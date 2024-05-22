using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Transparency : MonoBehaviourPun
{
    // 투명화 아이템
    [SerializeField] materials normalMaterial;
    [SerializeField] materials transparentMaterial;  // 완전 투명
    [SerializeField] materials translucentMaterial;  // 반투명

    [SerializeField] PlayerItemController user;
    [SerializeField] SkinnedMeshRenderer render;

    [SerializeField] float setTime;
    List<Material> materiallist = new List<Material>();

    [SerializeField] LayerMask transparentLayer;
    [SerializeField] LayerMask translucentLayer;
    [SerializeField] LayerMask normalLayer;

    public void SetUser(PlayerItemController user)
    {
        this.user = user;
        render = user.gameObject.GetComponent<SkinnedMeshRenderer>();
    }

    public void Use()
    {
        user.photonView.RPC("SetTransparent", RpcTarget.Others);
        SetTranslucent();
    }

    [PunRPC]
    private void SetTransparent()
    {   // 투명 설정
        materiallist.Clear();
        materiallist.Add(transparentMaterial.skinMaterial); materiallist.Add(transparentMaterial.baseMaterial);
        //user.gameObject.layer = 30;

        render.SetMaterials(materiallist);
        StartCoroutine(SettingTime());
    }
    /*
    private void SetTransparent()
    {   // 불투명 설정
        materiallist.Clear();
        materiallist.Add(transparentMaterial.skinMaterial); materiallist.Add(transparentMaterial.baseMaterial);
        //user.gameObject.layer = 30;

        render.SetMaterials(materiallist);
        StartCoroutine(SettingTime());
    }*/

    private void SetTranslucent()
    {   // 반투명 설정
        materiallist.Clear();
        //user.gameObject.layer = 31;
        materiallist.Add(translucentMaterial.skinMaterial); materiallist.Add(translucentMaterial.baseMaterial);

        render.SetMaterials(materiallist);
        StartCoroutine(SettingTime());
    }

    IEnumerator SettingTime()
    {
        yield return new WaitForSeconds(setTime);
        materiallist.Clear();
        //user.gameObject.layer = 3;
        materiallist.Add(normalMaterial.skinMaterial); materiallist.Add(normalMaterial.baseMaterial);

        render.SetMaterials(materiallist);
    }

    [Serializable]
    public struct materials
    {
        public Material baseMaterial;
        public Material skinMaterial;
    }

}
