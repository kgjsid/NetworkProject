using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMission : MonoBehaviourPun
{
    // 미션용 스크립트
    // 이모트 미션도 만들어 보기
    [SerializeField] MissionType curMissionType;

    public UnityEvent<MissionType> changeMission;

    [SerializeField] int killCount;
    [SerializeField] bool isRun;

    Coroutine missionRoutine;

    private MissionType CurMissionType
    {
        get { return curMissionType; }
        set
        {
            curMissionType = value;
            changeMission?.Invoke(curMissionType);

            switch(curMissionType)
            {
                case MissionType.killMission:
                    missionRoutine = StartCoroutine(KillMission());
                    break;
                case MissionType.runMission:
                    missionRoutine = StartCoroutine(RunMission());
                    break;
                case MissionType.itemMission:
                    missionRoutine = StartCoroutine(ItemMission());
                    break;
            }
        }
    }

    private void Start()
    {
        CurMissionType = MissionType.killMission;
    }

    private void ClearMission()
    {
        Debug.Log("미션 끝");
        CurMissionType = MissionType.Clear;
    }

    private IEnumerator RunMission()
    {
        Debug.Log("달리기 미션");
        while (true)
        {
            yield return new WaitUntil(() => isRun);
            float startTime = Time.time;
            yield return new WaitUntil(() => !isRun || Time.time >= (startTime + 3f));
            float endTime = Time.time;

            float runningTime = endTime - startTime;

            if(runningTime >= 3f)
            {
                break;
            }
        }

        ClearMission();
    }

    private IEnumerator KillMission()
    {   // 킬처리 추가
        Debug.Log("킬미션");
        int killCount = 0;
        while (killCount < 3)
        {
            yield return new WaitForSeconds(0.1f);
        }

        ClearMission();
    }

    private IEnumerator ItemMission()
    {   // 아이템전은 나중에 아이템 만들고 나면 스크립트 이용해 볼 것
        Debug.Log("아이템 미션");
        yield return null;

        ClearMission();
    }

    private void OnRun(InputValue value)
    {
        isRun = value.isPressed;
    }

    [PunRPC]
    public void SetMission(MissionType mission)
    {
        if (photonView.IsMine)
            CurMissionType = mission;
    }


}
