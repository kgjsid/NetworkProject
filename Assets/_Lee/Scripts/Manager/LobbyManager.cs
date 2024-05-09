using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject mainCharacter;
    public enum Panel { Login, Main, Lobby, Room }// 패널 상태

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
        if ( loginPanel != null ) loginPanel.gameObject.SetActive(panel == Panel.Login);
        if ( mainPanel != null )
        {
            mainPanel.gameObject.SetActive(panel == Panel.Main);
            mainCharacter.gameObject.SetActive(panel == Panel.Main);
        }
        if ( lobbyPanel != null ) lobbyPanel.gameObject.SetActive(panel == Panel.Lobby);
        if(roomPanel !=null ) roomPanel.gameObject.SetActive(panel == Panel.Room);
    }
    public override void OnConnected()
    {
        SetActivePanel(Panel.Main); // 로그인되면 메인화면으로
    }
    public override void OnDisconnected( DisconnectCause cause )
    {
        if ( cause == DisconnectCause.ApplicationQuit ) return;

        SetActivePanel(Panel.Login); // 로그아웃? 되면 로그인 화면으로
    }
    public override void OnJoinedLobby()
    {
        SetActivePanel(Panel.Lobby); // 로비를 찾을 때 로비화면 보여줌
    }
    public override void OnLeftLobby()
    {
        SetActivePanel (Panel.Main); // 로비에서 나갈때 메인화면으로
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
    public override void OnRoomListUpdate( List<RoomInfo> roomList )
    {
        lobbyPanel.UpdateRoomList(roomList);
    }
}