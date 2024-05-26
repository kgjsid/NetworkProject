using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviourPun
{
    [SerializeField] Animator animator;
    [SerializeField] Slider coolTimeGauge;
    [SerializeField] int coolTime;
    [SerializeField] AudioClip attackSound;
    [SerializeField] PlayerController playerController;
    public bool canAttack = true;
    KillLogUI killLogUI;
    private void Awake()
    {
        killLogUI = FindObjectOfType<KillLogUI>();
        playerController = GetComponent<PlayerController>();
    }
    private void OnAttack( InputValue value )
    {   // 어택
        if ( canAttack == false )                             // 어택이 불가능한 상태라면
            return;

        if ( EventSystem.current.IsPointerOverGameObject() )  // UI가 가려져 있는 상태라면
            return;

        if ( playerController.IsDead )
            return;

        animator.SetTrigger("Attack");                      // 어택을 진행
        StartCoroutine(StartCoolTime());                    // 어택 쿨타임 코루틴 진행
    }

    private IEnumerator StartCoolTime()
    {   // 어택 쿨타임 코루틴
        canAttack = false;      // 어택은 불가능한 상태로
        float timePassed = 0f;  // 0초부터 시작하여 쿨타임 체크

        while ( timePassed < coolTime )
        {   // 쿨타임 측정
            timePassed += Time.deltaTime;
            coolTimeGauge.value = timePassed / coolTime; // Update the cool time gauge if needed
            yield return null;
        }
        coolTimeGauge.value = 1f;
        canAttack = true;       // 다시 어택은 가능한 상태로 전환
    }

    int killCount = 0;
    int curtargetID = 0;
    [PunRPC]
    public void KillLogNickName( int targetID )
    {
        // 모든 클라이언트에서 실행됨
        PhotonView targetView = PhotonView.Find(targetID);
        if ( targetView != null )
        {

            killCount = photonView.Controller.GetKillCount() + 1;
            photonView.Controller.SetKillCount(killCount);
            string targetName = targetView.Controller.NickName;
            if ( targetView.gameObject.layer == 3 )
            {
                if ( curtargetID != targetID )
                {
                    killLogUI.KillLog(photonView.Controller.NickName, targetName);
                    curtargetID = targetID;
                }
            }
            else
            {
                if ( curtargetID != targetID )
                {
                    killLogUI.KillLog(photonView.Controller.NickName, "AI");
                    curtargetID = targetID;
                }
            }
        }
    }
    public void KillLog( int targetID )
    {
        photonView.RPC("KillLogNickName", RpcTarget.All, targetID);
    }

}
