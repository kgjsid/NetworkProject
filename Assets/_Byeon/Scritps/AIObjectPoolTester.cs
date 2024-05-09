using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIObjectPoolTester : MonoBehaviour
{
    [SerializeField] PooledObject pooledObject;
    [SerializeField] int size;
    [SerializeField] int capacity;

    private void Awake()
    {
        size = 4;
        capacity = 4;
    }

    private void Start()
    {
        Manager.Pool.CreatePool(pooledObject,size,capacity);
    }
}
