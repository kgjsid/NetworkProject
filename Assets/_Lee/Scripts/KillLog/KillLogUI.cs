using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class KillLogUI : MonoBehaviour
{
    [SerializeField] GameObject KillLogContent;
    public void KillLog( string killer, string die )
    {
        Debug.Log("킬로그 생성");
        GameObject killLogObject = PhotonNetwork.Instantiate("KillLogPanel", new Vector3(0, 0, 0), Quaternion.identity);
        killLogObject.transform.parent = KillLogContent.transform;
        killLogObject.GetComponent<KillLogPanel>().changeLog(killer, die);
    }
}
