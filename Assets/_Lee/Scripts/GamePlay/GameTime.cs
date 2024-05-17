using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTime : MonoBehaviourPun
{
    [SerializeField] LobbyManager lobbyManager;
    int maxTime = 90;
    [SerializeField] PhotonView PV;
    [SerializeField] TMP_Text timeText;
    [SerializeField] Image endingCredit;
    public TMP_Text TimeText { get { return timeText; } }
    [SerializeField] Button lobbyButton;

    private void Awake()
    {
        lobbyManager = FindObjectOfType<LobbyManager>();
        lobbyButton.onClick.AddListener(LobbyButton);
    }
    // 여기 밑에 있는거 네트워크로 동기화 시켜야됨
    private void Update()
    {
        if(BaseGameScene.Instance.CheckGameState.CurState == GameState.GameProgress) // 게임 실행중일때 부터 타이머 움직이게 함
        RequestTimer();
    }
    public void RequestTimer()
    {
        timeText.text = $"{( int )( Time.time )}";
        if ( Time.time > maxTime )
        {
            endingCredit.gameObject.SetActive(true);
            BaseGameScene.Instance.CheckGameState.CurState = GameState.GameEnd; // 게임 끝난 상태로 바꿔줌
            // 여기서 로비로 보내주는거 해주면될듯
        }
    }
    private void LobbyButton()
    {
        PhotonNetwork.LoadLevel("NetworkRoom");
    }
}
