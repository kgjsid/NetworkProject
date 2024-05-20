using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class RoomPanel_TestChat : MonoBehaviourPun
{
    [SerializeField] TMP_Text roomName;
    [SerializeField] RectTransform playerContent;
    [SerializeField] PlayerEntry platyerEntry;

    [SerializeField] Button lobbyButton;
    [SerializeField] Button startButton;

    // 현재들어온 플레이어들 관리
    private List<PlayerEntry> PlayerList;

    [SerializeField] TMP_Text chatTextPrefab;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] RectTransform content;


    private void Awake()
    {
        inputField.onSubmit.AddListener(Send);
    }

    private void Send(string a)
    {
        photonView.RPC("SendRpc", RpcTarget.All, inputField.text);
        inputField.text = "";
    }

    [PunRPC]
    private void SendRpc(string inputField)
    {
        TMP_Text textPrefab = Instantiate(chatTextPrefab, content);
        textPrefab.text = $"{PhotonNetwork.NickName} : {inputField}";
    }



    private void Start()
    {
        Player player = PhotonNetwork.LocalPlayer;
        if ( player.IsMasterClient )
        {
            startButton.interactable = false;
        }
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
    }
    private void GameStart()
    {
        Player player = PhotonNetwork.LocalPlayer;
        if ( !player.IsMasterClient )
        {
            bool ready = player.GetReady();
            player.SetReady(!ready);
        }
        else if ( player.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount > 1 )
        {
            // 마스터일때는 준비와 게임 시작을하게 해줄거임
            PhotonNetwork.CurrentRoom.IsVisible = false; // 방 닫기
            PhotonNetwork.LoadLevel("BaseGameScene_bcw");
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

    public void MasterClientSwitched( Player newMaster )
    {
        foreach ( PlayerEntry entry in PlayerList )
        {
            if ( entry.Player.ActorNumber == newMaster.ActorNumber )
            {
                entry.SetPlayer(newMaster);
            }
        }
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
        if ( readyCount == PhotonNetwork.PlayerList.Length - 1 && PhotonNetwork.PlayerList.Length != 1 )
        {
            startButton.interactable = true;
        }
    }
}
