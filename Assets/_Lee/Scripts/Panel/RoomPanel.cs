using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class RoomPanel : MonoBehaviour
{
    [SerializeField] TMP_Text roomName;
    [SerializeField] RectTransform playerContent;
    [SerializeField] PlayerEntry platyerEntry;

    [SerializeField] Button lobbyButton;
    [SerializeField] Button startButton;

    // 현재들어온 플레이어들 관리
    private List<PlayerEntry> PlayerList;

    private void Start()
    {
        lobbyButton.onClick.AddListener(() => Lobby());
        startButton.onClick.AddListener(() => GameStart());
    }
    private void OnEnable()
    {
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        PlayerList = new List<PlayerEntry>();
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
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // 로비로 보내기
    private void Lobby()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.JoinLobby();
    }
    private void GameStart()
    {
        Player player = PhotonNetwork.LocalPlayer;

        foreach(PlayerEntry playerEntry in PlayerList )
        {
            if(player.ActorNumber == playerEntry.Player.ActorNumber )
            {
                playerEntry.ReadyTxT.text = "준비완료";
            }
        }
        bool ready = player.GetReady();
        player.SetReady(!ready);
        
        if ( player.IsMasterClient )
        {
            // 마스터일때는 준비와 게임 시작을하게 해줄거임
            PhotonNetwork.CurrentRoom.IsVisible = false; // 방 닫기
            PhotonNetwork.LoadLevel("BaseGameScene");
        }
    }
    public void PlayerEnterRoom( Player newPlayer )
    {
        PlayerEntry playerEntry = Instantiate(platyerEntry, playerContent); // 생성 
        playerEntry.SetPlayer(newPlayer);
        PlayerList.Add(playerEntry); // 플레이어 저장
        AllPlayerReadycheck();
    }
    public void PlayerLeftRoom( Player otherPlayer )
    {
        PlayerEntry playerEntry = null;

        foreach ( PlayerEntry entry in PlayerList )
        {
            if ( entry.Player.ActorNumber == otherPlayer.ActorNumber )
            {
                playerEntry = entry;
            }
        }
        PlayerList.Remove(playerEntry);
        Destroy(playerEntry.gameObject);
        AllPlayerReadycheck();

    }
    public void PlayerPropertiesUpdate( Player targetPlayer, PhotonHashtable changedProps )
    {
        PlayerEntry playerEntry = null;
        foreach ( PlayerEntry entry in PlayerList )
        {
            if ( entry.Player.ActorNumber == targetPlayer.ActorNumber )
            {
                playerEntry = entry;
            }
        }
        playerEntry.ChangeCustomProperty(changedProps);
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
            if ( player.GetReady() )
                readyCount++;
        }
        // 같아지면 시작할수 있게
        if ( readyCount == PhotonNetwork.PlayerList.Length - 1 )
        {
            startButton.interactable = true;
        }
    }
}
