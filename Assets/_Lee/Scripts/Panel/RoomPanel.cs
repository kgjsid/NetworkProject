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
            playerEntry.SetPlayer(player);
            PlayerList.Add(playerEntry); // 플레이어 저장
        }

        // 여기서 이제 레디상황을 알아야된다. 근데 레디상황은 플레이어 목록이 들고있다.
        PhotonNetwork.LocalPlayer.SetReady(false);
        PhotonNetwork.LocalPlayer.SetLoad(false);
        AllPlayerReadycheck();
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
        PhotonNetwork.JoinLobby();
    }
    private void GameStart()
    {
        platyerEntry.ReadyAndStart();
    }

    public void PlayerEnterRoom( Player newPlayer )
    {
        PlayerEntry playerEntry = Instantiate(platyerEntry, playerContent); // 생성 
        playerEntry.SetPlayer(newPlayer);
        PlayerList.Add(playerEntry); // 플레이어 저장
        AllPlayerReadycheck();
    }
    public void PlayerLeftRoom(Player otherPlayer )
    {
        PlayerEntry playerEntry = null;

        foreach(PlayerEntry entry in PlayerList )
        {
            if( entry.Player.ActorNumber == otherPlayer.ActorNumber )
            {
                playerEntry = entry;
            }
        }
        PlayerList.Remove(playerEntry);
        Destroy(playerEntry.gameObject);
        AllPlayerReadycheck();

    }

    private void AllPlayerReadycheck()
    {
        // 마스터만 확인
        if ( !PhotonNetwork.IsMasterClient )
            return;
        // 카운터 재기
        int readyCount = 0; // 처음에 초기화
        foreach ( Player player in PhotonNetwork.PlayerList )
        {
            if(player.GetReady())
                readyCount++;
        }
        // 같아지면 시작할수 있게
       if( readyCount == PhotonNetwork.PlayerList.Length -1)
        {
            startButton.interactable = true;
        }
    }
}
