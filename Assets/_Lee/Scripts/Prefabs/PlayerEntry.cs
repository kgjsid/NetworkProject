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
    [SerializeField] TMP_Text readyIMG;
    [SerializeField] Image masterIcon;

    private Player player;
    public Player Player { get { return player; } }

    private void Awake()
    {

    }

    public void SetPlayer( Player player )
    {
        this.player = player;

        nickName.text = player.NickName;
        if ( player.IsMasterClient )
        {
            readyIMG.gameObject.SetActive(false);
            masterIcon.gameObject.SetActive(true);
        }
        else
        {
            readyIMG.gameObject.SetActive(true);
            masterIcon.gameObject.SetActive(false);
        }
    }
    public void ReadyAndStart()
    {
        bool ready = player.GetReady();
        player.SetReady(ready);
        if(!player.IsMasterClient)
        {
            readyIMG.text = "준비완료";
        }

        if(player.IsMasterClient )
        {
            // 마스터일때는 준비와 게임 시작을하게 해줄거임
            PhotonNetwork.CurrentRoom.IsVisible = false; // 방 닫기
            PhotonNetwork.LoadLevel("BaseGameScene");
        }
    }
}
