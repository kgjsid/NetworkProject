using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class MissionGameScene : BaseGameScene
{   // 미션 게임 씬
    private static MissionGameScene instance;
    public static MissionGameScene Instance { get { return instance; } }

    [SerializeField] float missionInterval;
    [SerializeField] float missionTime;

    Coroutine missionRoutine;

    protected override IEnumerator Start()
    {
        yield return base.Start();
        missionRoutine = StartCoroutine(MissionRoutine());
    }

    private IEnumerator MissionRoutine()
    {
        yield return new WaitForSeconds(missionInterval);

        foreach (Player player in players)
        {
            if (player.GetState() == PlayerState.Die)
                continue;

            MissionType randomMission = (MissionType)UnityEngine.Random.Range(0, (int)MissionType.Size);
            player.SetMission(randomMission);
        }

        yield return new WaitForSeconds(missionTime);
        Debug.Log("미션 시간 종료");
        foreach(Player player in players)
        {
            if (player.GetState() == PlayerState.Die)
                continue;

            if(player.GetMission() != MissionType.Clear)
            {

            }
        }
    }


}
