using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AIController : MonoBehaviour, IDamageable,IPointerClickHandler
{
    private Animation aiAnimation;
    private AIMove aiMove;
    private ObjectPoolManager objectPoolManager;
    private AISpawnPos aiSpawnPos;

    private void Awake()
    {
        aiAnimation = GetComponent<Animation>();
        aiMove = GetComponent<AIMove>();
    }
    private void Start()
    {
        
    }

    public void TakeDamage()
    {
        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TakeDamage();
    }
}
