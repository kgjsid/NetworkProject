using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;


public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject mainCharacter;
    public enum Panel { Login, Main, Lobby, Room }// 패널 상태

    [SerializeField] Panel curPanel;

    [SerializeField] LoginPanel loginPanel;
    [SerializeField] MainPanel mainPanel;
    [SerializeField] LobbyPanel lobbyPanel;
    [SerializeField] RoomPanel roomPanel;

    private ClientState state; // 클라이언트의 상태
    private void Awake()
    {
        SetActivePanel(Panel.Login); // 시작시 무조건 Login 화면이 나올수 있게
    }
    private void Update()
    {
        ClientState curState = PhotonNetwork.NetworkClientState;// 네트워크가 받은 클라이언트의 현재상태
        if ( state == curState ) // 같을 때 리턴
            return;

        state = curState; // 같지 않으면 같게 만듬
    }
    // 패널(상태)이 변할때만다 다른거 다꺼줌
    private void SetActivePanel( Panel panel )
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
    }
    int connectedCount = 0;
    public override void OnConnected()
    { // 서버에 접속할 때 한번
        Debug.Log("2");
        if ( connectedCount == 0 )
        {
            SetActivePanel(Panel.Main);
        }
    }
    
    public override void OnConnectedToMaster()
    {   // 마스터 서버에 접속할 때
        // 방을 떠나면 방 서버에서 나가고 매칭을 위한 마스터 서버로 접속할때마다 호출
        
        Debug.Log("1");
        if(connectedCount > 0 )
        SetActivePanel(Panel.Lobby);
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
        SetActivePanel(Panel.Lobby); // 로비를 찾을 때 로비화면 보여줌
    }
    public override void OnLeftLobby()
    {
        SetActivePanel(Panel.Main); // 로비에서 나갈때 메인화면으로
    }
    public override void OnCreateRoomFailed( short returnCode, string message ) // 방만들기 실패시
    {
        Debug.Log($"방 만들기 실패 : {returnCode}, {message}");
    }
    public override void OnCreatedRoom() // 방만들기 성공시
    {
        Debug.Log("방 만들기 성공");
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
    public override void OnJoinRoomFailed( short returnCode, string message )       // 방을 못찾았으면 호출
    {
        Debug.Log($"방 들어가기 실패 : {returnCode}, {message}");
    }
    public override void OnLeftRoom()
    {
        Debug.Log("로비로감");
        SetActivePanel(Panel.Lobby);
        Debug.Log("방 떠나는 콜백");
    }

    public override void OnPlayerEnteredRoom( Player newPlayer )        // 새로운 플레이어가 방에 들어올떄
    {
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
}