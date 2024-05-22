using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameUI : MonoBehaviour
{
    private static BaseGameUI instance;
    public static BaseGameUI Instance {  get { return instance; } }
    [SerializeField] GameObject GameUI;
    private void Awake()
    {
        if ( instance == null )
        {
            instance = this;
        }
        GameUI.SetActive(true);

    }

    public void EndGame()
    {
        GameUI.SetActive( false );
    }
}
