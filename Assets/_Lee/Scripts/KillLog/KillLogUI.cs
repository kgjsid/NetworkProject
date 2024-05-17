using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class KillLogUI : MonoBehaviour
{
    [SerializeField] GameObject KillLogContent;
    [SerializeField] GameObject killLogPrefab;
    [SerializeField] GameObject killLogObject;
    public void KillDieLog( string die )
    {
        Debug.Log("킬로그 생성");
        killLogObject = Instantiate(killLogPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        killLogObject.transform.parent = KillLogContent.transform;
        killLogObject.GetComponent<KillLogPanel>().ChangeDie(die);
    }
    public void KillerLog(string killer )
    {
        killLogObject.GetComponent<KillLogPanel>().ChangKiller(killer);
    }
}
