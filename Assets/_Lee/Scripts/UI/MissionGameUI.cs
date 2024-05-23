using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionGameUI : MonoBehaviour
{
    private static MissionGameUI instance;
    public static MissionGameUI Instance { get { return instance; } }

    [SerializeField] GameObject GameUI;

    [SerializeField] KillLogPanel killLogUI;
    [SerializeField] GameTime gameTime;


    private void Awake()
    {
        if ( instance == null )
        {
            instance = this;
        }
        GameUI.SetActive(true);

    }
    private void Start()
    {
        killLogUI = GetComponentInChildren<KillLogPanel>();
        gameTime = GetComponentInChildren<GameTime>();
    }
    public void EndGame()
    {
        GameUI.SetActive(false);
    }

}
