using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class MissionGameScene : BaseGameScene
{   // 미션 게임 씬
    private static MissionGameScene instance;
    public static MissionGameScene Instance { get { return instance; } }

    [SerializeField] float missionInterval;
    [SerializeField] float missionTime;

    Coroutine missionRoutine;

    [SerializeField] List<PlayerMission> playerMissions;

    public float MissionTime { get { return missionTime; } }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    protected override IEnumerator Start()
    {
        yield return base.Start();
        
        missionRoutine = StartCoroutine(MissionRoutine());
    }

    private IEnumerator MissionRoutine()
    {
        while (true)
        {
            if (playerMissions.Count == 0)
                break;

            yield return new WaitForSeconds(missionInterval);

            foreach (PlayerMission playerMission in playerMissions)
            {
                photonView.RPC("SetPlayerMission", RpcTarget.MasterClient);
            }

            yield return new WaitForSeconds(missionTime);
            Debug.Log("미션 시간 종료");

            for(int i = 0; i < playerMissions.Count; i++)
            {
                if (playerMissions[i].CurMissionType != MissionType.Clear)
                {   // 미션 실패시 10 데미지
                    playerMissions[i].CurMissionType = MissionType.Fail;

                    PlayerController target = playerMissions[i].gameObject.GetComponent<PlayerController>();
                    if (target == null) // 미리 맞아서 죽었을 수 있으니까
                        continue;
                    
                    playerMissions.Remove(playerMissions[i]);

                    if (target.Hp > 0)
                    {
                        target.TakeDamage(10);
                    }
                }
            }
        }
    }

    public void SetMissionScript(PlayerMission playerMission)
    {
        playerMissions.Add(playerMission);
    }

    [PunRPC]
    private void SetPlayerMission()
    {
        for (int i = 0; i < playerMissions.Count; i++)
        {
            MissionType randomMission = (MissionType)Random.Range(0, (int)MissionType.Size);
            playerMissions[i].SetMission(randomMission);
        }
    }
}

public enum MissionType
{
    killMission,
    itemMission,
    runMission,
    Size,
    Clear,
    Fail
}