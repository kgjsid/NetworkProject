using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Slider coolTimeGauge;
    [SerializeField] int coolTime;
    [SerializeField] AudioClip attackSound;
    public bool canAttack = true;
    private void OnAttack(InputValue value)
    {   // 어택
        if (canAttack == false)                             // 어택이 불가능한 상태라면
            return;

        if (EventSystem.current.IsPointerOverGameObject())  // UI가 가려져 있는 상태라면
            return;

        animator.SetTrigger("Attack");                      // 어택을 진행
        StartCoroutine(StartCoolTime());                    // 어택 쿨타임 코루틴 진행
    }

    private IEnumerator StartCoolTime()
    {   // 어택 쿨타임 코루틴
        canAttack = false;      // 어택은 불가능한 상태로
        float timePassed = 0f;  // 0초부터 시작하여 쿨타임 체크

        while (timePassed < coolTime)
        {   // 쿨타임 측정
            timePassed += Time.deltaTime;
            coolTimeGauge.value = timePassed / coolTime; // Update the cool time gauge if needed
            yield return null;
        }
        coolTimeGauge.value = 1f;
        canAttack = true;       // 다시 어택은 가능한 상태로 전환
    }
}
