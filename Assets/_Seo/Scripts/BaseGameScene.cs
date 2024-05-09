using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameScene : BaseScene
{
    [SerializeField] List<AISpawner> aiSpanwers = new List<AISpawner>();
    [SerializeField] CheckGameState checkGameState;

    // 플레이어
    [SerializeField] List<GameObject> players = new List<GameObject>();
    // 죽은 플레이어
    [SerializeField] List<GameObject> deathPlayers = new List<GameObject>();

    /*
    private IEnumerator Start()
    {   // 테스트용 Start -> 나중에 로딩 루틴으로 갈아타야 함
        checkGameState.CurState = GameState.InitGame;
        foreach (AISpawner spawner in aiSpanwers)
        {   // 모든 스포너에 대하여 스폰 루틴 진행
            yield return SpawnRoutine(spawner);
        }

        yield return null;
        checkGameState.CurState = GameState.GameProgress;
    }
    */

    public override IEnumerator LoadingRoutine()
    {   // 로딩 루틴
        if(checkGameState == null)
        {   // 비어 있으면 하나는 찾아야 함
            checkGameState = FindObjectOfType<CheckGameState>();
        }

        checkGameState.CurState = GameState.InitGame;
        foreach(AISpawner spawner in aiSpanwers)
        {   // 모든 스포너에 대하여 스폰 루틴 진행
            yield return SpawnRoutine(spawner);
        }

        yield return null;
        checkGameState.CurState = GameState.GameProgress;
    }

    private IEnumerator SpawnRoutine(AISpawner spawner) 
    {   // 실제 스폰 루틴
        spawner.Spawn();

        yield return null;
    }

}
