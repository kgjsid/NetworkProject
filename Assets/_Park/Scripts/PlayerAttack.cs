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
    {
        if (canAttack == false)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        animator.SetTrigger("Attack");
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
        coolTimeGauge.value = 1f;
        canAttack = true;
    }
}
