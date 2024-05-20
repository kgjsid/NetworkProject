using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class TempAIKill : MonoBehaviour
{
    [SerializeField] PlayerMission playerMission;

    public void IncreasePlayerKillCount()
    {
        

        playerMission.IncreaseKillCount();
    }

    public void GetOther(int actorNumber)
    {   // 액터 넘버를 받아서 확인해보면 찾을 수 있음
        
        /*foreach(Player player in PhotonNetwork.PlayerList)
        {
            if(player.ActorNumber == actorNumber)
            {
                
            }
        }*/

    }
}
