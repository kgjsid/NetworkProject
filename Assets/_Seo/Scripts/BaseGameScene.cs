using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameScene : BaseScene
{
    [SerializeField] List<AISpawner> aiSpanwers = new List<AISpawner>();


    public override IEnumerator LoadingRoutine()
    {


        yield return null;
    }
}
