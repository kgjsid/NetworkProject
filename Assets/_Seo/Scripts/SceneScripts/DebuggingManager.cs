using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class DebuggingManager : MonoBehaviourPunCallbacks
{
    // 네트워크 디버깅용
    [SerializeField] string debugRoomName = "DebugRoom 1";
    [SerializeField] string sceneName;

    private void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = $"TestPlayer({Random.Range(0, 1000)})";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 10;
        TypedLobby typedLobby = new TypedLobby("DebugLobby", LobbyType.Default);

        PhotonNetwork.JoinOrCreateRoom(debugRoomName, options, typedLobby);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(GameStartDelay());
    }

    IEnumerator GameStartDelay()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(1f);
        // 원격에게 함수 호출
        photonView.RPC("StartGame", RpcTarget.All);
    }

    [PunRPC]
    public void StartGame()
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
}
