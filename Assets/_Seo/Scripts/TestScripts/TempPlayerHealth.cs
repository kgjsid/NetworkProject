using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TempPlayerHealth : MonoBehaviourPun
{
    [SerializeField] int hp;

    private void Awake()
    {
        hp = 1;
    }

    [ContextMenu("Hit")]
    public void TakeHit()
    {
        Die();
    }

    private void Die()
    {
        PhotonNetwork.LocalPlayer.SetState(PlayerState.Die);
    }
}
