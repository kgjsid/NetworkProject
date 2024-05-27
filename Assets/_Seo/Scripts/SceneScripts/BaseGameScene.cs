using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;
using System.Runtime.InteropServices.WindowsRuntime;

// 모든 데이터들이 관리 해주는 친구
public class BaseGameScene : MonoBehaviourPunCallbacks
{
    private static BaseGameScene instance;
    public static BaseGameScene Instance { get { return instance; } }

    [Header("Spanwer & SpawnPoint")]
    [SerializeField] protected List<AISpawner> aiSpanwers;
    [SerializeField] protected List<Transform> playerSpawnPoints;

    [SerializeField] protected CheckGameState checkGameState;
    public List<AIController> aiControllers; // AI관리하기위해
    public CheckGameState CheckGameState { get { return checkGameState; } }
    protected List<Player> players;
    public List<Player> Players { get { return players; } }

    [SerializeField] protected Image fade;
    [SerializeField] protected float fadeTime = 2f;
    [SerializeField] KillLogUI killLogUI;
    [SerializeField] protected PlayerList playerList;

    protected int loadCount = 0;
    protected int deathCount = 0;
    [SerializeField] protected string spawnName;
    public UnityEvent masterChangeEvent;

    // 한판 시간
    [SerializeField] protected GameTime gameTimeUI;

    protected virtual void Awake()
    {
        gameTimeUI = FindObjectOfType<GameTime>();
        if ( instance == null )
        {
            instance = this;
        }
        else
        {
            Destroy(instance.gameObject);
        }
    }

    protected virtual IEnumerator Start()
    {
        checkGameState.CurState = GameState.InitGame;
        deathCount = 0;
        loadCount = 0;

        fade.gameObject.SetActive(true);
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1f);

        if ( checkGameState == null )
        {   // 비어 있으면 하나는 찾아야 함
            checkGameState = FindObjectOfType<CheckGameState>();
        }

        // 현재 플레이어들
        int index = 0;
        players = PhotonNetwork.PlayerList.ToList();
        foreach ( Player player in players )
        {   // 시작 시 플레이어의 상태는 살아있는 상태로
            player.SetState(PlayerState.Live);
            playerList.CreateProfile();
            playerList.PlayerProfiles [index++].PlayerNickname(player);
        }
        // 플레이어 스폰
        yield return PlayerSpawn();
        // AI 스폰
        yield return SpawnRoutine();

        yield return FadeIn();

        StartCoroutine(GameOver());
        StartCoroutine(TimeOut());

        checkGameState.CurState = GameState.GameProgress;
    }

    protected virtual IEnumerator SpawnRoutine()
    {   // 실제 스폰 루틴
        if ( PhotonNetwork.IsMasterClient )
        {   // 마스터 클라이언트는 AI를 스폰
            foreach ( AISpawner aiSpawner in aiSpanwers )
            {
                aiSpawner.Spawn();
            }
        }
        PhotonNetwork.LocalPlayer.SetLoad(true);
        yield return new WaitUntil(() => ( loadCount == players.Count ));
        yield return null;
    }
    protected virtual IEnumerator PlayerSpawn()
    {   // 플레이어 스폰루틴
        int randPoint = Random.Range(0, playerSpawnPoints.Count);
        GameObject instance = PhotonNetwork.Instantiate(spawnName, Vector3.zero, Quaternion.identity);

        instance.GetComponent<CharacterController>().enabled = false;
        instance.transform.position = new Vector3(playerSpawnPoints [randPoint].position.x, 1.4f, playerSpawnPoints [randPoint].position.z);
        instance.GetComponent<CharacterController>().enabled = true;

        yield return null;
    }
    protected virtual IEnumerator FadeIn()
    {
        float rate = 0;
        Color fadeOutColor = new Color(fade.color.r, fade.color.g, fade.color.b, 1f);
        Color fadeInColor = new Color(fade.color.r, fade.color.g, fade.color.b, 0f);

        while ( rate <= 1 )
        {
            rate += Time.deltaTime / fadeTime;
            fade.color = Color.Lerp(fadeOutColor, fadeInColor, rate);
            yield return null;
        }

        fade.gameObject.SetActive(false);
        // 화면 전환끝나고 타이머시작
        photonView.RPC("StartTime", RpcTarget.AllViaServer);
    }

    public override void OnPlayerPropertiesUpdate( Player targetPlayer, PhotonHashtable changedProps )
    {
        if ( changedProps.ContainsKey(CustomProperty.LOAD) )
        {
            loadCount++;
        }
        else if ( changedProps.ContainsKey(CustomProperty.PLAYERSTATE) )
        {
            if ((PlayerState)changedProps [CustomProperty.PLAYERSTATE] != PlayerState.Die)
                return;

            foreach ( PlayerProfileEntry entry in playerList.PlayerProfiles )
            {
                entry.playerDied(targetPlayer);
            }

            deathCount++;
            if ( deathCount >= players.Count - 1 )
            {
                // if (PhotonNetwork.CurrentRoom.GetMode() != GameMode.Item)
                checkGameState.CurState = GameState.GameEnd;
            }
        }
    }

    public override void OnMasterClientSwitched( Player newMasterClient )
    {
        masterChangeEvent?.Invoke();
    }

    // 시간 동기화
    [PunRPC]
    public void StartTime()
    {
        gameTimeUI.StartTimer();
    }

    protected IEnumerator GameOver()
    {
        // 조건 
        // 1. 플레이어가 한명 남았을때
        // 2. 시간이 다 지났으면 AI를 다 없애주기
        // 게임상태가 끝났을때
        // 게임 상태를 해보았으나 처음에 End상태로 가서 바꿈
        while ( true )
        {
            if ( checkGameState.CurState == GameState.GameEnd )
            {
                if ( PhotonNetwork.CurrentRoom.GetMode() != GameMode.Item )
                {
                    if ( PhotonNetwork.LocalPlayer.GetState() == PlayerState.Live )
                    {
                        gameTimeUI.EndingImage();
                        gameTimeUI.Victory();
                    }
                    else if ( PhotonNetwork.LocalPlayer.GetState() == PlayerState.Die )
                    {
                        gameTimeUI.EndingImage();
                        gameTimeUI.Lose();
                    }
                }
                else
                {   // 추가로 킬카운트를 늘리기?
                    if (PhotonNetwork.LocalPlayer.GetState() == PlayerState.Live)
                    {
                        gameTimeUI.EndingImage();
                        gameTimeUI.Victory();
                    }
                    else if (PhotonNetwork.LocalPlayer.GetState() == PlayerState.Die)
                    {
                        gameTimeUI.EndingImage();
                        gameTimeUI.Lose();
                    }
                }
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    protected IEnumerator TimeOut()
    {
        // 시간이 다되면
        while ( true )
        {
            if ( gameTimeUI.Time == 0 )
            {
                foreach ( AIController aIController in aiControllers )
                {
                    //if (aIController.gameObject != null)
                        aIController.TakeDamage(10);
                }
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
