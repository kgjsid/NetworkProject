using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathController : MonoBehaviourPun
{   // 사망시 처리를 담당할 컨트롤러
    [SerializeField] bool isDead;

    [SerializeField] Material skinTransparent;
    [SerializeField] Material baseTransparent;

    public bool IsDead
    {
        get { return isDead; }
        set
        {
            isDead = value;
            if(isDead)
            {
                SetDie();
            }
        }
    }

    private void SetDie()
    {
        photonView.RPC("SetDeath", RpcTarget.All);
        if (photonView.IsMine)
        {   // everyThing 설정
            Camera.main.cullingMask = -1;
        }
    }

    [PunRPC]
    private void SetDeath()
    {
        gameObject.GetComponent<PlayerAttack>().enabled = false;

        StartCoroutine(DieMaterial());
    }

    IEnumerator DieMaterial()
    {
        yield return new WaitForSeconds(2.5f);

        SkinnedMeshRenderer render = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        render.SetMaterials(new List<Material> { baseTransparent, skinTransparent });
        gameObject.layer = 11;
        foreach (Transform child in transform)
        {   // Death로 설정
            child.gameObject.layer = 11;
        }
    }
}
