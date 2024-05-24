using Photon.Pun;
using System.Collections;
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


    [SerializeField] int hp; //AI 체력
    public int Hp { get { return hp; } }



    private void Awake()
    {
        if (BaseGameScene.Instance != null)
        {
            BaseGameScene.Instance.aiControllers.Add(this);
        }
        animator = GetComponent<Animator>();
        aiMove = GetComponent<AIMove>();
        itemAImove = GetComponent<ItemAIMove>();
        hp = 1;
        state = AIstate.Idle;
    }

    private void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        if (hp - damage < 0)
        {
            hp = 0;
            Die();
        }
        else
        {
            hp -= damage;
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
        

        state = AIstate.Die;
        animator.SetTrigger("Die");
        StartCoroutine(DieDelay());
    }

    IEnumerator DieDelay()
    {
        yield return new WaitForSeconds(3f);
        if (photonView.IsMine)
            PhotonNetwork.Destroy(gameObject);
    }


    //플레이어 애니메이션 이벤트용 
    public void PlayerMove()
    {

    }
    public void PlayerNotMove()
    {

    }

    /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //포톤뷰를 가진 게임오브젝트에 접근하는 방법
        //Rigidbody rigid = PhotonView.Find(photonView.ViewID).GetComponent<Rigidbody>(); 

        //변수동기화
        if (stream.IsWriting) // photonView.IsMine 내가 로컬일때
        {
            // 네트워크로 보내기
            stream.SendNext(currentSpeed);
        }
        else // steam.IsReading , !photonView.IsMine 내가 리모트일때
        {
            // 네크워크에서 받기 , 보낸 순서대로 받아야한다.
            currentSpeed = (float)stream.ReceiveNext();
        }
    }*/
}
