using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using static AIController;

public class AIController : MonoBehaviour, IDamageable//,IPointerClickHandler
{
    public enum AIstate { Idle, Walk, Die}

    AIstate aiState;

    private Animator animator;
    public Animator Animator {  get { return animator; } }

    private AIMove aiMove;

    private ObjectPoolManager objectPoolManager;
    private AISpawnPos aiSpawnPos;

    [SerializeField] int hp; //AI 체력



    private void Awake()
    {
        animator = GetComponent<Animator>();
        aiMove = GetComponent<AIMove>();
        hp = 1;
    }
    private void Start()
    {
        aiState = AIstate.Idle;
    }

    public void TakeDamage(int damage)
    {
        if (hp - damage < 0)
        {
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
        aiMove.Agent.speed = 0;
        aiMove.Agent.angularSpeed = 0;

        aiState = AIstate.Die;
        animator.SetTrigger("Die");
        StartCoroutine(DieDelay());
    }

    IEnumerator DieDelay()
    {
        yield return new WaitForSeconds(3f);
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
