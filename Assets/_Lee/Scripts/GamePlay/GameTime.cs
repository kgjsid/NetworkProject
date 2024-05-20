using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTime : MonoBehaviourPun
{
    int time = 10;
    public int Time { get { return time; } }
    [SerializeField] PhotonView PV;
    [SerializeField] TMP_Text timeText;
    [SerializeField] Image endingCredit;
    [SerializeField] TMP_Text resultText;
    public TMP_Text TimeText { get { return timeText; } }
    private void Awake()
    {
    }
    // 여기 밑에 있는거 네트워크로 동기화 시켜야됨
    public void StartTimer()
    {
        if ( timerRoutine == null )
            timerRoutine = StartCoroutine(Timer());
        Debug.Log("시간 들어옴?");
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
            Debug.Log("시간 코루틴에 들어옴?");
            timeText.text = time.ToString();
            time--;
            yield return new WaitForSeconds(1);
        }
    }
    IEnumerator EndingLobby()
    {
        yield return new WaitForSeconds(3f);
        RoomButton();
    }
    public void EndingImage()
    {
        // 여기 있는 조건 게임이 끝났을때 조건
        endingCredit.gameObject.SetActive(true);
        StartCoroutine(EndingLobby());
    }

    private void RoomButton()
    {
        PhotonNetwork.LoadLevel("NetworkRoom");
    }
}