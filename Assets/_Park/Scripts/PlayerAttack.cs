using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Slider coolTimeGauge;
    [SerializeField] int coolTime;

    public bool canAttack = true;
    private void OnAttack(InputValue value)
    {
        if (canAttack == false)
        {
            return;
        }
        animator.SetBool("Attack02", true);
        StartCoroutine(StartCoolTime());
    }

    private IEnumerator StartCoolTime()
    {
        canAttack = false;
        float timePassed = 0f;

        while (timePassed < coolTime)
        {
            timePassed += Time.deltaTime;
            coolTimeGauge.value = timePassed / coolTime; // Update the cool time gauge if needed
            yield return null;
        }
        canAttack = true;
        coolTimeGauge.value = 1f;
    }
}
