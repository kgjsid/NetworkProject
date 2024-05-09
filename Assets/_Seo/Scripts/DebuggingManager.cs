using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class DebuggingManager : MonoBehaviourPunCallbacks
{
    [SerializeField] string debugRoomName = "DebugRoom 1";

    private void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = $"TestPlayer({Random.Range(0, 5)})";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 10;
        options.IsVisible = false;
        TypedLobby typedLobby = new TypedLobby("DebugLobby", LobbyType.Default);

        PhotonNetwork.JoinOrCreateRoom(debugRoomName, options, typedLobby);
    }

    public override void OnJoinedRoom()
    {
        StartCoroutine(GameStartDelay());
    }

    IEnumerator GameStartDelay()
    {
        yield return new WaitForSeconds(1f);
        GameStart();
    }

    private void GameStart()
    {
        PhotonNetwork.LoadLevel("BaseGameScene");
    }
}
