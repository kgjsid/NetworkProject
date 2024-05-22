using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionPanel : MonoBehaviour
{
    [SerializeField] Image missionPanel;
    [SerializeField] TMP_Text missionDetail;
    [SerializeField] TMP_Text clearOrNot;
    [SerializeField] TMP_Text timeText;

    [SerializeField] Color clearColor;
    [SerializeField] Color failColor;

    [SerializeField] MissionDetailStruct [] missionDetailStruct;

    Coroutine missionTimer;

    private void Start()
    {
        missionDetail.text = "미션이 없습니다";
        clearOrNot.text = "";
        timeText.text = "";
    }

    public void ShowMissionDetail( MissionType missionType )
    {
        if ( missionType == MissionType.Clear )
        {
            ClearMission();
        }
        else if ( missionType == MissionType.Fail )
        {
            FailMission();
        }
        else
        {
            SetfailColor();
            missionPanel.gameObject.SetActive(true);
            missionDetail.text = missionDetailStruct [( int )missionType].missionDetail;
            clearOrNot.text = "미완료";
            missionTimer = StartCoroutine(MissionTimer());
        }
    }


    IEnumerator MissionTimer()
    {
        int time = (int)MissionGameScene.Instance.MissionTime;

        while ( true )
        {
            timeText.text = $" (남은 시간 : {time})";
            time--;
            yield return new WaitForSeconds(1);
        }
    }


    private void ClearMission()
    {
        SetClearColor();
        clearOrNot.text = "완료";
        StopAllCoroutines();
        timeText.text = $" (남은 시간 : 0)";
    }

    [Serializable]
    struct MissionDetailStruct
    {
        public string missionDetail;
    }

    private void FailMission()
    {
        SetfailColor();
        missionDetail.text = $"{missionDetail.text} (실패)";
        StopAllCoroutines();
        timeText.text = " (남은 시간 : 0)";
    }

    private void SetClearColor()
    {
        missionDetail.color = clearColor;
        clearOrNot.color = clearColor;
        timeText.color = clearColor;
    }
    private void SetfailColor()
    {
        missionDetail.color = failColor;
        clearOrNot.color = failColor;
        timeText.color = failColor;
    }
}
