using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BaseGameScene : MonoBehaviourPunCallbacks
{
    // 게임 씬
    [SerializeField] List<AISpawner> aiSpanwers = new List<AISpawner>();
    [SerializeField] CheckGameState checkGameState;
    [SerializeField] List<Player> players;
    [SerializeField] Image fade;
    [SerializeField] float fadeTime = 2f;

    int loadCount = 0;
    int deathCount = 0;

    private IEnumerator Start()
    {
        checkGameState.CurState = GameState.InitGame;
        deathCount = 0;
        loadCount = 0;
        fade.gameObject.SetActive(true);
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1f);

        if (checkGameState == null)
        {   // 비어 있으면 하나는 찾아야 함
            checkGameState = FindObjectOfType<CheckGameState>();
        }

        // 현재 플레이어들
        players = PhotonNetwork.PlayerList.ToList();
        foreach (Player player in players)
        {   // 시작 시 플레이어의 상태는 살아있는 상태로
            player.SetState(PlayerState.Live);
        }

        // 플레이어 스폰
        yield return PlayerSpawn();
        // AI 스폰
        yield return SpawnRoutine();

        yield return FadeIn();
        checkGameState.CurState = GameState.GameProgress;
    }

    private IEnumerator SpawnRoutine()
    {   // 실제 스폰 루틴
        if (PhotonNetwork.IsMasterClient)
        {   // 마스터 클라이언트는 AI를 스폰
            foreach (AISpawner aiSpawner in aiSpanwers)
            {
                aiSpawner.Spawn();
            }
        }
        PhotonNetwork.LocalPlayer.SetLoad(true);
        yield return new WaitUntil(() => (loadCount == players.Count));
        yield return null;
    }

    private IEnumerator PlayerSpawn()
    {   // 플레이어 스폰루틴
        GameObject instance = PhotonNetwork.Instantiate("Player", transform.position, Quaternion.identity);

        instance.GetComponent<CharacterController>().enabled = false;
        instance.transform.position = new Vector3(transform.position.x, 1.4f, transform.position.z);
        instance.GetComponent<CharacterController>().enabled = true;

        yield return null;
    }

    IEnumerator FadeIn()
    {
        float rate = 0;
        Color fadeOutColor = new Color(fade.color.r, fade.color.g, fade.color.b, 1f);
        Color fadeInColor = new Color(fade.color.r, fade.color.g, fade.color.b, 0f);

        while (rate <= 1)
        {
            rate += Time.deltaTime / fadeTime;
            fade.color = Color.Lerp(fadeOutColor, fadeInColor, rate);
            yield return null;
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        if (changedProps.ContainsKey(CustomProperty.LOAD))
        {
            loadCount++;
        }
        else if(changedProps.ContainsKey(CustomProperty.PLAYERSTATE))
        {
            // 플레이어 상태 바뀐 것 체크
            deathCount++;

            if(deathCount >= players.Count - 1)
            {
                checkGameState.CurState = GameState.GameEnd;
            }
        }
    }

}
