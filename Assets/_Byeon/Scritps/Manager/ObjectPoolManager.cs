using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    //풀링할 프리팹 목록
    [Header("AICreate")]
    [SerializeField] GameObject aiPrefab;
    [SerializeField] int aiPrefabCount;
    //스폰위치 배열에 접근 가능
    [SerializeField] AISpawnPos spawnPos;

    
    private void Awake()
    {
        //aiPrefab = Resources.Load<PooledObject>("AI");
        
    }

    private void Start()
    {
        AICreat(aiPrefab, aiPrefabCount);
    }

    private void AICreat(GameObject prefab, int size)
    {
        GameObject obejctParent = new GameObject($"{prefab.name}Pool");

        for (int i = 0; i < size; i++)
        {
            Quaternion randRot = Random.rotation;
            randRot.x = 0f; randRot.z = 0f;

            GameObject gameObject = Instantiate(prefab, spawnPos.SpawnPos[Random.Range(0, spawnPos.SpawnPos.Length)].position, randRot);
            gameObject.transform.SetParent(obejctParent.transform);
        }
    }

    private void ItemCreate()
    {
        /*Manager.Pool.CreatePool(itemPrefab, 64, 64);
        for(int i = 0; i < 64; i++)
        {
            Manager.Pool.GetPool(itemPrefab, aISpawnManager.SpawnPos[Random.Range(0, aISpawnManager.SpawnPos.Length)].position, transform.rotation);
        }*/
    }
}
