using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGameState : MonoBehaviour
{   // 게임의 진행상황 체크용
    [SerializeField] GameState curState;   // 현재 상태

    Coroutine curRoutine;

    public GameState CurState
    {
        get { return curState; }
        set
        {
            curState = value;

            if (curRoutine != null)
                StopCoroutine(curRoutine);

            switch (curState)
            {
                case GameState.InitGame:
                    curRoutine = StartCoroutine(InitGameRoutine());
                    break;
                case GameState.GameProgress:
                    curRoutine = StartCoroutine(GameProgressRoutine());
                    break;
                case GameState.GameEnd:
                    curRoutine = StartCoroutine(GameEndRoutine());
                    break;
            }
        }
    }

    private void Awake()
    {
    }

    IEnumerator InitGameRoutine()
    {   // 게임 초기화 진행 중...
        while (true)
        {
            Debug.Log("초기화 진행 중");
            yield return null;
        }
    }
    IEnumerator GameProgressRoutine()
    {   // 게임 진행 상태
        while (true)
        {
            Debug.Log("게임 진행 중");
            yield return null;
        }
    }
    IEnumerator GameEndRoutine()
    {   // 게임 끝
        while (true)
        {
            yield return null;
        }
    }
}

public enum GameState
{   // 현재 게임의 상태 -> 초기화 진행, 게임 상태, 게임 끝
    InitGame,
    GameProgress,
    GameEnd
}