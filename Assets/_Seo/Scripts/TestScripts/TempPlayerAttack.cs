using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayerAttack : MonoBehaviourPun
{
    [SerializeField] float attackCoolTime;
    [SerializeField] AttackState curAttackState;
    [SerializeField] LayerMask attackLayer;

    [SerializeField] float attackTime = 0f;

    Coroutine AttackCoolTime;
    Collider[] colliders = new Collider[10];

    public AttackState CurAttackState { get { return curAttackState; } }

    [PunRPC]
    private void RequestAttack(int viewID)
    {   // 마스터에게 어택 요청

    }

    [PunRPC]
    private void ResultAttack()
    {   // 어택 결과를 반환

    }

    IEnumerator AttackCoolTimeCoroutine()
    {   // 어택 쿨타임 코루틴
        curAttackState = AttackState.AttackCoolTime;

        yield return new WaitForSeconds(attackCoolTime);

        curAttackState = AttackState.Attackable;
    }

    public void Attack()
    {   // 실제 어택 메소드 -> 마스터에게 공격 요청
        if (curAttackState != AttackState.Attackable)
        {   // 어택 중, 쿨타임에는 공격 요청불가
            return;
        }

        curAttackState = AttackState.BeginAttack;
        photonView.RPC("RequestAttack", RpcTarget.MasterClient, photonView.ViewID);
    }
}

public enum AttackState
{   // 어택 상태를 표현할 열거형
    Attackable,
    BeginAttack,
    Attacking,
    AttackCoolTime,
    EndAttack
}