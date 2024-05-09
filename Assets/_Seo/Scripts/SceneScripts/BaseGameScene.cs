using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameScene : MonoBehaviourPunCallbacks
{
    // 게임 씬(매니저)

    [SerializeField] List<AISpawner> aiSpanwers = new List<AISpawner>();
    [SerializeField] CheckGameState checkGameState;
    [SerializeField] List<GameObject> players = new List<GameObject>();
    
    private IEnumerator Start()
    {   
        if (checkGameState == null)
        {   // 비어 있으면 하나는 찾아야 함
            checkGameState = FindObjectOfType<CheckGameState>();
        }
        GameObject instance = PhotonNetwork.Instantiate("Player", transform.position, Quaternion.identity);

        instance.GetComponent<CharacterController>().enabled = false;
        instance.transform.position = new Vector3(transform.position.x, 1.4f, transform.position.z);
        instance.GetComponent<CharacterController>().enabled = true;

        if (PhotonNetwork.IsMasterClient)
        {
            checkGameState.CurState = GameState.InitGame;
            foreach (AISpawner spawner in aiSpanwers)
            {   // 모든 스포너에 대하여 스폰 루틴 진행
                yield return SpawnRoutine(spawner);
            }

            yield return null;
        }
        checkGameState.CurState = GameState.GameProgress;
    }

    private IEnumerator SpawnRoutine(AISpawner spawner) 
    {   // 실제 스폰 루틴
        spawner.Spawn();

        yield return null;
    }

}
