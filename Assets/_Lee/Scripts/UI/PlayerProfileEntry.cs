using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfileEntry : MonoBehaviourPun
{
    [SerializeField] TMP_Text playerName;
    [SerializeField] Image playerDie;
    [SerializeField] Image playerLive;

    [SerializeField] Player player;

    private void OnEnable()
    {
        playerLive.gameObject.SetActive(true);
        playerDie.gameObject.SetActive(false);
    }
    public void PlayerNickname( Player player, string Nickname )
    {
        this.player = player;
        playerName.text = Nickname;
    }
    public void playerDied( Player player )
    {
        if ( this.player == player )
        {
            playerLive.gameObject.SetActive(false);
            playerDie.gameObject.SetActive(true);
        }
    }
}
