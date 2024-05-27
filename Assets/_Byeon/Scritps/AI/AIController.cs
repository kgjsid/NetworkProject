using Photon.Pun;
using System.Collections;
using System.Net;
using UnityEngine;

public class AIController : MonoBehaviourPun, IDamageable//, IPunObservable
{
    public enum AIstate { Idle, Walk, Run, Attack, Emote, Die }

    private AIstate state;
    public AIstate State { get { return state; } set { state = value; } }

    private Animator animator;
    public Animator Animator { get { return animator; } }

    private AIMove aiMove;
    private ItemAIMove itemAImove;
    private CapsuleCollider capsuleCollider;


    [SerializeField] int hp; //AI 체력
    public int Hp { get { return hp; } }

    bool timeOver;

    private void Awake()
    {
        if (BaseGameScene.Instance != null)
        {
            BaseGameScene.Instance.aiControllers.Add(this);

        }
        animator = GetComponent<Animator>();
        aiMove = GetComponent<AIMove>();
        itemAImove = GetComponent<ItemAIMove>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        hp = 1;
        state = AIstate.Idle;
    }

    private void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        if(hp != 0)
        {
            if (damage != 10)
            {
                timeOver = true;
            }
            else
            {
                timeOver = false;
            }
            
            if (hp - damage <= 0)
            {
                hp = 0;
                Die();
            }
            else
            {
                hp -= damage;
            }
        }
        
    }

    private void Die()
    {
        //죽은 ai의 이동속도, 회전속도 0으로 해서 네비메시 이동 정지
        if(aiMove != null)
        {
            aiMove.Agent.speed = 0;
            aiMove.Agent.angularSpeed = 0;
        }
        if(itemAImove != null)
        {
            itemAImove.Agent.speed = 0;
            itemAImove.Agent.angularSpeed = 0;
        }

        //콜라이더 꺼서 킬로그 반응 끄기
        capsuleCollider.enabled = false;

        state = AIstate.Die;
        animator.SetTrigger("Die");

        //마스터클라이언트에서 전체에 전송하여 삭제
        photonView.RPC("RequestDie", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void RequestDie()
    {
        if (!timeOver)
        {
            BaseGameScene.Instance.aiControllers.Remove(gameObject.GetComponent<AIController>());
        }

        photonView.RPC("ResultAIDie", RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void ResultAIDie()
    {
        if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(gameObject);
    }


    //플레이어 애니메이션 이벤트용 
    public void PlayerMove()
    {

    }
    public void PlayerNotMove()
    {

    }
}
