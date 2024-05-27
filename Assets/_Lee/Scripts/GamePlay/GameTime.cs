using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTime : MonoBehaviourPun
{
    int time = 60;
    public int Time { get { return time; } }
    [SerializeField] PhotonView PV;
    [SerializeField] TMP_Text timeText;
    [SerializeField] Image endingCredit;
    [SerializeField] TMP_Text resultText;
    [SerializeField] TMP_Text winnerNickname;

    //[Header("ItemMode")]
    [SerializeField] CinemachineVirtualCamera resultCamera;

    [Header("Sound")]
    [SerializeField] AudioClip bgm;
    [SerializeField] AudioClip victory;
    [SerializeField] AudioClip Applause;

    public TMP_Text TimeText { get { return timeText; } }
    private void Start()
    {
        Manager.Sound.PlayBGM(bgm);
    }
    // 여기 밑에 있는거 네트워크로 동기화 시켜야됨
    public void StartTimer()
    {
        if ( timerRoutine == null )
            timerRoutine = StartCoroutine(Timer());
    }
    public void Victory()
    {
        resultText.text = "게임 승리";
    }
    public void Lose()
    {
        resultText.text = "게임 패배";
    }
    Coroutine timerRoutine;
    IEnumerator Timer()
    {
        while ( time >= 0 )
        {
            timeText.text = time.ToString();
            time--;
            yield return new WaitForSeconds(1);
        }
    }
    IEnumerator EndingLobby()
    {
        resultCamera.Priority = 30;
        Manager.Sound.StopBGM();
        Manager.Sound.PlaySFX(victory);
        Manager.Sound.PlaySFX(Applause);
        yield return new WaitForSeconds(5f);
        RoomButton();
    }
    public void EndingImage()
    {
        // 여기 있는 조건 게임이 끝났을때 조건
        if (BaseGameScene.Instance != null)
        {
            foreach (Player player in BaseGameScene.Instance.Players)
            {
                if (player.GetState() == PlayerState.Live)
                {
                    winnerNickname.text = player.NickName;
                }
            }
        }
        resultCamera.Priority = 20;
        endingCredit.gameObject.SetActive(true);
        StartCoroutine(EndingLobby());
    }

    private void RoomButton()
    {
        if ( PhotonNetwork.IsMasterClient )
            PhotonNetwork.LoadLevel("NetworkRoom");
    }
}