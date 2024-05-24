using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, IShieldable
{
    [SerializeField] PlayerItemController user;
    Coroutine shieldRoutine;

    private void OnEnable()
    {
        StopAllCoroutines();
        shieldRoutine = StartCoroutine(ShieldRoutine());
    }

    public void SetUser(PlayerItemController user)
    {
        this.user = user;
    }

    public void Shielding()
    {   // 일단 꺼보기
        // 추후에 깨지는 이펙트나 소리도 넣으면 좋음
        StopAllCoroutines();
    }

    private IEnumerator ShieldRoutine()
    {
        Debug.Log("쉴드 루틴");
        user.Controller.IsShield = true;
        yield return new WaitForSeconds(5f);
        user.Controller.IsShield = false;
        gameObject.SetActive(false);
    }
}
