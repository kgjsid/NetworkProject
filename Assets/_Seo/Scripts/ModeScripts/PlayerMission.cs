using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMission : MonoBehaviourPun
{
    // 미션용 스크립트
    // 이모트 미션도 만들어 보기
    [SerializeField] MissionType curMissionType;

    public Action<MissionType> changeMission;

    [SerializeField] int killCount;
    [SerializeField] bool isRun;

    Coroutine missionRoutine;
    MissionPanel panel;
    [SerializeField] Damage damageChack;

    [SerializeField] private LayerMask ItemCheck;
    [SerializeField] GameObject itemBoxPrefab;
    public MissionType CurMissionType
    {
        get { return curMissionType; }
        set
        {
            curMissionType = value;
            changeMission?.Invoke(curMissionType);

            switch ( curMissionType )
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
        MissionGameScene.Instance.SetMissionScript(this);

        if (photonView.IsMine)
        {
            panel = FindObjectOfType<MissionPanel>();
            changeMission += panel.ShowMissionDetail;
        }
    }

    private void ClearMission()
    {
        CurMissionType = MissionType.Clear;
    }
    //완
    private IEnumerator RunMission()
    {
        while ( true )
        {
            yield return new WaitUntil(() => isRun);
            float startTime = Time.time;
            yield return new WaitUntil(() => !isRun || Time.time >= ( startTime + 3f ));
            float endTime = Time.time;

            float runningTime = endTime - startTime;

            if ( runningTime >= 3f )
            {
                break;
            }
        }

        ClearMission();
    }

    private IEnumerator KillMission()
    {   // 킬처리 추가
        // 킬이 3킬 ( AI만)
        // 일단 만들긴했는데 플레이어가 어택 했을때 맞은 콜라이더가 AI면 되게해서 중복으로 잡으면 카운트하나를 놓친다.
        int killCount = 0;
        while ( killCount < 3 )
        {
            PhotonView targetView = PhotonView.Find(damageChack.TargetID);
            if ( targetView != null && targetView.gameObject.layer == 6 )
            {
                killCount++;
                damageChack.TargetIDReset();
            }
            yield return new WaitForSeconds(0.1f);
        }
        ClearMission();
    }

    GameObject instanceItemBox;
    private IEnumerator ItemMission()
    {   // 아이템전은 나중에 아이템 만들고 나면 스크립트 이용해 볼 것
        // 아이템 먹는거 구현 완료
        // 특정 아이템을 특정 플레이어만 먹을 수 있게(다른 사람이 보여야함)
        List<GameObject> itemSapwns = MissionGameScene.Instance.ItemSpawnsPoint;
        int randPoint = UnityEngine.Random.Range(0, itemSapwns.Count);
        instanceItemBox = Instantiate(itemBoxPrefab, Vector3.zero, Quaternion.identity);
        instanceItemBox.transform.position = new Vector3(itemSapwns [randPoint].transform.position.x, 1.4f, itemSapwns [randPoint].transform.position.z);
        // 여기까지 생성
        // 이제 먹으면 완료되게
        itemCount = 0;
        while ( true)
        {
            if ( itemCount == 1 )
            {
                ClearMission();
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }

    }
    int itemCount = 0;
    private void OnTriggerEnter( Collider other )
    {
        if ( ( ItemCheck.value & 1 << other.gameObject.layer ) != 0 )
        {
            itemCount++;
            Destroy(instanceItemBox);
        }

    }
    private void OnRun( InputValue value )
    {
        isRun = value.isPressed;
    }


    public void SetMission( MissionType mission )
    {
        if ( !photonView.IsMine )
            return;

        CurMissionType = mission;
    }

    public void IncreaseKillCount()
    {
        killCount++;
    }
}
