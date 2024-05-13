using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    // 언제간 여기서 상점으로 가게하는걸 만들겠지.....
    [SerializeField] Button startButton;
    [SerializeField] TMP_Text nickname;

    private void Start()
    {
        startButton.onClick.AddListener(() => Lobby());
        nickname.text = PhotonNetwork.LocalPlayer.NickName;
    }

    // Lobby를 찾아서줌
    private void Lobby()
    {
        PhotonNetwork.JoinLobby();
    }
}
