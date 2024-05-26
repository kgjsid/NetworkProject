using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class RoomPanel : MonoBehaviour
{
    [SerializeField] TMP_Text roomName;
    [SerializeField] RectTransform playerContent;
    [SerializeField] PlayerEntry platyerEntry;
    [SerializeField] TMP_Text gameMod;
    [SerializeField] List<Image> gameModIMG;


    [SerializeField] Button lobbyButton;
    [SerializeField] Button startButton;
    [SerializeField] Button GameModButton;

    [SerializeField] GameModPanel gameModPanel;
    // 현재들어온 플레이어들 관리
    private List<PlayerEntry> PlayerList;

    List<string> gameModName;
    public List<string> GameModName { get { return gameModName; } }

    [SerializeField] string baseGameScene;
    [SerializeField] string missionGameScne;
    [SerializeField] string itemGameScene;
    public string curentGameModname;

    private void Awake()
    {
        gameModName = new List<string>();
        PlayerList = new List<PlayerEntry>();
    }
    private void Start()
    {
        lobbyButton.onClick.AddListener(Lobby);
        startButton.onClick.AddListener(GameStart);
        GameModButton.onClick.AddListener(GameMod);
        GameModName.Add(baseGameScene);
        GameModName.Add(missionGameScne);
        GameModName.Add(itemGameScene);
    }
    private void OnEnable()
    {
        // 여기서 초기화시켜줘서 레디 false로 바꿔줍시다

        gameMod.text = "기본 모드";
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);
        if ( PhotonNetwork.LocalPlayer.IsMasterClient )
        {
            startButton.interactable = false;
            GameModButton.interactable = true;
        }
        if ( !PhotonNetwork.LocalPlayer.IsMasterClient )
        {
            GameModButton.interactable = false;
            startButton.interactable = true;

        }
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        foreach ( Player player in PhotonNetwork.PlayerList ) // 네트워크에서 신호 보내준 플레이어 가져온다.
        {
            PlayerEntry playerEntry = Instantiate(platyerEntry, playerContent); // 생성 
            playerEntry.SetPlayer(player);
            PlayerList.Add(playerEntry); // 플레이어 저장
        }
        PhotonNetwork.CurrentRoom.IsVisible = true; // 방 열기
        // 여기서 이제 레디상황을 알아야된다. 근데 레디상황은 플레이어 목록이 들고있다.
        PhotonNetwork.LocalPlayer.SetReady(false);
        PhotonNetwork.LocalPlayer.SetLoad(false);
        AllPlayerReadycheck();
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public void CurrentGameMod(int index, string modName)
    {
        gameMod.text = modName;
        foreach( Image image in gameModIMG )
        {
            image.gameObject.SetActive(false);
        }
        gameModIMG [index].gameObject.SetActive(true);
    }

    private void GameMod()
    {
        gameModPanel.gameObject.SetActive(true);
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
            PhotonNetwork.LoadLevel(curentGameModname); // 게임 모드 바뀔때마다 바꿔야됨
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
