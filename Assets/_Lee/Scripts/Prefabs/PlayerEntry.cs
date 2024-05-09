using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntry : MonoBehaviour
{
    [SerializeField] TMP_Text nickName;
    [SerializeField] Image Ready;
    [SerializeField] Image masterIcon;
    private void Start()
    {
        nickName.text = PhotonNetwork.LocalPlayer.NickName;
        if( PhotonNetwork.IsMasterClient )
        {
            Ready.gameObject.SetActive( false );
            masterIcon.gameObject.SetActive( true );
        }
    }
}
