using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KillLogUI : MonoBehaviour
{
    [SerializeField] GameObject KillLogContent;
    [SerializeField] GameObject killLogPrefab;
    GameObject killLogObject;
    Queue<GameObject> killLogQueue;
    private void Awake()
    {
        killLogQueue = new Queue<GameObject>();
    }
    public void KillLog( string killer, string die )
    {
        Debug.Log("킬로그 생성");
        killLogObject = Instantiate(killLogPrefab, KillLogContent.transform);
        killLogQueue.Enqueue(killLogObject);
        killLogObject.transform.parent = KillLogContent.transform;
        killLogObject.GetComponent<KillLogPanel>().changeLog(killer, die);
        if ( killLogQueue.Count > 8 )
        {
            killLogQueue.Dequeue().gameObject.SetActive(false);
        }
        StartCoroutine(KillLogCool());
    }

    IEnumerator KillLogCool()
    {
        yield return new WaitForSeconds(3);
        killLogQueue.Dequeue().gameObject.SetActive(false);
    }
}
