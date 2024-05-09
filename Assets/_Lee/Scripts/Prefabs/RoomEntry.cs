using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomEntry : MonoBehaviour
{
    [SerializeField] TMP_Text roomName;
    [SerializeField] TMP_Text players;
    [SerializeField] TMP_Text gameMod;
    [SerializeField] Button joinRoomButton;

    private RoomInfo roomInfo;
    private void Start()
    {
        joinRoomButton.onClick.AddListener(() => JoinRoom());
    }
    public void SetRoomInfo( RoomInfo roomInfo )
    {
        this.roomInfo = roomInfo;
        roomName.text = roomInfo.Name;
        players.text = $"{roomInfo.PlayerCount}/ {roomInfo.MaxPlayers}";
        joinRoomButton.interactable = roomInfo.PlayerCount < roomInfo.MaxPlayers;
    }

    public void JoinRoom()
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.JoinRoom(roomInfo.Name);
    }
}
