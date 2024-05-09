using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : MonoBehaviour
{
    [SerializeField] TMP_Text roomName;
    [SerializeField] RectTransform playerContent;
    [SerializeField] PlayerEntry platyerEntry;

    [SerializeField] Button lobbyButton;
    [SerializeField] Button startButton;

    // 현재들어온 플레이어들 관리
    private List<PlayerEntry> PlayerList;
    private void Awake()
    {
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        PlayerList = new List<PlayerEntry>();
    }
    private void OnEnable()
    {        
        foreach ( Player player in PhotonNetwork.PlayerList ) // 네트워크에서 신호 보내준 플레이어 가져온다.
        {
            PlayerEntry playerEntry = Instantiate(platyerEntry, playerContent); // 생성 
            PlayerList.Add(playerEntry); // 플레이어 저장
        }
        
    }
    private void Start()
    {
        lobbyButton.onClick.AddListener(() => Lobby());
        startButton.onClick.AddListener(() => GameStart());
    }

    // 로비로 보내기
    private void Lobby()
    {
        PhotonNetwork.LeaveRoom();
    }
    private void GameStart()
    {

    }

}
