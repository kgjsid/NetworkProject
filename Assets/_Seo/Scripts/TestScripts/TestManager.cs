using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestManager : MonoBehaviourPunCallbacks
{
    [SerializeField] List<PlayerMission> playerMissions; 

    private IEnumerator Start()
    {
        PhotonNetwork.Instantiate("MissionPlayer", transform.position, Quaternion.identity);

        yield return new WaitForSeconds(2f);

        playerMissions = FindObjectsOfType<PlayerMission>().ToList();
        Debugging();
    }

    private void Debugging()
    {
        foreach(PlayerMission playerMission in playerMissions)
        {
            Debug.Log($"{playerMission.name}");
        }
    }
}
