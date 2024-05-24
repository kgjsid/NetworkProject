using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, IShieldable
{
    public void Shielding()
    {   // 일단 꺼보기
        // 추후에 깨지는 이펙트나 소리도 넣으면 좋음
        gameObject.SetActive(false);
    }
}
