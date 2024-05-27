using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;


public class LobbyManager : MonoBehaviourPunCallbacks
{
    private static LobbyManager instance;
    public static LobbyManager Instance { get { return instance; } }
    [SerializeField] GameObject mainCharacter;
    public enum Panel { Login, Main, Lobby, Room, Info, SignUp }// 패널 상태

    [SerializeField] Panel curPanel;
  
    [SerializeField] LoginPanel loginPanel;
    [SerializeField] MainPanel mainPanel;
    [SerializeField] LobbyPanel lobbyPanel;
    [SerializeField] RoomPanel roomPanel;
    [SerializeField] InfoPanel infoPanel;
    [SerializeField] SignUpPanel signUpPanel;
    [SerializeField] SettingPanel settingPanel;
    [SerializeField] Button SettingButton;
    [SerializeField] AudioClip BGM;
    private ClientState state; // 클라이언트의 상태
    private void Awake()
    {

        if ( instance == null )
        {
            instance = this;
        }

    }
    public void Setting()
    {
        settingPanel.gameObject.SetActive(true);
    }
    private void Start()
    {
        SettingButton.onClick.AddListener(Setting);
        SetActivePanel(Panel.Login); // 시작시 무조건 Login 화면이 나올수 있게
        Manager.Sound.PlayBGM(BGM);
        if ( PhotonNetwork.CurrentRoom != null )
        {
            SetActivePanel(Panel.Room);

        }
        else
        {
            SetActivePanel(Panel.Login);
        }
    }
    private void Update()
    {
        ClientState curState = PhotonNetwork.NetworkClientState;// 네트워크가 받은 클라이언트의 현재상태
        if ( state == curState ) // 같을 때 리턴
            return;

        state = curState; // 같지 않으면 같게 만듬
    }
    // 패널(상태)이 변할때만다 다른거 다꺼줌
    public void SetActivePanel( Panel panel )
    {
        curPanel = panel;

        if ( loginPanel != null ) loginPanel.gameObject.SetActive(panel == Panel.Login);
        if ( mainPanel != null )
        {
            mainPanel.gameObject.SetActive(panel == Panel.Main);
            mainCharacter.gameObject.SetActive(panel == Panel.Main);
        }
        if ( lobbyPanel != null ) lobbyPanel.gameObject.SetActive(panel == Panel.Lobby);
        if ( roomPanel != null ) roomPanel.gameObject.SetActive(panel == Panel.Room);
        if ( infoPanel != null ) infoPanel.gameObject.SetActive(panel == Panel.Info);
        if ( signUpPanel != null ) signUpPanel.gameObject.SetActive(panel == Panel.SignUp);
    }
    int connectedCount = 0;
    public override void OnConnected()
    { // 서버에 접속할 때 한번
        if ( connectedCount == 0 )
        {
            SetActivePanel(Panel.Main);
        }
    }

    public override void OnConnectedToMaster()
    {   // 마스터 서버에 접속할 때
        // 방을 떠나면 게임 서버에서 나가고 매칭을 위한 마스터 서버로 접속할때마다 호출
        if ( connectedCount > 0 )
        {
            Debug.Log("들어옴");
            SetActivePanel(Panel.Lobby);
        }
        connectedCount++;
    }
    public override void OnDisconnected( DisconnectCause cause )
    {
        // 게임 자체를 나갔을경우 리턴
        if ( cause == DisconnectCause.ApplicationQuit ) return;

        SetActivePanel(Panel.Login); // 로그아웃? 되면 로그인 화면으로
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("로비패널로 감");
        SetActivePanel(Panel.Lobby); // 로비를 찾을 때 로비화면 보여줌
    }
    public override void OnLeftLobby()
    {
        Debug.Log("메인패널로 감");
        SetActivePanel(Panel.Main); // 로비에서 나갈때 메인화면으로
    }
    public override void OnCreatedRoom() // 방만들기 성공시
    {
        SetActivePanel(Panel.Room);
    }
    public override void OnRoomListUpdate( List<RoomInfo> roomList ) // 방이 생성될때마다 호출
    {
        lobbyPanel.UpdateRoomList(roomList);
    }
    public override void OnJoinedRoom()     // 방을 찾았으면 호출
    {
        SetActivePanel(Panel.Room);
    }
    public override void OnLeftRoom()
    {
        SetActivePanel(Panel.Lobby);
    }
    public override void OnPlayerEnteredRoom( Player newPlayer )        // 새로운 플레이어가 방에 들어올떄
    {
        // 마스터 서버, 게임 서버
        roomPanel.PlayerEnterRoom(newPlayer);
    }
    public override void OnPlayerLeftRoom( Player otherPlayer )         // 어떤 플레이어가 방을 나갈때
    {
        roomPanel.PlayerLeftRoom(otherPlayer);
    }
    public override void OnPlayerPropertiesUpdate( Player targetPlayer, PhotonHashtable changedProps )
    {
        roomPanel.PlayerPropertiesUpdate(targetPlayer, changedProps);
    }
    public override void OnMasterClientSwitched( Player newMasterClient )
    {
        roomPanel.MasterClientSwitched(newMasterClient);
    }
    public void ShowInfo( string message )
    {
        infoPanel.ShowInfo(message);
    }
}