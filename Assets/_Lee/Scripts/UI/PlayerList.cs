using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class PlayerList : MonoBehaviourPun
{
    [SerializeField] GameObject currentPlayer;
    [SerializeField] GameObject PlayerProfile;
    List<PlayerProfileEntry> playerProfiles = new List<PlayerProfileEntry>();
    public List<PlayerProfileEntry> PlayerProfiles {  get { return playerProfiles; } }

    public void CreateProfile()
    {
        PlayerProfileEntry playerPrefab = Instantiate(PlayerProfile, currentPlayer.transform).GetComponent<PlayerProfileEntry>();

        if ( playerPrefab == null )
        {
            return;
        }
        playerPrefab.transform.parent = currentPlayer.transform;
        playerProfiles.Add(playerPrefab);
    }
}
